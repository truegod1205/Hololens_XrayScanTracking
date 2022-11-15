using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum XrayState
{
    None,
    Init,
    Scanning,
    Prepare,
    Play,
    Wait,
}

namespace XRayScan.GameStage
{
    public class XrayStateManager : MonoBehaviour
    {
        private static XrayStateManager instance = null;
        public static XrayStateManager Instance
        {
            get
            {
                return instance;
            }
        }

        private XrayState preXrayState = XrayState.None;
        private XrayState currentXrayState = XrayState.None;

        public XrayState CurrentXrayState
        {
            get
            {
                return this.currentXrayState;
            }
        }

        protected void Awake()
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
        }
        /*
        // Start is called before the first frame update
        void Start()
        {
            this.StartCoroutine(this.DelayStartStateMachine());
        }

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator DelayStartStateMachine()
        {
            yield return null;
            this.SetXrayState(XrayState.Init);
        }

        public void SetXrayState(XrayState state)
        {
            if (this.currentXrayState == state)
            {
                return;
            }

            this.preXrayState = this.currentXrayState;
            switch (this.currentXrayState)
            {
                case XrayState.Init:
                    this.LeaveInitState();
                    break;
                case XrayState.Scanning:
                    this.LeaveScanningState();
                    break;
                case XrayState.Prepare:
                    this.LeavePrepareState();
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

            this.currentXrayState = state;
            switch (this.currentXrayState)
            {
                case XrayState.Init:
                    this.EnterInitState();
                    break;
                case XrayState.Scanning:
                    this.EnterScanningState();
                    break;
                case XrayState.Prepare:
                    this.EnterPrepareState();
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
            //XRayScan.QRCode.QRCodeManager.Instance.QRCodeSetupFinished += this.ListenQRCodeManagerReady;
            XRayScan.QRCode.QRCodeManager.Instance.AllowSetup(true);

        }

        private void LeaveInitState()
        {
            Debug.Log("LeaveInitState()");
            XRayScan.QRCode.QRCodeManager.Instance.AllowSetup(false);
            //XRayScan.QRCode.QRCodeManager.Instance.QRCodeSetupFinished -= this.ListenQRCodeManagerReady;
        }

        private void EnterScanningState()
        {
            Debug.Log("EnterScanningState()");
            XRayScan.QRCode.QRCodeManager.Instance.OnQRCodeScanned += this.ListenQRCodeScanned;
            XRayScan.QRCode.QRCodeManager.Instance.isQRCodeObjExist = false;
            XRayScan.QRCode.QRCodeManager.Instance.SwitchDetector(true);
        }

        private void LeaveScanningState()
        {
            Debug.Log("LeaveScanningState()");
            XRayScan.QRCode.QRCodeManager.Instance.SwitchDetector(false); ;
            XRayScan.QRCode.QRCodeManager.Instance.OnQRCodeScanned -= this.ListenQRCodeScanned;
        }

        private void EnterPrepareState()
        {
            Debug.Log("EnterPrepareState()");
            XRayScan.Screen.MeshCreater.Instance.MeshCreaterSetupFinished += this.ListenMeshCreaterComplete;

        }

        private void LeavePrepareState()
        {
            Debug.Log("LeavePrepareState()");
            XRayScan.Screen.MeshCreater.Instance.MeshCreaterSetupFinished -= this.ListenMeshCreaterComplete;
        }

        private void EnterWaitState()
        {
            Debug.Log("EnterWaitState()");

        }

        private void LeaveWaitState()
        {
            Debug.Log("LeaveWaitState()");
        }

        private void EnterPlayState()
        {
            Debug.Log("EnterPlayState()");

        }

        private void LeavePlayState()
        {
            Debug.Log("LeavePlayState()");
        }

        // callbacks
        private void ListenQRCodeManagerReady()
        {
            this.SetXrayState(XrayState.Scanning);
        }

        private void ListenQRCodeScanned(Vector3 position, Quaternion pose)
        {
            Debug.Log("ListenQRCodeScanned()");
            this.SetXrayState(XrayState.Prepare);
            XRayScan.Screen.MeshCreater.Instance.InitScreen(position, pose);
        }

        private void ListenMeshCreaterComplete()
        {
            Debug.Log("ListenMeshCreaterComplete()");
            this.SetXrayState(XrayState.Wait);
        }
        */
    }
}