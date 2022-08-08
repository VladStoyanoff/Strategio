using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [Header("Lists&Arrays")]
    List<Unit> unitList;
    List<Unit> friendlyUnitList;
    List<Unit> enemyUnitList;

    void Awake()
    {
        SetInstance();
        InitializationAwake();
    }

    void Start()
    {
        InitializationStart();
    }

    void InitializationAwake()
    {
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    void InitializationStart()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    void SetInstance()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Add(unit);
        if (unit.IsEnemy()) enemyUnitList.Add(unit);
        else friendlyUnitList.Add(unit);
    }
    
    void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Remove(unit);
        if (unit.IsEnemy()) enemyUnitList.Remove(unit);
        else friendlyUnitList.Remove(unit);
    }

    public List<Unit> GetUnitList() => unitList;
    public List<Unit> GetFriendlyUnitList() => friendlyUnitList;
    public List<Unit> GetEnemyUnitList() => enemyUnitList;
}
