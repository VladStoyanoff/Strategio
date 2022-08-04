using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actionPointsText;
    [SerializeField] Unit unit;
    [SerializeField] Image healthBarImage;
    [SerializeField] HealthSystem healthSystem;

    void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        UpdateActionPointsText();
        UpdateHealthBar();
    }
    void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }
    
    void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
