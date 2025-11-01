using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Tile", menuName = "2D/Tiles/Custom Tile")]
public class CustomTile : Tile
{
    public TileType tileType;
    public float speedMultiplier = 1.0f;
    public bool isWalkable = true;
}