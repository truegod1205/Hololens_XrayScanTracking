using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private MainUI mainUI;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitPage()
        {
            //TODO init setting value
        }

        public void OpenSettingsPage()
        {
            this.mainUI.OpenSettingsPage();
        }


        public void MinimizeMainmenu()
        {
            this.mainUI.MinimizeMainmenu();
        }

        public void GoToTestStage()
        {
            XRayScan.GameStage.GameStageManager.Instance.SetGameStage(XRayScan.GameStage.GameStageType.TestStage);
        }

        public void GoToReadingStage()
        {
            XRayScan.GameStage.GameStageManager.Instance.SetGameStage(XRayScan.GameStage.GameStageType.ReadingStage);
        }

        public void QuitApp()
        {
            Application.Quit();
        }
    }
}