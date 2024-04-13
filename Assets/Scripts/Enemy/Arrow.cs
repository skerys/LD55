using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Transform arrowTransform;
    [SerializeField] private float speed;

    private Rigidbody _body;
    
    public void Start()
    {
        _body = GetComponent<Rigidbody>();
        _body.velocity = transform.forward * speed;
    }

    public void Update()
    {
        Vector3 a = Vector3.ProjectOnPlane(Vector3.forward, Camera.main.transform.forward);
        Vector3 b = Vector3.ProjectOnPlane(transform.forward, Camera.main.transform.forward);
        
        float zAngle = Vector3.SignedAngle(a, b, -Camera.main.transform.forward);
        
        arrowTransform.rotation = Quaternion.Euler(30f, 0f, -zAngle);
    }

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
