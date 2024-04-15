using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [System.Serializable]
    public class StagedInfo
    {
        public bool startMenu;
        public bool ovenGame;
        public bool babyGame;

        public int meleeCount;
        public int rangedCount;
        public int barbarianCount;
    }
    
    
    
    [CreateAssetMenu(fileName = "GameStateManager.asset", menuName = "Liutauras/GameStateManager")]
    public class GameStateManager : ScriptableSingleton<GameStateManager>
    {
        public bool NeedToShowStartMenu = false;
        public bool OvenGameStartable = false;
        public bool BabyGameStartable = false;

        public int MeleeCount = 10;
        public int RangedCount = 0;
        public int BarbarianCount = 0;

        public int currentStage;
        public List<StagedInfo> infoByStage = new List<StagedInfo>();

        public void NextStage()
        {
            currentStage++;
            var nextStageInfo = infoByStage[Mathf.Max(currentStage, infoByStage.Count)];

            NeedToShowStartMenu = nextStageInfo.startMenu;
            OvenGameStartable = nextStageInfo.ovenGame;
            BabyGameStartable = nextStageInfo.babyGame;

            MeleeCount = nextStageInfo.meleeCount;
            RangedCount = nextStageInfo.rangedCount;
            BarbarianCount = nextStageInfo.barbarianCount;
        }
    }
}