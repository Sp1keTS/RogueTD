using UnityEngine;

public class Resource : ScriptableObject
{
    
    public void SetRankedName(int rank)
    {
        name += $"-rank-{rank}";
    }
}
