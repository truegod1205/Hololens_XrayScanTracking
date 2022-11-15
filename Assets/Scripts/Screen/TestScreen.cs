using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.Screen
{
    public class TestScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject cornerPrefab;

        [SerializeField]
        private LineRenderer lineTop;

        [SerializeField]
        private LineRenderer lineRight;

        [SerializeField]
        private LineRenderer lineBottom;

        [SerializeField]
        private LineRenderer lineLeft;

        [SerializeField]
        private Transform ControlSet;

        [SerializeField]
        private GameObject RescanButton;

        [SerializeField]
        private GameObject StartButton;

        [SerializeField]
        private GameObject RestartButton;

        [SerializeField]
        private GameObject DistanceDisplay;

        [SerializeField]
        private GameObject TimerDisplay;

        [SerializeField]
        private TextMesh distanceTextMesh;

        private List<GameObject> gridCorners;

        public Action ScreenSetupFinished;

        public Action OnClickRescan;

        public Action OnClickStart;

        public Action OnClickRestart;

        private void Awake()
        {
            this.gridCorners = new List<GameObject>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
            
        }

        

        // Update is called once per frame
        void Update()
        {
            if (this.distanceTextMesh.gameObject.activeInHierarchy && this.gridCorners.Count >= 4)
            {
                var center = (this.gridCorners[0].transform.position + this.gridCorners[1].transform.position + 
                    this.gridCorners[2].transform.position + this.gridCorners[3].transform.position) / 4f;
                this.distanceTextMesh.text = ((int)(Vector3.Distance(Camera.main.transform.position, center) * 100f)).ToString("#0.00");
            }
        }


        public void DestroyRectangle()
        {
            for (int i = this.gridCorners.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(this.gridCorners[i]);
            }
            this.gridCorners.Clear();
            this.lineTop.gameObject.SetActive(false);
            this.lineBottom.gameObject.SetActive(false);
            this.lineLeft.gameObject.SetActive(false);
            this.lineRight.gameObject.SetActive(false);
        }

        public void AddGridCorner(Vector3 position, Quaternion pose)
        {
            for (int i = 0; i < this.gridCorners.Count; i++)
            {
                if (Vector3.Distance(position, this.gridCorners[i].transform.position) < 0.05f)
                {
                    return;
                }
            }

            var obj = Instantiate(cornerPrefab, position, pose);
            obj.SetActive(true);
            this.gridCorners.Add(obj);

            if (gridCorners.Count >= 4)
            {
                this.DecideCorners();
                this.ReFrameScreen();
                this.lineTop.gameObject.SetActive(true);
                this.lineBottom.gameObject.SetActive(true);
                this.lineLeft.gameObject.SetActive(true);
                this.lineRight.gameObject.SetActive(true);
                this.ScreenSetupFinished?.Invoke();
            }
        }

        private void DecideCorners()
        {
            GameObject t1, t2, b1, b2;
            var yValue = float.MinValue;
            var index = 0;
            for (int i = 0; i < this.gridCorners.Count; i++)
            {
                if (this.gridCorners[i].transform.position.y >= yValue)
                {
                    index = i;
                    yValue = this.gridCorners[i].transform.position.y;
                }
            }

            t1 = this.gridCorners[index];
            this.gridCorners.RemoveAt(index);

            yValue = float.MinValue;
            index = 0;
            for (int i = 0; i < this.gridCorners.Count; i++)
            {
                if (this.gridCorners[i].transform.position.y >= yValue)
                {
                    index = i;
                    yValue = this.gridCorners[i].transform.position.y;
                }
            }

            t2 = this.gridCorners[index];
            this.gridCorners.RemoveAt(index);

            b1 = this.gridCorners[0];
            b2 = this.gridCorners[1];

            this.gridCorners.Clear();

            var t1V = (t1.transform.position - Camera.main.transform.position).normalized;
            var t2V = (t2.transform.position - Camera.main.transform.position).normalized;
            var b1V = (b1.transform.position - Camera.main.transform.position).normalized;
            var b2V = (b2.transform.position - Camera.main.transform.position).normalized;

            var cT1T2 = Vector3.Cross(t1V, t2V).normalized;
            if (Vector3.Angle(cT1T2, Vector3.up) < 90f)
            {
                //LT = t1.transform.position;
                //RT = t2.transform.position;

                this.gridCorners.Add(t1);
                this.gridCorners.Add(t2);
            }
            else
            {
                //LT = t2.transform.position;
                //RT = t1.transform.position;
                this.gridCorners.Add(t2);
                this.gridCorners.Add(t1);
            }

            var cB1B2 = Vector3.Cross(b1V, b2V).normalized;
            if (Vector3.Angle(cB1B2, Vector3.up) < 90f)
            {
                //LB = b1.transform.position;
                //RB = b2.transform.position;
                this.gridCorners.Add(b2);
                this.gridCorners.Add(b1);
            }
            else
            {
                //LB = b2.transform.position;
                //RB = b1.transform.position;
                this.gridCorners.Add(b1);
                this.gridCorners.Add(b2);
            }
        }

        public void ReFrameScreen()
        {
            this.lineTop.SetPosition(0, gridCorners[0].transform.position);
            this.lineTop.SetPosition(1, gridCorners[1].transform.position);
            this.lineRight.SetPosition(0, gridCorners[1].transform.position);
            this.lineRight.SetPosition(1, gridCorners[2].transform.position);
            this.lineBottom.SetPosition(0, gridCorners[2].transform.position);
            this.lineBottom.SetPosition(1, gridCorners[3].transform.position);
            this.lineLeft.SetPosition(0, gridCorners[3].transform.position);
            this.lineLeft.SetPosition(1, gridCorners[0].transform.position);
            this.ControlSet.forward = -1 * Vector3.Cross(
                (this.gridCorners[1].transform.position - this.gridCorners[0].transform.position),
                (this.gridCorners[3].transform.position - this.gridCorners[0].transform.position));

            var height = Vector3.Distance(this.gridCorners[3].transform.position, this.gridCorners[0].transform.position);
            var width = Vector3.Distance(this.gridCorners[1].transform.position, this.gridCorners[0].transform.position);

            this.ControlSet.position = (this.gridCorners[0].transform.position + this.gridCorners[1].transform.position +
                    this.gridCorners[2].transform.position + this.gridCorners[3].transform.position) / 4f + 
                    this.ControlSet.up * (height / 2.0f + 0.1f);
            this.RescanButton.transform.localPosition = new Vector3(-0.5f * width - 0.1f, 0, 0);
        }

        //public void ResetScreen()
        //{
            //QRCode.QRCodeManager.Instance.ResetIsQRCodeObjExist();
            //Destroy(this.gameObject, 1f);
        //}

        public List<GameObject> GetCorners()
        {
            var corners = new List<GameObject>(this.gridCorners);
            return corners;
        }

        public void SwitchRescanButton(bool enable)
        {
            this.RescanButton.SetActive(enable);
        }

        public void SwitchStartButton(bool enable)
        {
            this.StartButton.SetActive(enable);
        }

        public void SwitchRestartButton(bool enable)
        {
            this.RestartButton.SetActive(enable);
        }

        public void SwitchTimer(bool enable)
        {
            this.TimerDisplay.SetActive(enable);
        }

        public void SwitchDistance(bool enable)
        {
            this.DistanceDisplay.SetActive(enable);
        }

        public void ClickRescan()
        {
            Audio.AudioManager.Instance.PlayAudio("button");
            this.OnClickRescan?.Invoke();
        }

        public void ClickStart()
        {
            Audio.AudioManager.Instance.PlayAudio("button");
            this.OnClickStart?.Invoke();
        }

        public void ClickRestart()
        {
            Audio.AudioManager.Instance.PlayAudio("button");
            this.OnClickRestart?.Invoke();
        }
    }
}
