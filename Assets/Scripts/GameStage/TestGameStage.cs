using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.GameStage
{
    public class TestGameStage : GameStage
    {
        [SerializeField]
        private UI.TestUI testUI;

        [SerializeField]
        private XrayTestStateController testStateController;

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
            this.testStateController.OnEnableScanningHint += this.EnableScanningHint;
            this.testStateController.StartStateMachine();
        }

        public override void LeaveStage()
        {
            this.testStateController.ShutDownStateMachine();
            this.testStateController.OnEnableScanningHint -= this.EnableScanningHint;
            this.CloseUI();
        }

        private void EnableScanningHint(bool enable)
        {
            this.testUI.EnableScanQRCodeHint(enable);
        }

        private void ResetUI()
        {
            this.testUI.gameObject.SetActive(true);
            this.testUI.InitUI();
        }

        private void CloseUI()
        {
            this.testUI.FinishUI();
            this.testUI.gameObject.SetActive(false);
        }
    }
}