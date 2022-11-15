using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.QR;

namespace XRayScan.QRCode
{
    public class QRCodeManager : MonoBehaviour
    {
        private static QRCodeManager instance = null;
        public static QRCodeManager Instance
        {
            get
            {
                return instance;
            }
        }

        [Tooltip("Determines if the QR codes scanner should be automatically started.")]
        public bool AutoStartQRTracking = true;

        public bool IsTrackerRunning { get; private set; }

        public bool IsSupported { get; private set; }

        public event EventHandler<bool> QRCodesTrackingStateChanged;
        public delegate void QREventReceived(Microsoft.MixedReality.QR.QRCode code, QREventType eventType);
        public QREventReceived OnQRCode;

        public Action<Vector3, Quaternion> OnQRCodeScanned;

        [SerializeField]
        private QRCodeDetector detector;

        private System.Collections.Generic.SortedDictionary<System.Guid, Microsoft.MixedReality.QR.QRCode> qrCodesList = new SortedDictionary<System.Guid, Microsoft.MixedReality.QR.QRCode>();

        private QRCodeWatcher qrTracker;
        private bool capabilityInitialized = false;
        private bool allowSetup = false; 
        private QRCodeWatcherAccessStatus accessStatus;
        private System.Threading.Tasks.Task<QRCodeWatcherAccessStatus> capabilityTask;

        public System.Guid GetIdForQRCode(string qrCodeData)
        {
            lock (qrCodesList)
            {
                foreach (var ite in qrCodesList)
                {
                    if (ite.Value.Data == qrCodeData)
                    {
                        return ite.Key;
                    }
                }
            }
            return new System.Guid();
        }

        public System.Collections.Generic.IList<Microsoft.MixedReality.QR.QRCode> GetList()
        {
            lock (qrCodesList)
            {
                return new List<Microsoft.MixedReality.QR.QRCode>(qrCodesList.Values);
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

        // Use this for initialization
        async protected virtual void Start()
        {
            IsSupported = QRCodeWatcher.IsSupported();
            capabilityTask = QRCodeWatcher.RequestAccessAsync();
            accessStatus = await capabilityTask;
            capabilityInitialized = true;
        }

        private void SetupQRTracking()
        {
            try
            {
                qrTracker = new QRCodeWatcher();
                qrTracker.Stop();

                qrCodesList.Clear();
                IsTrackerRunning = false;
                qrTracker.Added += QRCodeWatcher_Added;
                qrTracker.Updated += QRCodeWatcher_Updated;
                qrTracker.Removed += QRCodeWatcher_Removed;
                qrTracker.EnumerationCompleted += QRCodeWatcher_EnumerationCompleted;
            }
            catch (Exception ex)
            {
                Debug.Log("QRCodesManager : exception starting the tracker " + ex.ToString());
            }

            if (AutoStartQRTracking)
            {
                StartQRTracking();
            }
        }

        public void StartQRTracking()
        {
            if (qrTracker != null && !IsTrackerRunning)
            {
                Debug.Log("QRCodesManager starting QRCodeWatcher");
                try
                {
                    qrTracker.Start();
                    IsTrackerRunning = true;
                    QRCodesTrackingStateChanged?.Invoke(this, true);
                    Debug.Log("QRCodesManager starting Over");
                }
                catch (Exception ex)
                {
                    Debug.Log("QRCodesManager starting QRCodeWatcher Exception:" + ex.ToString());
                }
            }
        }

        public void StopQRTracking()
        {
            if (IsTrackerRunning)
            {
                IsTrackerRunning = false;
                if (qrTracker != null)
                {
                    qrTracker.Stop();
                    qrCodesList.Clear();
                }

                var handlers = QRCodesTrackingStateChanged;
                if (handlers != null)
                {
                    handlers(this, false);
                }
            }
        }

        private void QRCodeWatcher_Removed(object sender, QRCodeRemovedEventArgs args)
        {
            Debug.Log("QRCodesManager QRCodeWatcher_Removed");


            lock (qrCodesList)
            {
                if (qrCodesList.ContainsKey(args.Code.Id))
                {
                    qrCodesList.Remove(args.Code.Id);

                }
            }
            if (OnQRCode != null)
            {
                OnQRCode(args.Code, QREventType.Removed);
            }
        }

        private void QRCodeWatcher_Updated(object sender, QRCodeUpdatedEventArgs args)
        {
            Debug.Log("QRCodesManager QRCodeWatcher_Updated");

            // change logic : do nothing on added and act on first update
            bool found = false;
            lock (qrCodesList)
            {
                found = qrCodesList.ContainsKey(args.Code.Id);

                qrCodesList[args.Code.Id] = args.Code;
            }
            if (!found) // this is what we use for added
            {
                OnQRCode(args.Code, QREventType.Added);
            }
            else
            {
                OnQRCode(args.Code, QREventType.Updated);
            }
            /*
             bool found = false;
            lock (qrCodesList)
            {
                if (qrCodesList.ContainsKey(args.Code.Id))
                {
                    found = true;
                    qrCodesList[args.Code.Id] = args.Code;
                }
            }
            if (found)
            {
                if (OnQRCode != null)
                {
                    OnQRCode(args.Code, QREventType.Updated);
                }
            }
            */
        }

        private void QRCodeWatcher_Added(object sender, QRCodeAddedEventArgs args)
        {
            Debug.Log("QRCodesManager QRCodeWatcher_Added");
            // change logic : do nothing on added and act on first update
            /*
            lock (qrCodesList)
            {
                qrCodesList[args.Code.Id] = args.Code;
            }
            if (OnQRCode != null)
            {
                OnQRCode(args.Code, QREventType.Added);
            }
            */
        }

        private void QRCodeWatcher_EnumerationCompleted(object sender, object e)
        {
            Debug.Log("QRCodesManager QrTracker_EnumerationCompleted");
        }

        void Update()
        {
            if (this.allowSetup)
            {
                if (qrTracker == null && capabilityInitialized && IsSupported)
                {
                    if (accessStatus == QRCodeWatcherAccessStatus.Allowed)
                    {
                        SetupQRTracking();
                    }
                    else
                    {
                        Debug.Log("Capability access status : " + accessStatus);
                    }
                }
            }
        }

        public void SwitchDetector(bool enable)
        {
            this.detector.enabled = enable;
        }

        public void AllowSetup(bool enable)
        {
            Debug.Log("AllowSetup");
            this.allowSetup = enable;
        }
    }
}