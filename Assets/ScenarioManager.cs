using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static int EnemyCount = 0;
    
    [SerializeField] private Gradient circleColor;
    [SerializeField] private SpriteRenderer circle;
    [SerializeField] private GameObject spawnAnimation;
    [SerializeField] private GameObject despawnAnimation;
    [SerializeField] private GameObject deathAnimation;
    [SerializeField] private GameObject ashEffect;
    [SerializeField] private DemonController demonGuy;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private SceneExit sceneExit;
    [SerializeField] private HealthUI healthUI;

    public List<Animator> warlocks = new List<Animator>();

    [SerializeField] private float chargeUpTime;
    [SerializeField] private float animationTime;

    private float _chargeUpTimer = 0f;
    private bool startCharging = false;

    private bool startUncharging = false;

    private bool enemySpawningDone = false;

    private GameObject spawnEffectGO = null;
    private GameObject deathEffectGO = null;
    private DemonHealth _demonHealth;

    private void Awake()
    {
        _demonHealth = demonGuy.GetComponent<DemonHealth>();
    }

    private void OnEnable()
    {
        enemySpawner.OnEmptyReserves += EnemySpawningDone;
        sceneExit.OnPlayerExit += StartEndSequence;
        _demonHealth.OnKill += StartDeathSequence;
    }

    private void OnDisable()
    {
        enemySpawner.OnEmptyReserves -= EnemySpawningDone;
        sceneExit.OnPlayerExit -= StartEndSequence;
        _demonHealth.OnKill -= StartDeathSequence;
    }

    [ContextMenu("StartSpawnSequence")]
    void StartSpawnSequence()
    {
        startCharging = true;
        
        StartCoroutine(SpawnEffect(chargeUpTime));
        StartCoroutine(EnableDemon(chargeUpTime + animationTime));
    }

    void SetupEndSequence()
    {
        enemySpawner.enabled = false;
        sceneExit.gameObject.SetActive(true);
    }

    void StartEndSequence()
    {
        sceneExit.gameObject.SetActive(false);
        
        demonGuy.gameObject.SetActive(false);
        Instantiate(despawnAnimation, demonGuy.transform.position + Vector3.up * 0.4f, despawnAnimation.transform.rotation);

        startUncharging = true;
    }

    void StartDeathSequence()
    {
        demonGuy.gameObject.SetActive(false);
        deathEffectGO = Instantiate(deathAnimation, demonGuy.transform.position + Vector3.up * 0.4f, deathAnimation.transform.rotation);
        demonGuy.transform.position += 100f * Vector3.forward;

        StartCoroutine(SpawnAshParticles(1f));
    }

    private void Update()
    {
        if (startCharging)
        {
            _chargeUpTimer += Time.deltaTime;
            circle.color = circleColor.Evaluate(_chargeUpTimer / chargeUpTime);

            if (_chargeUpTimer > chargeUpTime)
            {
                startCharging = false;
                _chargeUpTimer = 0f;
            }
        }

        if (startUncharging)
        {
            _chargeUpTimer += Time.deltaTime;
            circle.color = circleColor.Evaluate(1 - (_chargeUpTimer / chargeUpTime));

            if (_chargeUpTimer > chargeUpTime)
            {
                startUncharging = false;
                _chargeUpTimer = 0f;

                foreach (var warlock in warlocks)
                {
                    warlock.Play("WarlockIdle");
                }
            }
            
        }

        if (enemySpawningDone && EnemyCount <= 0)
        {
            SetupEndSequence();
            enemySpawningDone = false;
        }
        
    }

    IEnumerator SpawnEffect(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        spawnEffectGO = Instantiate(spawnAnimation, circle.transform.position, Quaternion.identity);
    }

    IEnumerator EnableDemon(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(spawnEffectGO);
        demonGuy.gameObject.SetActive(true);
        demonGuy.GetComponent<DemonImprovements>().SetupImprovements();
        
        healthUI.gameObject.SetActive(true);

        enemySpawner.enabled = true;
        EnemyCount = 0;
    }

    IEnumerator SpawnAshParticles(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Instantiate(ashEffect, deathEffectGO.transform.position, ashEffect.transform.rotation);
        Destroy(deathEffectGO);
        
        
    }

    private void EnemySpawningDone()
    {
        enemySpawningDone = true;
    }

    public static void AddEnemyCount(int amount)
    {
        EnemyCount++;
    }

    public static void RemoveEnemyCount(int amount)
    {
        EnemyCount--;
    }
}
