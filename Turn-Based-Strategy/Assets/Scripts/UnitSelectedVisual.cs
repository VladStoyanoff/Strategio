using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] Unit unit;

    [Header("Components")]
    MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
    }

    void Start()
    {
        UpdateVisual();
    }

    void UnitSelectedVisual_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        meshRenderer.enabled = UnitActionSystem.Instance.GetSelectedUnit() == unit;
    }

    void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitSelectedVisual_OnSelectedUnitChanged;
    }
}
