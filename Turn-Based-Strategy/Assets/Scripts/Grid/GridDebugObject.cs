 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshPro textMeshPro;

    [Header("Grid")]
    object gridObject;

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    protected virtual void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }
}
