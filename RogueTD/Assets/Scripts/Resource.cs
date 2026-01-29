using UnityEngine;
[System.Serializable]
public class Resource
{
    public string name;
    public Resource()
    {
        name = GetType().Name; 
    }
    public string SetRankedName(int rank)
    {
        name += $"-rank-{rank}";
        return name;
    }
}
