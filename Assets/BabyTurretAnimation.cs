using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyTurretAnimation : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float rotationSpeed;
    
    private Quaternion _initialRotation;

    private float _bobbingOffset;
    private float _oldBobbingOffset;
    
    void Awake()
    {
        _initialRotation = spriteTransform.rotation;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        spriteTransform.rotation = _initialRotation;

        _bobbingOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        spriteTransform.position += Vector3.up * (_bobbingOffset - _oldBobbingOffset);
        _oldBobbingOffset = _bobbingOffset;
    }
}
