using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseOnSpawn : MonoBehaviour
{
    [SerializeField] private float impulseForce;
    
    void Awake()
    {
        GetComponent<Rigidbody>()?.AddForce(impulseForce * Random.onUnitSphere, ForceMode.Impulse);
    }

}
