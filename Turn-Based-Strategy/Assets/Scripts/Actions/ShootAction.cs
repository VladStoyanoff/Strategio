using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    enum State { Aiming, Shooting, Cooloff, }

    State state;
    int shootDistance = 7;
    float stateTimer;
    Unit targetUnit;
    bool canShootBullet;
    float rotateSpeed = 10f;
    const float SHOOTING_STATE_TIME = .1f;
    const float COOLOFF_STATE_TIME = .5f;
    const float AIMING_STATE_TIME = 1f;

    void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                var aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;

        }

        if (stateTimer <= 0)
        {
            NextState();
        }
    }

    void Shoot()
    {
        targetUnit.Damage();
    }

    void NextState()
    {
        switch (state) 
        {
            case State.Aiming:
                state = State.Shooting;
                stateTimer = SHOOTING_STATE_TIME;
                break;
            case State.Shooting:
                state = State.Cooloff;
                stateTimer = COOLOFF_STATE_TIME;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }



    public override string GetActionName() => "Shoot";

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -shootDistance; x <= shootDistance; x++)
        {
            for (int z = -shootDistance; z <= shootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > shootDistance) continue;
                if (unitGridPosition == testGridPosition) continue;
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit  targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        state = State.Aiming;
        stateTimer = AIMING_STATE_TIME;

        canShootBullet = true;
    }
}
