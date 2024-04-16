using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonGuySpaceIndicator : MonoBehaviour
{
    [SerializeField] private GameObject spaceIndicator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spaceable")
        {
            spaceIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Spaceable")
        {
            spaceIndicator.SetActive(false);
        }
    }
}
