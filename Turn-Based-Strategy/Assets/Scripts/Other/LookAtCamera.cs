using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cameraTransform;

    void Awake()
    {
        InitializationAwake();
    }

    void LateUpdate()
    {
        UpdateDirection();
    }

    void UpdateDirection()
    {
        var dirToCamera = (cameraTransform.position - transform.position).normalized;
        transform.LookAt(transform.position + dirToCamera * -1);
    }

    void InitializationAwake()
    {
        cameraTransform = Camera.main.transform;
    }
}
