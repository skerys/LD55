using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public enum OneShotSoundTypes
{
    DashAttack,
    EnemySpawn,
    EnemyDeath,
    BabyCry,
    BabyHappy,
    PepperPlop,
    StirPot,
    Slurp,
    Pop,
    Woosh
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public GameObject dashAttack;
    public List<GameObject> enemySpawn;
    public List<GameObject> enemyDeath;
    public GameObject babyCry;
    public GameObject babyHappy;
    public GameObject pepperPlop;
    public GameObject stirPot;
    public GameObject slurp;
    public GameObject pop;
    public GameObject woosh;

    public AudioSource runHouse;
    public AudioSource runCombat;

    private float _currentVelocity;
    private bool _inHouse;
    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
            DontDestroyOnLoad(this.gameObject);
        } 
    }

    void Update()
    {
        runCombat.volume = !_inHouse && _currentVelocity > 0.1f ? 0.1f : 0f;
        runHouse.volume = _inHouse && _currentVelocity > 0.1f ? 0.02f : 0f;
    }

    public void UpdateVelocity(float value)
    {
        _currentVelocity = value;
    }

    public void UpdateScene(bool inHouse)
    {
        _currentVelocity = 0f;
        _inHouse = inHouse;
    }

    public void PlaySound(OneShotSoundTypes type)
    {
        switch (type)
        {
            case OneShotSoundTypes.DashAttack:
                Instantiate(dashAttack);
                break;
            case OneShotSoundTypes.EnemySpawn:
                Instantiate(enemySpawn[Random.Range(0, enemySpawn.Count)]);
                break;
            case OneShotSoundTypes.EnemyDeath:
                Instantiate(enemyDeath[Random.Range(0, enemyDeath.Count)]);
                break;
            case OneShotSoundTypes.BabyCry:
                Instantiate(babyCry);
                break;
            case OneShotSoundTypes.BabyHappy:
                Instantiate(babyHappy);
                break;
            case OneShotSoundTypes.PepperPlop:
                Instantiate(pepperPlop);
                break;
            case OneShotSoundTypes.StirPot:
                Instantiate(stirPot);
                break;
            case OneShotSoundTypes.Slurp:
                Instantiate(slurp);
                break;
            case OneShotSoundTypes.Pop:
                Instantiate(pop);
                break;
            case OneShotSoundTypes.Woosh:
                Instantiate(woosh);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
