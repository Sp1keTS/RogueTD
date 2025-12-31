using UnityEngine;
[System.Serializable]
public class Resource : ScriptableObject
{
    public string name;
    public string SetRankedName(int rank)
    {
        name += $"-rank-{rank}";
        return name;
    }
}
