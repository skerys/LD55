using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private float speed;
    [SerializeField] private float avoidanceRadius;
    [SerializeField] private float avoidanceStrength;
    [SerializeField] private LayerMask enemyLayermask = default;

    private void Update()
    {
        if (Target)
        {
            Vector3 dirToTarget = (Target.position - transform.position).normalized;

            Vector3 avoidanceSum = Vector3.zero;
            foreach (var enemy in Physics.OverlapSphere(transform.position, avoidanceRadius, enemyLayermask))
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                avoidanceSum += (transform.position - enemy.transform.position) * ((avoidanceRadius - distance) / avoidanceRadius);
            }

            avoidanceSum = Vector3.ClampMagnitude(avoidanceSum, 1f);

            dirToTarget += avoidanceSum * avoidanceStrength;
            dirToTarget = dirToTarget.normalized;
            
            Body.velocity = dirToTarget * speed;

            Sprite.flipX = Body.velocity.x < 0f;
        }
    }
}
