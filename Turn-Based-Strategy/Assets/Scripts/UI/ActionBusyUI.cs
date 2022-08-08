using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{

    void Start()
    {
        InitializationStart();
    }

    void InitializationStart()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        Hide();
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        if (isBusy) Show();
        if (!isBusy) Hide();
    }
}
