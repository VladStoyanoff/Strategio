using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    void Start()
    {
        InitializationStart();
    }

    void InitializationStart()
    {
        DestructableCrate.OnAnyDestroyed += DestructableCrate_OnAnyDestroyed;
    }

    void DestructableCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructableCrate destructableCrate = sender as DestructableCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructableCrate.GetGridPosition(), true);
    }
}
