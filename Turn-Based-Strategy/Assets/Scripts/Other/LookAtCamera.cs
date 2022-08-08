using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cameraTransform;

    void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        var dirToCamera = (cameraTransform.position - transform.position).normalized;
        transform.LookAt(transform.position + dirToCamera * -1);
    }
}
