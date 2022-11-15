using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.Screen
{
    public class ReadingRenderScreen : MonoBehaviour
    {
        [SerializeField]
        private Transform cornerLT;

        [SerializeField]
        private Transform cornerRT;

        [SerializeField]
        private Transform cornerRB;

        [SerializeField]
        private Transform cornerLB;

        [SerializeField]
        private LineRenderer lineTop;

        [SerializeField]
        private LineRenderer lineRight;

        [SerializeField]
        private LineRenderer lineBottom;

        [SerializeField]
        private LineRenderer lineLeft;

        [SerializeField]
        private GameObject ResetButton;

        [SerializeField]
        private GameObject StartButton;

        [SerializeField]
        private GameObject StopButton;

        [SerializeField]
        private TextMesh distanceTextMesh;

        // Start is called before the first frame update
        void Start()
        {
            
            
        }

        

        // Update is called once per frame
        void Update()
        {
            if (this.distanceTextMesh.gameObject.activeInHierarchy)
            {
                var center = (cornerLT.position + cornerLB.position + cornerRT.position + cornerRB.position) / 4f;
                this.distanceTextMesh.text = ((int)(Vector3.Distance(Camera.main.transform.position, center) * 100f)).ToString();
            }
        }

        public void Init()
        {
            this.CorrectUp();
            this.SetCorners();
            this.ReFrameScreen();
        }

        private void CorrectUp()
        {
            this.transform.forward = Camera.main.transform.forward;
        }

        private void SetCorners()
        {
            var width = 0.5f;// Config.ConfigManager.Instance.Config.screenWidth / 100f;
            var height = 0.5f;// Config.ConfigManager.Instance.Config.screenHeight / 100f;

            this.cornerLT.position = this.transform.position;
            this.cornerRT.position = this.transform.position + this.transform.right * width;
            this.cornerLB.position = this.transform.position - this.transform.up * height;
            this.cornerRB.position = this.transform.position - this.transform.up * height + this.transform.right * width - 0.01f * this.transform.forward;
        }

        public void ReFrameScreen()
        {
            this.lineTop.SetPosition(0, cornerLT.position);
            this.lineTop.SetPosition(1, cornerRT.position);
            this.lineRight.SetPosition(0, cornerRT.position);
            this.lineRight.SetPosition(1, cornerRB.position);
            this.lineBottom.SetPosition(0, cornerRB.position);
            this.lineBottom.SetPosition(1, cornerLB.position);
            this.lineLeft.SetPosition(0, cornerLB.position);
            this.lineLeft.SetPosition(1, cornerLT.position);
        }

        //public void ResetScreen()
        //{
            //QRCode.QRCodeManager.Instance.ResetIsQRCodeObjExist();
            //Destroy(this.gameObject, 1f);
        //}

        public List<GameObject> GetCorners()
        {
            var corners = new List<GameObject>();
            corners.Add(cornerLT.gameObject);
            corners.Add(cornerRT.gameObject);
            corners.Add(cornerLB.gameObject);
            corners.Add(cornerRB.gameObject);
            return corners;
        }

        public void SwitchResetButton(bool enable)
        {
            this.ResetButton.SetActive(enable);
        }

        public void SwitchStartButton(bool enable)
        {
            this.StartButton.SetActive(enable);
        }

        public void SwitchStopButton(bool enable)
        {
            this.StopButton.SetActive(enable);
        }

        public void ClickReset()
        {
            Audio.AudioManager.Instance.PlayAudio("button");
            //MeshCreater.Instance.Reset();
        }

        public void ClickStart()
        {
            Audio.AudioManager.Instance.PlayAudio("button");
            //MeshCreater.Instance.Play();
        }

        public void ClickStop()
        {
            Audio.AudioManager.Instance.PlayAudio("button");
            //MeshCreater.Instance.Stop();
        }
    }
}
