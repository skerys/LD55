using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSceneController : MonoBehaviour
{
    [SerializeField] private Animator cinemachineAnimator;
    [SerializeField] private Transform ovenPoint;
    [SerializeField] private Transform cribPoint;
    [SerializeField] private DemonController player;
    [SerializeField] private float interactionRange;

    [SerializeField] private Transform ovenStandPosition;
    [SerializeField] private Transform cribStandPosition;

    [SerializeField] private OvenGame ovenGame;

    private Vector3 currentTargetPosition;

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
    }

    void Update()
    {
        switch (_currentState)
        {
            case HOUSE_STATE.Default:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (Vector3.Distance(player.transform.position, ovenPoint.position) < interactionRange)
                    {
                        SwitchToOven();
                    }

                    if (Vector3.Distance(player.transform.position, cribPoint.position) < interactionRange)
                    {
                        SwitchToCrib();
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
                    SwitchToDefault();
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
        currentTargetPosition = ovenStandPosition.position;
        
        ovenGame.StartOvenGame();
    }

    public void SwitchToCrib()
    {
        cinemachineAnimator.Play("Crib");
        _currentState = HOUSE_STATE.CribGame;
        currentTargetPosition = cribStandPosition.position;

        player.allowInput = false;
    }

    public void SwitchToDefault()
    {
        if(_currentState == HOUSE_STATE.OvenGame) ovenGame.EndOvenGame();

        player.allowInput = true;
        
        cinemachineAnimator.Play("Default");
        _currentState = HOUSE_STATE.Default;
        
    }
}
