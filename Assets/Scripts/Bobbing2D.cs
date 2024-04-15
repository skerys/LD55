using System;
using UnityEngine;

public class Bobbing2D : MonoBehaviour
{
    [SerializeField] private float bobAmplitude = 0.2f;
    [SerializeField] private float bobFrequency = 2f;

    private float _bobbingOffset;
    private float _oldBobbingOffset;
    
    public void Update()
    {
        _bobbingOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position += Vector3.up * (_bobbingOffset - _oldBobbingOffset);
        _oldBobbingOffset = _bobbingOffset;
    }
}
