using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    BaseAction selectedAction;
    bool isBusy;


    [Header("Game Objects")]
    [SerializeField] Unit selectedUnit;

    [Header("Layers")]
    [SerializeField] LayerMask unitsLayerMask;

    void Awake()
    {
        SetInstance();
    }

    void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    void Update()
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        UpdateClick();
        HandleSelectedAction();
    }

    void UpdateClick()
    {
        if (isBusy) return;
        if (TryHandleUnitSelection()) return;
    }

    void HandleSelectedAction()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
        if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) return;
        SetBusyBoolToTrue();
        selectedAction.TakeAction(mouseGridPosition, SetBusyBoolToFalse);

        OnActionStarted?.Invoke(this, EventArgs.Empty);
    }

    void SetInstance()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    bool TryHandleUnitSelection()
    {
        if (!Input.GetMouseButton(0) ||
            !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, float.MaxValue, unitsLayerMask) ||
            !raycastHit.transform.TryGetComponent<Unit>(out Unit unit) ||
            unit == selectedUnit ||
            unit.IsEnemy())
        {
            return false;
        }
        SetSelectedUnit(unit);
        return true;
    }

    void SetBusyBoolToTrue()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    void SetBusyBoolToFalse()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);

    }

    public Unit GetSelectedUnit() => selectedUnit;
    public BaseAction GetSelectedAction() => selectedAction;
}
