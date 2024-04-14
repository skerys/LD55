using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private float speed;

    private void Update()
    {
        if (Target)
        {
            Vector3 dirToTarget = (Target.position - transform.position).normalized;
            Body.velocity = dirToTarget * speed;

            Sprite.flipX = Body.velocity.x < 0f;
        }
    }
}
