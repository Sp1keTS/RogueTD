using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class Vector2Converter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericType && 
               objectType.GetGenericTypeDefinition() == typeof(SerializedDictionary<,>) &&
               objectType.GetGenericArguments()[0] == typeof(Vector2);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var dict = new SerializedDictionary<Vector2, SerializedDictionary<string, int>>();
        var jObject = JObject.Load(reader);
        
        foreach (var property in jObject.Properties())
        {
            var key = ParseVector2(property.Name);
            var value = property.Value.ToObject<SerializedDictionary<string, int>>(serializer);
            dict[key] = value;
        }
        
        return dict;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var dict = (SerializedDictionary<Vector2, SerializedDictionary<string, int>>)value;
        writer.WriteStartObject();
        
        foreach (var kvp in dict)
        {
            writer.WritePropertyName($"({kvp.Key.x:F2}, {kvp.Key.y:F2})");
            serializer.Serialize(writer, kvp.Value);
        }
        
        writer.WriteEndObject();
    }

    private Vector2 ParseVector2(string str)
    {
        try
        {
            str = str.Trim('(', ')', ' ');
            var parts = str.Split(',');
            
            if (parts.Length == 2)
            {
                if (float.TryParse(parts[0].Trim(), out float x) && 
                    float.TryParse(parts[1].Trim(), out float y))
                {
                    return new Vector2(x, y);
                }
            }
        }
        catch
        {
        }
        
        return Vector2.zero;
    }
}