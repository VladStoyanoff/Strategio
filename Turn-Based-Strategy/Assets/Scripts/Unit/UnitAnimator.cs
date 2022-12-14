using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform bulletProjectilePrefab;
    [SerializeField] Transform shootPointTransform;
    [SerializeField] Transform rifleTransform;
    [SerializeField] Transform swordTransform;

    void Awake()
    {
        InitializationAwake();
    }

    void Start()
    {
        InitializationStart();
    }

    void InitializationAwake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    void InitializationStart()
    {
        EquipRifle();
    }

    void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }

    void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();
        animator.SetTrigger("SwordSlash");
    }

    void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");
        Transform bulleProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulleProjectileTransform.GetComponent<BulletProjectile>();

        var targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = shootPointTransform.position.y;
        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", true);
    }

    void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", false);
    }

    void EquipSword()
    {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }

    void EquipRifle()
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }
}
