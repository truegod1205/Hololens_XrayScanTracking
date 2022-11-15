using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.GameStage
{
    public class XrayTestStateController : MonoBehaviour
    {
        [SerializeField]
        private XRayScan.Screen.MeshCreater meshCreator;

        public Action<bool> OnEnableScanningHint;

        private XrayState preXrayState = XrayState.None;
        private XrayState currentXrayState = XrayState.None;

        public XrayState CurrentXrayState
        {
            get
            {
                return this.currentXrayState;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartStateMachine()
        {
            this.meshCreator.OnClickStartTest += this.ListenClickStartTest;
            this.meshCreator.OnBackToWait += this.ListenOnBackToWait;
            this.meshCreator.OnClickRescanQRCode += this.ListenClickRescanQRCode;
            this.SetXrayState(XrayState.Init);
        }

        public void ShutDownStateMachine() 
        {
            this.SetXrayState(XrayState.None);
            this.meshCreator.SwitchRescanButton(false);
            this.meshCreator.SwitchStartButton(false);
            this.meshCreator.SwitchRestartButton(false);
            this.meshCreator.SwitchDistance(false);
            this.meshCreator.SwitchTimer(false);
            this.meshCreator.DeInitScreen();
            this.meshCreator.OnClickStartTest -= this.ListenClickStartTest;
            this.meshCreator.OnBackToWait -= this.ListenOnBackToWait;
            this.meshCreator.OnClickRescanQRCode -= this.ListenClickRescanQRCode;
        }

        private void SetXrayState(XrayState state)
        {
            if (this.currentXrayState == state)
            {
                return;
            }

            this.preXrayState = this.currentXrayState;
            this.LeaveCurrentState();

            this.currentXrayState = state;
            this.EnterCurrentState();

        }

        private void LeaveCurrentState()
        {
            switch (this.currentXrayState)
            {
                case XrayState.Init:
                    this.LeaveInitState();
                    break;
                case XrayState.Scanning:
                    this.LeaveScanningState();
                    break;
                case XrayState.Play:
                    this.LeavePlayState();
                    break;
                case XrayState.Wait:
                    this.LeaveWaitState();
                    break;
                case XrayState.None:
                default:
                    break;
            }
            return;
        }

        private void EnterCurrentState()
        {
            switch (this.currentXrayState)
            {
                case XrayState.Init:
                    this.EnterInitState();
                    break;
                case XrayState.Scanning:
                    this.EnterScanningState();
                    break;
                case XrayState.Play:
                    this.EnterPlayState();
                    break;
                case XrayState.Wait:
                    this.EnterWaitState();
                    break;
                case XrayState.None:
                default:
                    break;
            }
        }

        private void EnterInitState()
        {
            Debug.Log("EnterInitState()");
            this.SetXrayState(XrayState.Scanning);
        }

        private void LeaveInitState()
        {
            Debug.Log("LeaveInitState()");
        }

        private void EnterScanningState()
        {
            Debug.Log("EnterScanningState()");
            this.meshCreator.InitScreen();
            this.meshCreator.RegisterScreenSetupFinished(this.ListenAllQRCodeScanned);
            XRayScan.QRCode.QRCodeManager.Instance.OnQRCodeScanned += this.ListenQRCodeScanned;
            XRayScan.QRCode.QRCodeManager.Instance.SwitchDetector(true);
            this.OnEnableScanningHint?.Invoke(true);
            this.meshCreator.SwitchRescanButton(false);
            this.meshCreator.SwitchStartButton(false);
            this.meshCreator.SwitchRestartButton(false);
            this.meshCreator.SwitchDistance(false);
            this.meshCreator.SwitchTimer(false);
        }

        private void LeaveScanningState()
        {
            Debug.Log("LeaveScanningState()");
            this.OnEnableScanningHint?.Invoke(false);
            XRayScan.QRCode.QRCodeManager.Instance.SwitchDetector(false);
            XRayScan.QRCode.QRCodeManager.Instance.OnQRCodeScanned -= this.ListenQRCodeScanned;
            this.meshCreator.UnregisterScreenSetupFinished(this.ListenAllQRCodeScanned);
        }

        private void EnterWaitState()
        {
            Debug.Log("EnterWaitState()");
            this.meshCreator.SwitchRescanButton(true);
            this.meshCreator.SwitchStartButton(true);
            this.meshCreator.SwitchRestartButton(false);
            this.meshCreator.SwitchDistance(true);
            this.meshCreator.SwitchTimer(true);
        }

        private void LeaveWaitState()
        {
            Debug.Log("LeaveWaitState()");
        }

        private void EnterPlayState()
        {
            Debug.Log("EnterPlayState()");
            this.meshCreator.SwitchRescanButton(true);
            this.meshCreator.SwitchStartButton(false);
            this.meshCreator.SwitchRestartButton(false);
            this.meshCreator.SwitchDistance(true);
            this.meshCreator.SwitchTimer(true);
        }

        private void LeavePlayState()
        {
            Debug.Log("LeavePlayState()");
        }

        // callbacks

        private void ListenQRCodeScanned(Vector3 position, Quaternion pose)
        {
            Debug.Log("ListenQRCodeScanned()");
            this.meshCreator.AddGridCorner(position, pose);
        }

        private void ListenAllQRCodeScanned()
        {
            Debug.Log("ListenAllQRCodeScanned()");
            this.SetXrayState(XrayState.Wait);
        }

        private void ListenClickStartTest()
        {
            this.SetXrayState(XrayState.Play);
        }

        private void ListenOnBackToWait()
        {
            this.SetXrayState(XrayState.Wait);
        }

        private void ListenClickRescanQRCode()
        {
            this.SetXrayState(XrayState.Scanning);
        }
    }
}