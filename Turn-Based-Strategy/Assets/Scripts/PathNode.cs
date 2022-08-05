using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    GridPosition gridPosition;
    int gCost;
    int hCost;
    int fCost;
    PathNode cameFromPathNode;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }

    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;
    }

    public override string ToString() => gridPosition.ToString();
    public int GetGCost() => gCost;
    public int GetHCost() => hCost;
    public int GetFCost() => fCost;
}