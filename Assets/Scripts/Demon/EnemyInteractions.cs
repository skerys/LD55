using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractions : MonoBehaviour
{
    private DashController _dash;
    private DemonHealth _health;

    void Start()
    {
        _dash = GetComponent<DashController>();
        _health = GetComponent<DemonHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<BaseEnemy>();

        if (enemy)
        {
            if (_dash.DashInProgress)
            {
                //Currently dashing, kill enemy/do damage
                if (enemy.DoHit(1))
                {
                    _dash.killsThisDash++;
                };
                
            }
            else
            {
                //Not dashing, take damage
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        var enemy = other.gameObject.GetComponent<BaseEnemy>();

        if (enemy)
        {
           _health.TakeDamage(other.gameObject.transform.position); 
        }
    }
}
