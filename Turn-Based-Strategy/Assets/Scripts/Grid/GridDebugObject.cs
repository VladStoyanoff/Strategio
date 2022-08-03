 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshPro textMeshPro;

    [Header("Grid")]
    GridObject gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }
}
