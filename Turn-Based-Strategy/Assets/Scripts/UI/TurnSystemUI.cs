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
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });


        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    void UpdateTurnText()
    {
        turnNumberText.text = "TURN: " + TurnSystem.Instance.GetTurnNumber();
    }

    void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    void UpdateEndTurnButtonVisibility()
    {
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());

    }
}
