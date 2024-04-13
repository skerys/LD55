using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHealth : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private float invulnerableTime = 0.3f;

    private DemonController _demon;
    private float _invulnerabiltyTimer = 0.1f;

    void Start()
    {
        _demon = GetComponent<DemonController>();

    }

    void Update()
    {
        if (_invulnerabiltyTimer > 0f)
            _invulnerabiltyTimer -= Time.deltaTime;
    }
    
    public void TakeDamage(Vector3 damagerPos)
    {
        if (_invulnerabiltyTimer > 0f)
            return;
        
        health--;
        _demon.Bounce(transform.position - damagerPos);
        _invulnerabiltyTimer = invulnerableTime;

    }
}
