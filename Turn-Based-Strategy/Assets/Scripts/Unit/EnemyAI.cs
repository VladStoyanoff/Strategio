using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }
    State state;

    [Header("Variables&Constants")]
    float timer;

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
        state = State.WaitingForEnemyTurn;
    }

    void InitializationStart()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    void Update()
    {
        UpdateCases();
    }

    void UpdateCases()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;
        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (TryTakeEnemyAIACtion(SetStateTakingTurn)) state = State.Busy;
                    else TurnSystem.Instance.NextTurn();
                }
                break;
            case State.Busy:
                break;
        }
    }

    void SetStateTakingTurn()
    {
        timer = .5f;
        state = State.TakingTurn;
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    bool TryTakeEnemyAIACtion(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIACtion(enemyUnit, onEnemyAIActionComplete)) return true;
        }
        return false;
    }

    bool TryTakeEnemyAIACtion(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction)) continue;
            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else return false;
    }
}
