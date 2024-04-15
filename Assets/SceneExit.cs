using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneExit : MonoBehaviour
{
    [SerializeField] private GameObject arrowIndicator;

    [SerializeField] private GameObject spaceIndicator;

    public event Action OnPlayerExit = delegate {};
    
    private bool _demonInside = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _demonInside)
        {
            SoundManager.Instance.PlaySound(OneShotSoundTypes.Pop);
            OnPlayerExit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DemonController>())
        {
            _demonInside = true;
            spaceIndicator.SetActive(true);
            arrowIndicator.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DemonController>())
        {
            _demonInside = false;
            spaceIndicator.SetActive(false);
            arrowIndicator.SetActive(true);
        }
    }
}
