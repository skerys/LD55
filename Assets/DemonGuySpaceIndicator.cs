using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DemonGuySpaceIndicator : MonoBehaviour
{
    [SerializeField] private GameObject spaceIndicator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceIndicator.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpaceableOven")
        {
            if(FindObjectOfType<OvenGame>().isStartable)
                spaceIndicator.SetActive(true);
        }
        
        if (other.gameObject.tag == "SpaceableCrib")
        {
            if(FindObjectOfType<BabyGame>().isStartable)
                spaceIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SpaceableOven" || other.gameObject.tag == "SpaceableCrib")
        {
            spaceIndicator.SetActive(false);
        }
    }
}
