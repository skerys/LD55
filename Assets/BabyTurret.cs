using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyTurret : MonoBehaviour
{
    [SerializeField] private Fireball fireballPrefab;
    [SerializeField] private float shotCooldown = 2f;
    [SerializeField] private float searchRadius = 5f;

    private float _cooldownTimer = 0f;

    void Update()
    {
        if (_cooldownTimer >= 0f)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer < 0f)
            {
                DoShot();
            }
        }
    }

    void DoShot()
    {
        foreach (var col in Physics.OverlapSphere(transform.position, searchRadius))
        {
            var enemy = col.gameObject.GetComponent<BaseEnemy>();
            if (enemy)
            {
                var newFireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
                newFireball.transform.forward = enemy.transform.position - transform.position;

                break;
            }
        }

        _cooldownTimer = shotCooldown;
    }
}
