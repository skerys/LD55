using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "ImprovementLibrary.asset", menuName = "Liutauras/Create Improvements Library", order = 0)]
public class ImprovementLibrary : ScriptableObject
{
    public enum ImprovementType
    {
        ShorterCooldown = 0,
        AdditionalHealth = 1,
        FasterMoveSpeed = 2,
        LongerDash = 3,
        FasterFireballs = 4,
        BabyPowerup = 5,
        Length = 6
    };

    public bool[] improvements = new bool[(int)ImprovementType.Length];

    public void EnableImprovement(ImprovementType type)
    {
        improvements[(int)type] = true;
    }

    public void DisableImprovement(ImprovementType type)
    {
        improvements[(int)type] = false;
    }
}
