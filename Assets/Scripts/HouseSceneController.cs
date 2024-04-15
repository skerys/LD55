using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseSceneController : MonoBehaviour
{
    [SerializeField] private ImprovementLibrary improvementLibrary;
    [SerializeField] private Animator cinemachineAnimator;
    [SerializeField] private Transform ovenPoint;
    [SerializeField] private Transform cribPoint;
    [SerializeField] private DemonController player;
    [SerializeField] private float interactionRange;

    [SerializeField] private Transform ovenStandPosition;
    [SerializeField] private Transform cribStandPosition;

    [SerializeField] private Transform spawnAnimationPrefab;
    [SerializeField] private DemonAnimation demonGuy;
    [SerializeField] private Transform despawnAnimationPrefab;
    [SerializeField] private SpriteRenderer spawnCircle;

    [SerializeField] private float spawnAnimTime = 0.7f;

    [SerializeField] private OvenGame ovenGame;
    [SerializeField] private BabyGame babyGame;
    [SerializeField] private SpriteRenderer blackSquare;
    [SerializeField] private SceneExit sceneExit;

    private Vector3 currentTargetPosition;
    private float blackSquareTargetAlpha;
    private float circleTargetAlpha;

    private bool canExit = true;

    public enum HOUSE_STATE
    {
        Default,
        OvenGame,
        CribGame
    };

    private HOUSE_STATE _currentState;

    private void Awake()
    {
        _currentState = HOUSE_STATE.Default;

        if (!ovenGame) ovenGame = FindObjectOfType<OvenGame>();
        if (!babyGame) babyGame = FindObjectOfType<BabyGame>();
        if (!sceneExit) sceneExit = FindObjectOfType<SceneExit>();
        SetBlackSquareAlpha(1f);
    }

    void Start()
    {
        //improvementLibrary.DisableImprovement(ImprovementLibrary.ImprovementType.BabyPowerup);
        
        circleTargetAlpha = 1f;
        spawnCircle.sortingOrder = 2;
        blackSquareTargetAlpha = 1f;
        StartCoroutine(WaitForStartAnim(spawnAnimTime));
    }

    IEnumerator WaitForStartAnim(float t)
    {
        yield return new WaitForSeconds(1f);
        blackSquareTargetAlpha = 0f;
        yield return new WaitForSeconds(1.2f);
        spawnCircle.sortingOrder = -1;
        var spawnAnimation = Instantiate(spawnAnimationPrefab, demonGuy.transform.position,
            spawnAnimationPrefab.transform.rotation);
        yield return new WaitForSeconds(t);
        spawnAnimation.gameObject.SetActive(false);
        demonGuy.gameObject.SetActive(true);
        circleTargetAlpha = 0f;
    }

    void PrepareExit()
    {
        sceneExit.gameObject.SetActive(true);
        sceneExit.OnPlayerExit += DoExit;
        circleTargetAlpha = 1f;
        canExit = false;
    }

    void DoExit()
    {
        demonGuy.gameObject.SetActive(false);
        Instantiate(despawnAnimationPrefab, demonGuy.transform.position + Vector3.up * 0.4f, despawnAnimationPrefab.transform.rotation);

        StartCoroutine(LoadScene("CombatScene"));
    }

    IEnumerator LoadScene(string scene)
    {
        yield return new WaitForSeconds(1f);
        spawnCircle.sortingOrder = 2;
        blackSquareTargetAlpha = 1f;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scene);
    }
    
    void SetBlackSquareAlpha(float value)
    {
        var current = blackSquare.color;
        blackSquare.color = new Color(current.r, current.g, current.b, value);
    }
    
    void Update()
    {
        if(canExit && !babyGame.isStartable && !ovenGame.isStartable) PrepareExit();
        
        Color cur = spawnCircle.color;
        spawnCircle.color = new Color(cur.r, cur.g, cur.b, cur.a + (circleTargetAlpha - cur.a) * 3f * Time.deltaTime);
        
        SetBlackSquareAlpha(blackSquare.color.a + Mathf.Sign(blackSquareTargetAlpha - blackSquare.color.a) * 1f * Time.deltaTime);
        
        switch (_currentState)
        {
            case HOUSE_STATE.Default:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (Vector3.Distance(player.transform.position, ovenPoint.position) < interactionRange)
                    {
                       if(ovenGame.isStartable) SwitchToOven();
                    }

                    if (Vector3.Distance(player.transform.position, cribPoint.position) < interactionRange)
                    {
                        if(babyGame.isStartable) SwitchToCrib();
                    }
                    
                }
                break;
            }
            
            case HOUSE_STATE.CribGame:
            case HOUSE_STATE.OvenGame:
            {
                player.TargetVelocity = currentTargetPosition - player.transform.position;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //SwitchToDefault();
                }

                break;
            }
        }
    }

    public void SwitchToOven()
    {
        cinemachineAnimator.Play("Oven");
        _currentState = HOUSE_STATE.OvenGame;

        player.allowInput = false;
        
        ovenGame.isActive = true;
        currentTargetPosition = ovenStandPosition.position;
        
        ovenGame.StartOvenGame();
    }

    public void SwitchToCrib()
    {
        cinemachineAnimator.Play("Crib");
        _currentState = HOUSE_STATE.CribGame;
        currentTargetPosition = cribStandPosition.position;

        babyGame.isActive = true;

        player.allowInput = false;
    }

    public void SwitchToDefault()
    {
        if(_currentState == HOUSE_STATE.OvenGame) ovenGame.EndOvenGame();

        player.allowInput = true;

        babyGame.isActive = false;
        ovenGame.isActive = false;
        
        cinemachineAnimator.Play("Default");
        _currentState = HOUSE_STATE.Default;
        
    }
}
