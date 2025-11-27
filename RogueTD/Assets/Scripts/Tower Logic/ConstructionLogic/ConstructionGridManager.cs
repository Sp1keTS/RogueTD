using System.Collections.Generic;
using UnityEngine;

public static class ConstructionGridManager
{
    static public Grid constructionGrid;
    static public Dictionary<Vector2,Building> buildingsSpace = new Dictionary<Vector2,Building>();
    static public Dictionary<Vector2, Building> buildingsPos = new Dictionary<Vector2,Building>();
}
