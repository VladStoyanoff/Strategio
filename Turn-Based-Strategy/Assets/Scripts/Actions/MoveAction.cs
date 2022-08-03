using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [Header("Setup")]
    [SerializeField] Animator unitAnimator;

    [Header("Movement")]
    Vector3 targetPosition;
    [SerializeField] int moveDistance = 4;

    [Header("Constants&Variables")]
    float moveSpeed = 4f;
    float rotateSpeed = 10f;
    const float STOPPING_DISTANCE = .1f;

    protected override void Awake()
    {
        base.Awake();
        SetStartPosition();
    }

    void Update()
    {
        UpdateMove();
    }

    void SetStartPosition()
    {
        targetPosition = transform.position;
    }

    void UpdateMove()
    {
        if (!isActive) return;
        var moveDirection = (targetPosition - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        if (Vector3.Distance(targetPosition, transform.position) < STOPPING_DISTANCE)
        {
            unitAnimator.SetBool("isWalking", false);
            ActionComplete();
            return;
        }

        unitAnimator.SetBool("isWalking", true);
        transform.position += moveDirection * Time.deltaTime * moveSpeed;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -moveDistance; x <= moveDistance; x++)
        {
            for (int z = -moveDistance; z <= moveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (unitGridPosition == testGridPosition) continue;
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }

    public override string GetActionName() => "Move";
}
