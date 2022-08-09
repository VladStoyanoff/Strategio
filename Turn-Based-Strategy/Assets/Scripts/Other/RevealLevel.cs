using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealLevel : MonoBehaviour
{
    [SerializeField] List<GameObject> hider1List;
    [SerializeField] List<GameObject> hider2List;
    [SerializeField] List<GameObject> enemy1List;
    [SerializeField] List<GameObject> enemy2List;
    [SerializeField] Door door1;
    [SerializeField] Door door2;

    void Start()
    {
        InitializeStart();
    }

    void InitializeStart()
    {
        door1.OnDoorOpened += (object sender, EventArgs e) =>
        {
            SetActiveGameObjectList(hider1List, false);
            SetActiveGameObjectList(enemy1List, true);
        };
        door2.OnDoorOpened += (object sender, EventArgs e) =>
        {
            SetActiveGameObjectList(hider2List, false);
            SetActiveGameObjectList(enemy2List, true);
        };
    }


    void SetActiveGameObjectList(List<GameObject> gameObjectList, bool isActive)
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(isActive);
        }
    }

}
