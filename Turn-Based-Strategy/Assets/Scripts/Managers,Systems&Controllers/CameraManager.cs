using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject actionCameraGameObject;


    void Start()
    {
        InitializationStart();
    }

    void InitializationStart()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }

    void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                var cameraCharacterHeight = Vector3.up * 1.7f;
                var shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                float shoulderOffsetAmount = .5f;
                var shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;
                var  actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);
                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                ShowActionCamera();
                break;
        }
    }

    void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }
}
