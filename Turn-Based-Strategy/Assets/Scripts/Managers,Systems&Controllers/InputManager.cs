#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance { get; private set; }
    PlayerInputActions playerInputActions;

    void Awake()
    {
        SetInstance();
        InitializationAwake();
    }

    void InitializationAwake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    void SetInstance()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one InputManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        Input.mousePosition;
#endif
    }

    public bool IsMouseButtonDownThisFrame() 
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.Click.WasPressedThisFrame();
#else
        Input.GetMouseButtonDown(0);
#endif
    }

    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        var inputMoveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) inputMoveDir.y += 1;
        if (Input.GetKey(KeyCode.S)) inputMoveDir.y -= 1;
        if (Input.GetKey(KeyCode.A)) inputMoveDir.x -= 1;
        if (Input.GetKey(KeyCode.D)) inputMoveDir.x += 1;
        return inputMoveDir;
#endif
    }

    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        float rotateAmount = 0f;

        if (Input.GetKey(KeyCode.Q)) rotateAmount = +1f;
        if (Input.GetKey(KeyCode.E)) rotateAmount = -1f;

        return rotateAmount;
#endif
    }

    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        float zoomAmount = 0f;
        if (Input.mouseScrollDelta.y > 0) zoomAmount = -1f;
        if (Input.mouseScrollDelta.y < 0) zoomAmount = +1f;
        return zoomAmount;
#endif
    }
}

