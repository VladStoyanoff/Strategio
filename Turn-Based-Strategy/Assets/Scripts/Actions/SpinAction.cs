using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    [Header("Variables&Constants")]
    float totalSpinAmount;

    void Update()
    {
        UpdateStopCondition();
        UpdateSpin();
    }

    void UpdateSpin()
    {
        float spinAddAmount = 360 * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360)
        {
            ActionComplete();
        }
    }

    void UpdateStopCondition()
    {
        if (!isActive) return;
    }

    public override void TakeAction(GridPosition gridPositon, Action onActionComplete)
    {
        totalSpinAmount = 0;

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetActionPointsCost() => 2;

    public override string GetActionName() => "Spin";

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction 
        { 
            gridPosition = gridPosition,
            actionValue = 0
        };
    }
}
