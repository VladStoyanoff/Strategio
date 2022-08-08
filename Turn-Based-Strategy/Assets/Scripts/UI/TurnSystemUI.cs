using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button endTurnBtn;
    [SerializeField] TextMeshProUGUI turnNumberText;
    [SerializeField] GameObject enemyTurnVisualGameObject;

    void Start()
    {
        InitializationStart();
    }

    void InitializationStart()
    {
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });


        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        TurnText();
        EnemyTurnVisual();
        EndTurnButtonVisibility();
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        TurnText();
        EnemyTurnVisual();
        EndTurnButtonVisibility();
    }

    void TurnText()
    {
        turnNumberText.text = "TURN: " + TurnSystem.Instance.GetTurnNumber();
    }

    void EnemyTurnVisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    void EndTurnButtonVisibility()
    {
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());

    }
}
