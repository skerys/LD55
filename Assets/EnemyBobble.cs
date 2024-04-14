using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBobble : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private float frequency;
    [SerializeField] private float amplitude;

    [SerializeField] private float rotationFrequency;
    [SerializeField] private float rotationAmplitude;

    private Rigidbody _body;

    private float _bobbingFrequencyOffset;
    private Vector3 _bobbingOffset;
    private Vector3 _oldBobbingOffset;

    private float _rotationOffset;
    private float _rotationOldOffset;

    private Quaternion _initialRot;

    private bool _hasReset = false;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _bobbingFrequencyOffset = Random.Range(0, 2 * Mathf.PI);
        _oldBobbingOffset = Vector3.zero;
        _rotationOldOffset = 0f;

        _initialRot = spriteTransform.rotation;
    }
    
    void Update()
    {
        if (_body.velocity.sqrMagnitude > 0.01f)
        {
            _hasReset = false;

            _bobbingOffset = Vector3.up * (Mathf.Abs(Mathf.Sin(Time.time * frequency + _bobbingFrequencyOffset)) * amplitude);
            spriteTransform.position += _bobbingOffset - _oldBobbingOffset;
            _oldBobbingOffset = _bobbingOffset;

            _rotationOffset = Mathf.Sin(Time.time * rotationFrequency + _bobbingFrequencyOffset) * rotationAmplitude;
            spriteTransform.rotation = Quaternion.Euler(spriteTransform.rotation.eulerAngles.x, spriteTransform.rotation.eulerAngles.y, spriteTransform.rotation.eulerAngles.z + _rotationOffset - _rotationOldOffset);
            _rotationOldOffset = _rotationOffset;
        }
        else
        {
            if (_hasReset) return;
            
            spriteTransform.position -= _oldBobbingOffset;
            spriteTransform.rotation = _initialRot;
            _hasReset = true;
            _oldBobbingOffset = Vector3.zero;
        }
    }
}
