using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinScale : MonoBehaviour
{
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float baseIncrease;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * baseIncrease + Vector3.one * (Mathf.Sin(Time.time * frequency) * amplitude);
    }
}
