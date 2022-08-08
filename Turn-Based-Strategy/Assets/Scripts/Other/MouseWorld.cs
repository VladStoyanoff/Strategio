using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [Header("Singleton")]
    static MouseWorld instance;

    [Header("Layers")]
    [SerializeField] LayerMask mousePlaneLayerMask;

    void Awake()
    {
        InitializationAwake();
    }

    void InitializationAwake()
    {
        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition()),
                        out RaycastHit raycastHit,
                        float.MaxValue,
                        instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
}
