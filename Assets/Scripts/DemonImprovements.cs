using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DemonImprovements : MonoBehaviour
{
    public ImprovementLibrary improvementLib;
    
    private DemonController _demon;
    private DashController _dash;
    private DemonHealth _health;

    [SerializeField] private GameObject babyTurret;

    public void Start()
    {
        _demon = GetComponent<DemonController>();
        _dash = GetComponent<DashController>();
        _health = GetComponent<DemonHealth>();
    }
    
    
    public void SetupImprovements()
    {
        Start();
        if (improvementLib.improvements[(int)ImprovementLibrary.ImprovementType.AdditionalHealth])
        {
            _health.AddMaxHealth(1);
        }

        if (improvementLib.improvements[(int)ImprovementLibrary.ImprovementType.LongerDash])
        {
            _dash.ModifyDashLength(1.5f);
        }
        
        if (improvementLib.improvements[(int)ImprovementLibrary.ImprovementType.ShorterCooldown])
        {
            _dash.ModifyDashCooldown(0.75f);
        }
        
        if (improvementLib.improvements[(int)ImprovementLibrary.ImprovementType.FasterMoveSpeed])
        {
            _demon.ModifyMoveSpeed(1.2f);
        }
        
        if (improvementLib.improvements[(int)ImprovementLibrary.ImprovementType.FasterFireballs])
        {
            babyTurret.GetComponent<BabyTurret>().ModifyShotCooldown(0.5f);
        }
        
        if (improvementLib.improvements[(int)ImprovementLibrary.ImprovementType.BabyPowerup])
        {
            babyTurret?.SetActive(true);
        }
    }
}
