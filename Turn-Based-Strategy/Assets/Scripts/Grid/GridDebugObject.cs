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

    protected virtual void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        textMeshPro.text = gridObject.ToString();
    }

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }
}
