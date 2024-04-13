using System;
using UnityEngine;

public class Fireball : BaseProjectile
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<BaseEnemy>();
        if (enemy)
        {
            enemy.DoHit(1);
            Destroy(gameObject);
        }
    }
}
