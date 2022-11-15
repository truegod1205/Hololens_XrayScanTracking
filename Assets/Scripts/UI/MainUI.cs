using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField]
        private MainMenuUI MainMenuPage;

        [SerializeField]
        private SettingsPageUI SettingsPage;

        [SerializeField]
        private GameObject MaximizeButton;

        [SerializeField]
        private TextMesh stateText;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        public void InitUI()
        {
            this.MaximizeButton.SetActive(true);
        }

        public void FinishUI()
        {
            this.MainMenuPage.gameObject.SetActive(false);
            this.SettingsPage.gameObject.SetActive(false);
            this.MaximizeButton.SetActive(false);
        }

        public void OpenSettingsPage()
        {
            this.MainMenuPage.gameObject.SetActive(false);
            this.SettingsPage.gameObject.SetActive(true);
            this.SettingsPage.InitPage();
        }

        public void BackToMainMenuPage(GameObject uiObject)
        {
            uiObject.SetActive(false);
            this.MainMenuPage.gameObject.SetActive(true);
            this.MainMenuPage.InitPage();
        }

        public void MaxmizeMainmenu()
        {
            this.MaximizeButton.SetActive(false);
            this.MainMenuPage.gameObject.SetActive(true);
            this.MainMenuPage.InitPage();
        }

        public void MinimizeMainmenu()
        {
            this.MainMenuPage.gameObject.SetActive(false);
            this.MaximizeButton.SetActive(true);
        }
    }
}