using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ReadingUI : MonoBehaviour
    {
        [SerializeField]
        private ReadingMenuUI ReadingMenuPage;

        [SerializeField]
        private GameObject MaximizeButton;

        [SerializeField]
        private GameObject ScanQRCodeHint;

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
            this.ReadingMenuPage.gameObject.SetActive(false);
            this.ScanQRCodeHint.gameObject.SetActive(false);
            this.MaximizeButton.SetActive(false);
        }

        public void MaxmizeReadingMenu()
        {
            this.MaximizeButton.SetActive(false);
            this.ReadingMenuPage.gameObject.SetActive(true);
            this.ReadingMenuPage.InitPage();
        }

        public void MinimizeReadingMenu()
        {
            this.ReadingMenuPage.gameObject.SetActive(false);
            this.MaximizeButton.SetActive(true);
        }

        public void EnableScanQRCodeHint(bool enable)
        {
            this.ScanQRCodeHint.gameObject.SetActive(enable);
        }

        public void GoToMainStage()
        {
            XRayScan.GameStage.GameStageManager.Instance.SetGameStage(XRayScan.GameStage.GameStageType.MainStage);
        }
    }
}