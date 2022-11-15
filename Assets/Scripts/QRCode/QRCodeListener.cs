using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.QRCode
{
    public enum QREventType
    {
        Added,
        Updated,
        Removed
    };

    struct ActionData
    {

        public QREventType type;
        public Microsoft.MixedReality.QR.QRCode qrCode;

        public ActionData(QREventType type, Microsoft.MixedReality.QR.QRCode qRCode) : this()
        {
            this.type = type;
            qrCode = qRCode;
        }
    }
    public interface IQRCodeListener
    {
        void OnQRCode(Microsoft.MixedReality.QR.QRCode code, QREventType eventType);
        // event Type is create update remove
    }


    public class QRCodeListener : MonoBehaviour, IQRCodeListener
    {

        private System.Collections.Generic.Queue<ActionData> pendingActions = new Queue<ActionData>();
        void Awake()
        {

        }

        // Use this for initialization
        public virtual void Start()
        {
            Debug.Log("QRCodeListener start");


            QRCodeManager.Instance.QRCodesTrackingStateChanged += Instance_QRCodesTrackingStateChanged;
            QRCodeManager.Instance.OnQRCode += OnQRCode;


        }
        private void Instance_QRCodesTrackingStateChanged(object sender, bool status)
        {
            Debug.Log("QRCodesTrackingStateChanged  " + status);

        }
        public virtual void OnQRCode(Microsoft.MixedReality.QR.QRCode code, QREventType eventType)
        {
            Debug.Log("QRCodeListener  " + eventType);
            lock (pendingActions)
            {
                pendingActions.Enqueue(new ActionData(eventType, code));
            }

        }
        public virtual void HandleQRCodeAdded(Microsoft.MixedReality.QR.QRCode code)
        {

        }
        public virtual void HandleQRCodeUpdated(Microsoft.MixedReality.QR.QRCode code)
        {

        }
        public virtual void HandleQRCodeRemoved(Microsoft.MixedReality.QR.QRCode code)
        {

        }

        private void HandleEvents()
        {
            lock (pendingActions)
            {
                while (pendingActions.Count > 0)
                {
                    var action = pendingActions.Dequeue();


                    switch (action.type)
                    {
                        case QREventType.Added:

                            HandleQRCodeAdded(action.qrCode);
                            break;
                        case QREventType.Updated:
                            HandleQRCodeUpdated(action.qrCode);
                            break;
                        case QREventType.Removed:
                            HandleQRCodeRemoved(action.qrCode);
                            break;

                    }
                }
            }

        }

        // Update is called once per frame
        protected void Update()
        {
            HandleEvents();
        }
    }
}