using UnityEngine;

public class Resource : ScriptableObject
{
    
    public string SetRankedName(int rank)
    {
        name += $"-rank-{rank}";
        return name;
    }
}
