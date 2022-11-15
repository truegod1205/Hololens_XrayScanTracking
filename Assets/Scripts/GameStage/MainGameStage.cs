using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.GameStage
{
    public class MainGameStage : GameStage
    {
        [SerializeField]
        private UI.MainUI mainUI;

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
        }

        public override void LeaveStage()
        {
            this.CloseUI();
        }

        private void ResetUI()
        {
            this.mainUI.gameObject.SetActive(true);
            this.mainUI.InitUI();
        }

        private void CloseUI()
        {
            this.mainUI.FinishUI();
            this.mainUI.gameObject.SetActive(false);
        }
    }
}