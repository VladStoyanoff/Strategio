using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform actionButtonPrefab;
    [SerializeField] Transform actionButtonContainerTransform;
    [SerializeField] TextMeshProUGUI actionPointsText;

    List<ActionButtonUI> actionButtonUIList;

    void Awake()
    {
        InitializationAwake();
    }

    void Start()
    {
        InitializationStart();
    }

    void InitializationAwake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    void InitializationStart()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        ActionPoints();
        CreateUnitActionButtons();
        SelectedVisual();
    }

    void CreateUnitActionButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform  actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);

        }
    }

    void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        SelectedVisual();
        ActionPoints();
    }

    void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        SelectedVisual();
    }

    void UnitActionSystem_OnActionStarted(object sende, EventArgs e)
    {
        ActionPoints();
    }

    void SelectedVisual()
    {
        foreach(ActionButtonUI actionbuttonUI in actionButtonUIList)
        {
            actionbuttonUI.UpdateSelectedVisual();
        }
    }
    
    void ActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        ActionPoints();
    }

    void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        ActionPoints();
    }
}
