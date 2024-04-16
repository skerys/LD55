using System.Collections.Generic;
using Unity.VisualScripting;
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
   // [FilePath("Assets/Data/GameStateManager.asset", FilePathAttribute.Location.ProjectFolder)]
    public class GameStateManager : ScriptableObject
    {
        public bool NeedToShowStartMenu = false;
        public bool OvenGameStartable = false;
        public bool BabyGameStartable = false;

        public int MeleeCount = 10;
        public int RangedCount = 0;
        public int BarbarianCount = 0;

        public int currentStage;
        public List<StagedInfo> infoByStage = new List<StagedInfo>();
        
        
        private static GameStateManager _instance;
        public static GameStateManager instance {
            get {
                if (_instance == null)
                {
                    _instance = Resources.Load<GameStateManager>("GameStateManager");
                }
                return _instance;
            }
        }
        
        public void NextStage()
        {
            currentStage++;
            var nextStageInfo = infoByStage[Mathf.Min(currentStage, infoByStage.Count - 1)];

            NeedToShowStartMenu = nextStageInfo.startMenu;
            OvenGameStartable = nextStageInfo.ovenGame;
            BabyGameStartable = nextStageInfo.babyGame;

            MeleeCount = nextStageInfo.meleeCount;
            RangedCount = nextStageInfo.rangedCount;
            BarbarianCount = nextStageInfo.barbarianCount;
        }
    }
}