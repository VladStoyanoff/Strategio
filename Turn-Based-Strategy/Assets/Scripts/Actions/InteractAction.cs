using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    int interactDistance = 1;

    void Update()
    {
        if (!isActive) return;
    }

    public override string GetActionName() => "Interact";

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -interactDistance; x <= interactDistance; x++)
        {
            for (int z = -interactDistance; z <= interactDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);
                if (door == null) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPositon, Action onActionComplete)
    {
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPositon);
        door.Interact(OnInteractComplete());
        ActionStart(onActionComplete);
    }

    void OnInteractComplete()
    {
        ActionComplete();
    }
}
