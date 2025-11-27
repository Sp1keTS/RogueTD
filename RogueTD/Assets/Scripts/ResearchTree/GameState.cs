using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Tower Defense/Game State")]
public class GameState : ScriptableObject
{
    public ResearchTree.TreeSaveData TreeSaveData {get; set; }

}
