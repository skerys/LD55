using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorImageDestruct : MonoBehaviour
{
    public float timeToDestruct = 1f;

    private float _timer = 0f;
    private SpriteRenderer _sprite;
    private Color startColor;
    private Color endColor;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        startColor = _sprite.color;
        endColor = startColor;
        endColor.a = 0f;
    }
    
    private void Update()
    {
        _sprite.color = Color.Lerp(startColor, endColor, _timer / timeToDestruct);
        
        _timer += Time.deltaTime;
        if (_timer > timeToDestruct)
        {
            Destroy(gameObject);
        }
    }
}
