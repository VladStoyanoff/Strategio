using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    Action onInteractionComplete;

    [Header("Colors")]
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private MeshRenderer meshRenderer;

    [Header("Grid")]
    GridPosition gridPosition;

    [Header("Booleans")]
    bool isGreen;
    bool isActive;

    [Header("Variables&Constants")]
    float timer;

    void Start()
    {
        InitializationStart();
    }

    void Update()
    {
        UpdateCheckWhetherInteractionIsComplete();
    }

    void UpdateCheckWhetherInteractionIsComplete()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    void InitializationStart()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        SetColorRed();
    }

    void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }

    void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = .5f;

        if (isGreen)
        {
            SetColorRed();
        }
        else
        {
            SetColorGreen();
        }
    }

}
