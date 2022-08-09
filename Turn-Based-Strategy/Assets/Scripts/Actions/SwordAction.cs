using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    State state;
    enum State 
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit
    }

    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    public static event EventHandler OnAnySwordHit;

    [Header("Variables&Constants")]
    const float AFTER_HIT_STATE_TIME = .5f;
    const float BEFORE_HIT_STATE_TIME = .7f;
    float rotateSpeed = 10f;
    float stateTimer;
    int swordDistance = 1;

    [Header("Objects")]
    Unit targetUnit;

    void Update()
    {
        UpdateCases();
    }

    void UpdateCases()
    {
        if (!isActive) return;
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                var aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        if (stateTimer <= 0)
        {
            NextState();
        }
    }

    void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                stateTimer = AFTER_HIT_STATE_TIME;
                targetUnit.Damage(100);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {

        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -swordDistance; x <= swordDistance; x++)
        {
            for (int z = -swordDistance; z <= swordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.SwingingSwordBeforeHit;
        stateTimer = AFTER_HIT_STATE_TIME;
        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override string GetActionName() => "Sword";

    public int GetSwordDistance() => swordDistance;
}
