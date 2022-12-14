using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public event EventHandler OnDoorOpened;

    Animator animator;
    Action onInteractionComplete;

    [Header("Booleans")]
    [SerializeField] bool isOpen;
    bool isActive;

    [Header("Grid")]
    GridPosition gridPosition;

    [Header("Variables&Constants")]
    float timer;

    void Awake()
    {
        InitializationAwake();
    }

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
        if (isOpen) OpenDoor();
        else CloseDoor();
    }

    void InitializationAwake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = .5f;
        if (isOpen) CloseDoor();
        else OpenDoor();
    }

    void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);

        OnDoorOpened?.Invoke(this, EventArgs.Empty);
    }

    void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
