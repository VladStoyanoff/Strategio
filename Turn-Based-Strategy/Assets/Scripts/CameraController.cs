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
    int zoomAmount = 1;
    int zoomSpeed = 5;
    const int MIN_FOLLOW_Y_OFFSET = 2;
    const int MAX_FOLLOW_Y_OFFSET = 12;

    void Awake()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    void Start()
    {
        followOffset = cinemachineTransposer.m_FollowOffset;
    }

    void Update()
    {
        UpdateCameraMovement();
        UpdateCameraRotation();
        UpdateZoomCamera();
    }

    void UpdateCameraMovement()
    {
        inputMoveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) inputMoveDir.z += 1;
        if (Input.GetKey(KeyCode.S)) inputMoveDir.z -= 1;
        if (Input.GetKey(KeyCode.A)) inputMoveDir.x -= 1;
        if (Input.GetKey(KeyCode.D)) inputMoveDir.x += 1;

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    void UpdateCameraRotation()
    {
        rotationVector = Vector3.zero;

        if (Input.GetKey(KeyCode.Q)) rotationVector.y += 1;
        if (Input.GetKey(KeyCode.E)) rotationVector.y -= 1;

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    void UpdateZoomCamera()
    {
        if (Input.mouseScrollDelta.y > 0) followOffset.y -= zoomAmount;
        if (Input.mouseScrollDelta.y < 0) followOffset.y += zoomAmount;

        followOffset.y = Mathf.Clamp(followOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
    }
}
