using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool isOpen;
    GridPosition gridPosition;
    Animator animator;
    Action onInteractComplete;
    bool isActive;
    float timer;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(gridPosition, this);
        if (isOpen) OpenDoor();
        else CloseDoor();
    }

    void Update()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            isActive = false;
            onInteractComplete();
        }
    }

    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
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
    }

    void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
