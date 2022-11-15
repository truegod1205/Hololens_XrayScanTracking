using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.Ray
{
    public class ScanTimer : MonoBehaviour
    {
        [SerializeField]
        private TextMesh timerText;

        public Action OnCountDown;

        private float currentTime = 0;

        private bool isInCountDown = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (this.isInCountDown)
            {
                this.currentTime -= Time.deltaTime;
                if (this.currentTime <= 0)
                {
                    this.currentTime = 0f;
                    this.OnCountDown?.Invoke();
                    //XRayScan.Screen.MeshCreater.Instance.Stop();
                }
                this.timerText.text = this.currentTime.ToString("#0.00");
            }
        }

        public void ResetTimer()
        {
            this.currentTime = Config.ConfigManager.Instance.Config.perGridTimeLimit *
                Config.ConfigManager.Instance.Config.horizontalPartition *
                Config.ConfigManager.Instance.Config.verticalPartition;
            this.timerText.text = this.currentTime.ToString("#0.00");
            this.isInCountDown = false;
        }

        public void SwitchCountDown(bool enable)
        {
            this.isInCountDown = enable;
        }
    }
}