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

    [Header("Booleans")]
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
        InitializationStart();
    }

    void InitializationStart()
    {
        SetSelectedUnit(selectedUnit);
    }

    void Update()
    {
        UpdateSelectedAction();
    }

    void UpdateSelectedAction()
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (isBusy) return;
        if (TryHandleUnitSelection()) return;
        if (!InputManager.Instance.IsMouseButtonDownThisFrame()) return;
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
        if (!InputManager.Instance.IsMouseButtonDownThisFrame() ||
            !Physics.Raycast(Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition()), out RaycastHit raycastHit, float.MaxValue, unitsLayerMask) ||
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
        SetSelectedAction(unit.GetAction<MoveAction>());
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
