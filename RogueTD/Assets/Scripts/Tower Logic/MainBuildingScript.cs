using System;
using UnityEngine;

public class MainBuildingScript : MonoBehaviour
{
    private void Start()
    {
        Building.onBuildingDestroyed += Destroyed;
    }


    private void Destroyed(Vector2Int position)
    {
        if (position == Vector2Int.zero)
        {
            UIEndScreen.Instance.EndGame("You Lose");
        }
    }
}
