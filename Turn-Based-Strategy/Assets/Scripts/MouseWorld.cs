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
        instance = this;   
    }

    public static Vector3 GetPosition()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
                        out RaycastHit raycastHit,
                        float.MaxValue,
                        instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
}
