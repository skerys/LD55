using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private float fadeOutSpeed = 1f;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = new Color(0f, 0f, 0f, image.color.a - fadeOutSpeed * Time.deltaTime);
    }
}
