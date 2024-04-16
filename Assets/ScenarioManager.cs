using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private SpriteRenderer blackSquare;

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject thanksBubbles;

    public List<Animator> warlocks = new List<Animator>();

    [SerializeField] private float chargeUpTime;
    [SerializeField] private float animationTime;
    [SerializeField] private float uiMoveSpeed;

    private float _chargeUpTimer = 0f;
    private bool needToChangeCircleCol;
    private bool startCharging = false;

    private bool startUncharging = false;

    private bool enemySpawningDone = false;

    private GameObject spawnEffectGO = null;
    private GameObject deathEffectGO = null;
    private DemonHealth _demonHealth;

    private float blackSquareTargetAlpha = 0f;

    private void Awake()
    {
        _demonHealth = demonGuy.GetComponent<DemonHealth>();
    }

    private void Start()
    {
        if (!GameStateManager.instance.NeedToShowStartMenu)
        {
            needToChangeCircleCol = false;
            startButton.SetActive(false);
            title.SetActive(false);
            StartSpawnSequence();
            SetBlackSquareAlpha(1f);
            circle.color = circleColor.Evaluate(1f);
            circle.sortingOrder = 2;
        }
        else
        {
            circle.color = circleColor.Evaluate(0f);
            needToChangeCircleCol = true;
            SetBlackSquareAlpha(0f);
        }

        SoundManager.Instance.UpdateScene(false);
        blackSquareTargetAlpha = 0f;
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
    public void StartSpawnSequence()
    {
        if (GameStateManager.instance.NeedToShowStartMenu)
        {
            SoundManager.Instance.PlaySound(OneShotSoundTypes.Pop);
            GameStateManager.instance.NeedToShowStartMenu = false;
        }
        
        startCharging = true;
        
        StartCoroutine(SpawnEffect(chargeUpTime));
        StartCoroutine(EnableDemon(chargeUpTime + animationTime));
    }

    public void RestartScene()
    {
        SoundManager.Instance.PlaySound(OneShotSoundTypes.Pop);
        SceneManager.LoadScene("CombatScene");
    }

    void SetupEndSequence()
    {
        enemySpawner.enabled = false;
        sceneExit.gameObject.SetActive(true);

        if (GameStateManager.instance.currentStage == GameStateManager.instance.infoByStage.Count - 1)
        {
            thanksBubbles.SetActive(true);
        }
    }

    void StartEndSequence()
    {
        sceneExit.gameObject.SetActive(false);
        
        demonGuy.gameObject.SetActive(false);
        Instantiate(despawnAnimation, demonGuy.transform.position + Vector3.up * 0.4f, despawnAnimation.transform.rotation);
        SoundManager.Instance.PlaySound(OneShotSoundTypes.Woosh);

        startUncharging = true;
    }

    void StartDeathSequence()
    {
        demonGuy.gameObject.SetActive(false);
        deathEffectGO = Instantiate(deathAnimation, demonGuy.transform.position + Vector3.up * 0.4f, deathAnimation.transform.rotation);
        SoundManager.Instance.PlaySound(OneShotSoundTypes.Woosh);
        demonGuy.transform.position += 100f * Vector3.forward;

        StartCoroutine(SpawnAshParticles(1f));
    }

    IEnumerator LoadScene(string sceneName)
    {
        healthUI.gameObject.SetActive(false);
        blackSquareTargetAlpha = 1f;
        circle.sortingOrder = 2;
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("HouseScene");
    }

    void SetBlackSquareAlpha(float value)
    {
        var current = blackSquare.color;
        blackSquare.color = new Color(current.r, current.g, current.b, value);
    }

    private void Update()
    {
        SetBlackSquareAlpha(blackSquare.color.a + Mathf.Sign(blackSquareTargetAlpha - blackSquare.color.a) * 1f * Time.deltaTime);
        
        if (startCharging)
        {
            _chargeUpTimer += Time.deltaTime;
            if(needToChangeCircleCol) circle.color = circleColor.Evaluate(_chargeUpTimer / chargeUpTime);

            startButton.transform.position += Vector3.down * (uiMoveSpeed * Time.deltaTime);
            title.transform.position += Vector3.up * (uiMoveSpeed * Time.deltaTime);

            if (_chargeUpTimer > chargeUpTime)
            {
                startCharging = false;
                _chargeUpTimer = 0f;
            }
        }

        if (startUncharging)
        {
            _chargeUpTimer += Time.deltaTime;
            //circle.color = circleColor.Evaluate(1 - (_chargeUpTimer / chargeUpTime));

            if (_chargeUpTimer > 0.9f * chargeUpTime)
            {
                foreach (var warlock in warlocks)
                {
                    warlock.Play("WarlockIdle");
                }
            }
            if (_chargeUpTimer > chargeUpTime)
            {
                startUncharging = false;
                _chargeUpTimer = 0f;
                StartCoroutine(LoadScene("HouseScene"));
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
        circle.sortingOrder = -1;
        spawnEffectGO = Instantiate(spawnAnimation, circle.transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(OneShotSoundTypes.Woosh);
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
        restartButton.SetActive(true);
        
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
