using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameState
{
    private static string SAVE_FOLDER = Path.Combine(Application.dataPath, "Saves");
    private static string SAVE_FILE_PATH = Path.Combine(SAVE_FOLDER, "gamestate.json");
    private static string BUILDINGS_SAVE_PATH = Path.Combine(SAVE_FOLDER, "buildings.json");
    private static string TREE_SAVE_PATH = Path.Combine(SAVE_FOLDER, "research_tree.json");

    public static GameState Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameState();
            }
            return _instance;
        }
    }
    private static GameState _instance;
    
    private bool isANewRun;
    private ResearchTree.TreeSaveData treeSaveData;
    private List<BuildingSaveData> buildings = new List<BuildingSaveData>();
    private int _currency;
    private int _wave;
    private EnemyWave currentWave;
    private GameState() 
    {
        isANewRun = true;
    }
    
    public bool IsANewRun 
    { 
        get => isANewRun; 
        set  
        {
            isANewRun = value;
            SaveToJson();
        }
    }
    
    public ResearchTree.TreeSaveData TreeSaveData 
    { 
        get => treeSaveData; 
        set  
        {
            treeSaveData = value;
            SaveTreeToJson();
        }
    }
    
    public EnemyWave CurrentWave
    {
        get => currentWave;
        set
        {
            currentWave = value;
        }
    }
    
    public List<BuildingSaveData> Buildings 
    { 
        get => buildings; 
        set 
        {
            buildings = value ?? new List<BuildingSaveData>(); 
        }
    }

    public int Wave
    {
        get => _wave;
        set
        {
            if (value <= 0) return;
            
            _wave = value;
            OnWaveChanged?.Invoke(_wave);
        }
    }

    public int Currency
    {
        get => _currency;
        private set
        {
            _currency = value;
            OnCurrencyChanged?.Invoke(_currency);
        }
    }

    public Action<int> OnWaveChanged;
    public Action<int> OnCurrencyChanged;


    public void ChangeWave(int newWave)
    {
        if (newWave < 0) return;
        
        _wave = newWave;
        OnWaveChanged?.Invoke(_wave);
    }
    
    public void IncrementWave()
    {
        Wave = _wave + 1;
    }
    
    public void ResetWave()
    {
        Wave = 0;
    }

    public void ChangeCurrency(int amount)
    {
        int newCurrency = _currency + amount;
        _currency = newCurrency;
        OnCurrencyChanged?.Invoke(_currency);
    }
    
    public void AddCurrency(int amount)
    {
        if (amount < 0) return;
        ChangeCurrency(amount);
    }
    
    public bool SpendCurrency(int amount)
    {
        if (amount < 0) return false;
        
        if (HasEnoughCurrency(amount))
        {
            ChangeCurrency(-amount);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool HasEnoughCurrency(int amount)
    {
        return _currency >= amount;
    }
    
    public void ResetGameState()
    {
        Buildings = new List<BuildingSaveData>();
        CurrentWave = null;
        _currency = 0;
        _wave = 0;
        isANewRun = true;
        treeSaveData = null;
        
        DeleteAllSaveFiles();
    }
    
    public void Initialize(int startingCurrency = 100, int startingWave = 0)
    {
        _currency = Mathf.Max(0, startingCurrency);
        _wave = Mathf.Max(0, startingWave);
    }

    [Serializable]
    private class GameStateSaveData
    {
        public int Currency;
        public int Wave;
        public bool IsANewRun;
        public string CurrentWaveJson;
    }

    [Serializable]
    private class BuildingsSaveData
    {
        public List<BuildingSaveData> Buildings = new List<BuildingSaveData>();
    }

    private JsonSerializerSettings GetJsonSettings()
    {
        return new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            Converters = new List<JsonConverter>
            {
                new Vector2Converter() 
            }
        };
    }

    public void SaveToJson()
    {
        try
        {
            var saveData = new GameStateSaveData
            {
                Currency = _currency,
                Wave = _wave,
                CurrentWaveJson = currentWave != null ? JsonConvert.SerializeObject(currentWave, GetJsonSettings()) : ""
            };

            string json = JsonConvert.SerializeObject(saveData, GetJsonSettings());
            File.WriteAllText(SAVE_FILE_PATH, json);
            Debug.Log($"GameState сохранен в: {SAVE_FILE_PATH}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка сохранения GameState: {e.Message}");
        }
    }

    public void SaveBuildingsToJson()
    {
        try
        {
            UpdateBuildingsHealth();
        
            var saveData = new BuildingsSaveData
            {
                Buildings = buildings
            };

            string json = JsonConvert.SerializeObject(saveData, GetJsonSettings());
            File.WriteAllText(BUILDINGS_SAVE_PATH, json);
            Debug.Log("Постройки сохранены");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void UpdateBuildingsHealth()
    {
        foreach (var saveData in buildings)
        {
            if (ConstructionGridManager.BuildingsPos.TryGetValue(saveData.Position, out Building building))
            {
                saveData.CurrentHealth = building.CurrentHealthPoints;
            }
        }
    }

    public void SaveTreeToJson()
    {
        try
        {
            if (treeSaveData == null) return;

            string json = JsonConvert.SerializeObject(treeSaveData, GetJsonSettings());
            File.WriteAllText(TREE_SAVE_PATH, json);
            Debug.Log($"Research Tree сохранен в: {TREE_SAVE_PATH}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка сохранения Research Tree: {e.Message}");
        }
    }

    public void SaveAll()
    {
        SaveToJson();
        SaveBuildingsToJson();
        SaveTreeToJson();
    }

    public bool LoadGameState()
    {
        if (!File.Exists(SAVE_FILE_PATH))
        {
            Debug.Log($"Файл GameState не найден: {SAVE_FILE_PATH}");
            return false;
        }

        try
        {
            string json = File.ReadAllText(SAVE_FILE_PATH);
            var saveData = JsonConvert.DeserializeObject<GameStateSaveData>(json, GetJsonSettings());
            
            if (saveData != null)
            {
                _currency = saveData.Currency;
                _wave = saveData.Wave;
                
                if (!string.IsNullOrEmpty(saveData.CurrentWaveJson))
                {
                    currentWave = JsonConvert.DeserializeObject<EnemyWave>(saveData.CurrentWaveJson, GetJsonSettings());
                }
                
                Debug.Log($"GameState загружен из: {SAVE_FILE_PATH}");
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка загрузки GameState: {e.Message}");
        }
        
        return false;
    }

    public bool LoadBuildings()
    {
        if (!File.Exists(BUILDINGS_SAVE_PATH))
        {
            Debug.Log($"Файл Buildings не найден: {BUILDINGS_SAVE_PATH}");
            return false;
        }

        try
        {
            string json = File.ReadAllText(BUILDINGS_SAVE_PATH);
            var saveData = JsonConvert.DeserializeObject<BuildingsSaveData>(json, GetJsonSettings());
            
            if (saveData != null)
            {
                buildings = saveData.Buildings ?? new List<BuildingSaveData>();
                ConstructionGridManager.SavePoses = Buildings;
                SaveBuildingsToJson();
                return true;
            }
            
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка загрузки Buildings: {e.Message}");
        }
        
        return false;
    }

    public bool LoadResearchTree()
    {
        if (!File.Exists(TREE_SAVE_PATH))
        {
            Debug.Log($"Файл Research Tree не найден: {TREE_SAVE_PATH}");
            return false;
        }

        try
        {
            string json = File.ReadAllText(TREE_SAVE_PATH);
            treeSaveData = JsonConvert.DeserializeObject<ResearchTree.TreeSaveData>(json, GetJsonSettings());
            
            Debug.Log($"Research Tree загружен из: {TREE_SAVE_PATH}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка загрузки Research Tree: {e.Message}");
        }
        
        return false;
    }

    public bool LoadAll()
    {
        bool gameStateLoaded = LoadGameState();
        bool buildingsLoaded = LoadBuildings();
        bool treeLoaded = LoadResearchTree();
        
        return gameStateLoaded || buildingsLoaded || treeLoaded;
    }

    public void DeleteAllSaveFiles()
    {
        TryDeleteFile(SAVE_FILE_PATH);
        TryDeleteFile(BUILDINGS_SAVE_PATH);
        TryDeleteFile(TREE_SAVE_PATH);
        Debug.Log("Все файлы сохранения удалены");
    }

    private void TryDeleteFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"Файл удален: {path}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка удаления файла {path}: {e.Message}");
        }
    }

    public bool HasSavedData()
    {
        return File.Exists(SAVE_FILE_PATH) || 
               File.Exists(BUILDINGS_SAVE_PATH) || 
               File.Exists(TREE_SAVE_PATH);
    }

}