using System;
using UnityEngine;

public class Arrow : BaseProjectile
{
    private void OnTriggerEnter(Collider other)
    {
        var demonHealth = other.GetComponent<DemonHealth>();
        if (demonHealth)
        {
            demonHealth.TakeDamage(transform.position);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(this.gameObject);
    }
}
