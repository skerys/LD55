using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcherDebug : MonoBehaviour
{
    private bool _inDefault = true;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_inDefault)
            {
                _animator.Play("Oven");
            }
            else
            {
                _animator.Play("Default");
            }

            _inDefault = !_inDefault;
        }
    }
}
