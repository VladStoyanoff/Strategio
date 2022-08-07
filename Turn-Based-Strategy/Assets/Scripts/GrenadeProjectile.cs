using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;
    [SerializeField] Transform grenadeExplodeVFXPrefab;
    Vector3 targetPosition;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] AnimationCurve arcYAnimationCurve;
    float totalDistance;
    Vector3 positionXZ;
    Action onGrenadeBehaviourComplete;
    int moveSpeed = 15;
    float reachedTargetDistance = .2f;
    float damageRadius = 4f;

    void Update()
    {
        var moveDir = (targetPosition - positionXZ).normalized;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);


        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {

            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit)) targetUnit.Damage(30);
            }
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplodeVFXPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
            onGrenadeBehaviourComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
