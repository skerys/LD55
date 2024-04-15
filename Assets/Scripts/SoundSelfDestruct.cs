using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundSelfDestruct : MonoBehaviour
{
    private static readonly float pitchRange = 0.3f;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.pitch += Random.Range(-pitchRange, pitchRange);
        
        Destroy(this.gameObject, _audioSource.clip.length);
    }
}
