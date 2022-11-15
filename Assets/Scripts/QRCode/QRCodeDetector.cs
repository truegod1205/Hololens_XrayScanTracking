using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Microsoft.MixedReality.QR;

using Microsoft.MixedReality.Toolkit.UI;
#if WINDOWS_UWP
using Windows.Perception.Spatial;
#endif

namespace XRayScan.QRCode
{
    public class QRCodeDetector : QRCodeListener
    {
        public string ExpectedQRCodeText;
        //public GameObject qrCodePrefab;
        public UnityEvent onQRCode;
        private Pose QRPose;

#if WINDOWS_UWP
        private SpatialCoordinateSystem CoordinateSystem = null;
#endif

        // Use this for initialization
        public override void Start()
        {
            Debug.Log("QRCodeDetector waiting for " + ExpectedQRCodeText);
            base.Start();
            this.enabled = false;
        }

        public override void HandleQRCodeAdded(Microsoft.MixedReality.QR.QRCode qrCode)
        {
            Debug.Log("HandleQRCodeAdded");
            // new QR code detected

            onQRCode.Invoke();
            var found = GetPoseFromSpatialNode(qrCode.SpatialGraphNodeId, out QRPose);
            if (found)
            {
                var qrVec = QRPose.position - Camera.main.transform.position;
                if (Vector3.Angle(qrVec, Camera.main.transform.forward) < 10f)
                {
                    //Instantiate(qrCodePrefab, QRPose.position, QRPose.rotation);
                    QRCodeManager.Instance.OnQRCodeScanned?.Invoke(QRPose.position, QRPose.rotation);
                }
            }

            /*
            if (ExpectedQRCodeText == qrCode.Data)
            {


                the_qrcode = qrCode;
                onQRCode.Invoke();
            }
            */
        }

        public override void HandleQRCodeUpdated(Microsoft.MixedReality.QR.QRCode qrCode)
        {
            Debug.Log("HandleQRCodeUpdated");
            // new QR code detected

            onQRCode.Invoke();
            var found = GetPoseFromSpatialNode(qrCode.SpatialGraphNodeId, out QRPose);
            if (found)
            {
                var qrVec = QRPose.position - Camera.main.transform.position;
                if (Vector3.Angle(qrVec, Camera.main.transform.forward) < 10f)
                {
                    QRCodeManager.Instance.OnQRCodeScanned?.Invoke(QRPose.position, QRPose.rotation);
                }
            }

            /*
            if (ExpectedQRCodeText == qrCode.Data)
            {


                the_qrcode = qrCode;
                onQRCode.Invoke();
            }
            */
        }

        private bool GetPoseFromSpatialNode(System.Guid nodeId, out Pose pose)
        {

            bool found = false;
            pose = Pose.identity;

#if WINDOWS_UWP
                
                CoordinateSystem = Windows.Perception.Spatial.Preview.SpatialGraphInteropPreview.CreateCoordinateSystemForNode(nodeId);
                

                if (CoordinateSystem != null)
                {
                    //info.text += "\ngot coordinate";
                    Quaternion rotation = Quaternion.identity;
                    Vector3 translation = new Vector3(0.0f, 0.0f, 0.0f);

                    SpatialCoordinateSystem rootSpatialCoordinateSystem = (SpatialCoordinateSystem)System.Runtime.InteropServices.Marshal.GetObjectForIUnknown(UnityEngine.XR.WSA.WorldManager.GetNativeISpatialCoordinateSystemPtr());

                    // Get the relative transform from the unity origin
                    System.Numerics.Matrix4x4? relativePose = CoordinateSystem.TryGetTransformTo(rootSpatialCoordinateSystem);

                    if (relativePose != null)
                    {
                        //info.text += "\n got relative pose";
                        System.Numerics.Vector3 scale;
                        System.Numerics.Quaternion rotation1;
                        System.Numerics.Vector3 translation1;
       
                        System.Numerics.Matrix4x4 newMatrix = relativePose.Value;

                        // Platform coordinates are all right handed and unity uses left handed matrices. so we convert the matrix
                        // from rhs-rhs to lhs-lhs 
                        // Convert from right to left coordinate system
                        newMatrix.M13 = -newMatrix.M13;
                        newMatrix.M23 = -newMatrix.M23;
                        newMatrix.M43 = -newMatrix.M43;

                        newMatrix.M31 = -newMatrix.M31;
                        newMatrix.M32 = -newMatrix.M32;
                        newMatrix.M34 = -newMatrix.M34;

                        System.Numerics.Matrix4x4.Decompose(newMatrix, out scale, out rotation1, out translation1);
                        translation = new Vector3(translation1.X, translation1.Y, translation1.Z);
                        rotation = new Quaternion(rotation1.X, rotation1.Y, rotation1.Z, rotation1.W);
                        pose = new Pose(translation, rotation);
                        found = true;
                      

                        // can be used later using gameObject.transform.SetPositionAndRotation(pose.position, pose.rotation);
                        //Debug.Log("Id= " + id + " QRPose = " +  pose.position.ToString("F7") + " QRRot = "  +  pose.rotation.ToString("F7"));
                    } else {
                          //info.text += "\nrelative pos NULL";
                    }
                } else {
                  //info.text += "\ncannot retrieve coordinate";
                }
                
#endif
            return found;

        }

        void Update()
        {
            base.Update();
        }
    }
}