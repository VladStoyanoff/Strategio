using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    Vector3 targetPosition;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] Transform bulletHitVFXPrefab;
    int moveSpeed = 200;
    float distanceBeforeMoving;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    void Update()
    {
        var distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
        var moveDir = (targetPosition - transform.position).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        var distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);
        if (distanceBeforeMoving > distanceAfterMoving) return;
        transform.position = targetPosition;
        trailRenderer.transform.parent = null;
        Destroy(gameObject);
        Instantiate(bulletHitVFXPrefab, targetPosition, Quaternion.identity);
    }
}
