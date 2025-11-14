using System.Collections.Generic;
using UnityEngine;

public static class ConstructionGridManager
{
    static public Grid constructionGrid;
    static public Dictionary<Vector2,Building> buildings = new Dictionary<Vector2,Building>();
}
