using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [Header("Lists&Arrays")]
    BaseAction[] baseActionArray;

    [Header("Grid")]
    GridPosition gridPosition;

    [Header("Actions")]
    int actionPoints = ACTION_POINTS_MAX;
    const int ACTION_POINTS_MAX = 100;
    HealthSystem healthSystem;

    [Header("Booleans")]
    [SerializeField] bool isEnemy;

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
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
    }

    void InitializationStart()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    void Update()
    {
        UpdateCheckIfUnitIsOnGrid();
    }

    void UpdateCheckIfUnitIsOnGrid()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
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

    void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T) return (T)baseAction;
        } return null;
    }

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

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount); 
    }

    public GridPosition GetGridPosition() => gridPosition;
    public BaseAction[] GetBaseActionArray() => baseActionArray;
    public float GetHealthNormalized() => healthSystem.GetHealthNormalized();
    public int GetActionPoints() => actionPoints;
    public bool IsEnemy() => isEnemy;
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) => actionPoints >= baseAction.GetActionPointsCost();
    public Vector3 GetWorldPosition() => transform.position;
}