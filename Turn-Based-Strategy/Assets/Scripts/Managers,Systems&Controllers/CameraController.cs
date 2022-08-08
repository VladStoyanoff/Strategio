using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineTransposer cinemachineTransposer;

    [Header("Movement")]
    Vector3 inputMoveDir;
    int moveSpeed = 10;

    [Header("Rotation")]
    Vector3 rotationVector;
    int rotationSpeed = 100;

    [Header("Zoom")]
    Vector3 followOffset;
    int zoomSpeed = 5;
    const int MIN_FOLLOW_Y_OFFSET = 2;
    const int MAX_FOLLOW_Y_OFFSET = 12;

    void Awake()
    {
        InitializationAwake();
    }

    void Start()
    {
        InitializationStart();
    }

    void InitializationStart()
    {
        followOffset = cinemachineTransposer.m_FollowOffset;
    }

    void InitializationAwake()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    void Update()
    {
        UpdateCameraMovement();
        UpdateCameraRotation();
        UpdateZoomCamera();
    }

    void UpdateCameraMovement()
    {
        var inputMoveDir = InputManager.Instance.GetCameraMoveVector();

        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    void UpdateCameraRotation()
    {
        rotationVector = Vector3.zero;

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    void UpdateZoomCamera()
    {
        followOffset.y += InputManager.Instance.GetCameraZoomAmount();

        followOffset.y = Mathf.Clamp(followOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
    }
}
