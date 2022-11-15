using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.GameStage
{
    public class ReadingGameStage : GameStage
    {
        [SerializeField]
        private UI.ReadingUI readingUI;

        [SerializeField]
        private XrayReadingStateController readingStateController;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void EnterStage()
        {
            this.ResetUI();
            this.readingStateController.OnEnableScanningHint += this.EnableScanningHint;
            this.readingStateController.StartStateMachine();
        }

        public override void LeaveStage()
        {
            this.readingStateController.ShutDownStateMachine();
            this.readingStateController.OnEnableScanningHint -= this.EnableScanningHint;
            this.CloseUI();
        }

        private void EnableScanningHint(bool enable)
        {
            this.readingUI.EnableScanQRCodeHint(enable);
        }

        private void ResetUI()
        {
            this.readingUI.gameObject.SetActive(true);
            this.readingUI.InitUI();
        }

        private void CloseUI()
        {
            this.readingUI.FinishUI();
            this.readingUI.gameObject.SetActive(false);
        }
    }
}