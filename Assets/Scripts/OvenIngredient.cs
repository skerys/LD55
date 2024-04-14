using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class OvenIngredient : MonoBehaviour
{
    public ImprovementLibrary.ImprovementType improvement;
    [SerializeField] private float bobAmplitude = 0.2f;
    [SerializeField] private float bobFrequency = 2f;
    [SerializeField] private OvenGame ovenGame;

    private float _bobbingOffset;
    private float _oldBobbingOffset;
    private Vector3 _targetPosition;

    private float _frequencyOffset;
    private static readonly int Tint = Shader.PropertyToID("_Tint");

    private bool _mousedOver = false;
    private bool _bobbingOn = true;

    // Start is called before the first frame update
    void Start()
    {
        _frequencyOffset = Random.Range(0f, Mathf.PI * 2);

        if (!ovenGame) ovenGame = FindObjectOfType<OvenGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_mousedOver && Input.GetMouseButtonDown(0))
        {
            ovenGame.PickIngredient(this);
        }

        if (!_bobbingOn && Vector3.Distance(transform.position, _targetPosition) < 0.02f)
        {
            _bobbingOn = true;
        }

        if (_bobbingOn)
        {
            _bobbingOffset = Mathf.Sin(Time.time * bobFrequency + _frequencyOffset) * bobAmplitude;
            transform.position += Vector3.up * (_bobbingOffset - _oldBobbingOffset);
            _oldBobbingOffset = _bobbingOffset;
        }
        else
        {
            transform.position += (_targetPosition - transform.position) * (5f * Time.deltaTime);
        }
        
    }

    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().material.SetColor(Tint, Color.gray);
        _mousedOver = true;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().material.SetColor(Tint, Color.black);
        _mousedOver = false;
    }

    public void MoveTo(Vector3 pos)
    {
        _targetPosition = pos;
        _bobbingOn = false;
    }
}
