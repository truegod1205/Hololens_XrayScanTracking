using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TestUI : MonoBehaviour
    {
        [SerializeField]
        private TestMenuUI TestMenuPage;

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
            this.TestMenuPage.gameObject.SetActive(false);
            this.ScanQRCodeHint.gameObject.SetActive(false);
            this.MaximizeButton.SetActive(false);
        }

        public void MaxmizeTestMenu()
        {
            this.MaximizeButton.SetActive(false);
            this.TestMenuPage.gameObject.SetActive(true);
            this.TestMenuPage.InitPage();
        }

        public void MinimizeTestMenu()
        {
            this.TestMenuPage.gameObject.SetActive(false);
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