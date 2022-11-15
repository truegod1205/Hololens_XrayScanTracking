using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.GameStage
{
    public enum GameStageType { InitStage = 0, MainStage, TestStage, ReadingStage };

    public class GameStageManager : MonoBehaviour
    {
        private static GameStageManager instance = null;
        public static GameStageManager Instance
        {
            get
            {
                return instance;
            }
        }

        private GameStageType currentGameStage = GameStageType.InitStage;

        public GameStageType CurrentGameStage
        {
            get
            {
                return this.currentGameStage;
            }
        }

        [SerializeField]
        private GameStage mainGameStage;

        [SerializeField]
        private GameStage testGameStage;

        [SerializeField]
        private GameStage readingGameStage;

        private Dictionary<GameStageType, GameStage> gameStageList;


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
            this.gameStageList = new Dictionary<GameStageType, GameStage>();
            this.gameStageList.Add(GameStageType.MainStage, this.mainGameStage);
            this.gameStageList.Add(GameStageType.TestStage, this.testGameStage);
            this.gameStageList.Add(GameStageType.ReadingStage, this.readingGameStage);
        }


        // Start is called before the first frame update
        void Start()
        {
            this.Initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Initialize()
        {
            Config.ConfigManager.Instance.LoadConfig();
            Localization.LocalizationManager.Instance.SetLanguage();
            this.StartCoroutine(this.DelayInitialize());
        }

        private IEnumerator DelayInitialize()
        {
            yield return null;
            XRayScan.QRCode.QRCodeManager.Instance.AllowSetup(true);
            yield return null;
            this.SetGameStage(GameStageType.MainStage);
        }

        public void SetGameStage(GameStageType stage)
        {
            if (stage == this.currentGameStage)
            {
                return;
            }

            if (this.gameStageList.ContainsKey(this.currentGameStage))
            {
                this.gameStageList[this.currentGameStage]?.LeaveStage();
            }

            this.currentGameStage = stage;

            if (this.gameStageList.ContainsKey(this.currentGameStage))
            {
                this.gameStageList[this.currentGameStage].EnterStage();
            }
        }
    }
}