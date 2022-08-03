using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    BaseAction[] baseActionArray;

    [Header("Grid")]
    GridPosition gridPosition;

    [Header("Actions")]
    MoveAction moveAction;
    SpinAction spinAction;
    int actionPoints = ACTION_POINTS_MAX;
    const int ACTION_POINTS_MAX = 2;

    [SerializeField] bool isEnemy;

    public static event EventHandler OnAnyActionPointsChanged;

    void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    void FixedUpdate()
    {
        UpdateCheckIfUnitIsOnGrid();
    }

    void UpdateCheckIfUnitIsOnGrid()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (IsEnemy() && TurnSystem.Instance.IsPlayerTurn() ||
            !IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        actionPoints = ACTION_POINTS_MAX;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public MoveAction GetMoveAction() => moveAction;
    public SpinAction GetSpinAction() => spinAction;
    public GridPosition GetGridPosition() => gridPosition;
    public BaseAction[] GetBaseActionArray() => baseActionArray;
    public int GetActionPoints() => actionPoints;
    public bool IsEnemy() => isEnemy;
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) => actionPoints >= baseAction.GetActionPointsCost();

    void SpendActionPoints(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else return false;
    }

    public void Damage()
    {
        Debug.Log(transform + " damaged!");
    }

    public Vector3 GetWorldPosition() => transform.position;
}