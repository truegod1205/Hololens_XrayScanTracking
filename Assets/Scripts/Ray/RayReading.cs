using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;

namespace XRayScan.Ray
{
    public class RayReading : MonoBehaviour
    {
        public Action<bool, Vector3> OnTrackRecord;

        private bool isPlaying;

        private static DateTime lastEyeSignalUpdateTimeFromET = DateTime.MinValue;

        // Start is called before the first frame update
        void Start()
        {
            lastEyeSignalUpdateTimeFromET = DateTime.MinValue;
            this.isPlaying = false;
        }

        void OnGUI()
        {
            if ((CoreServices.InputSystem != null) && (CoreServices.InputSystem.EyeGazeProvider != null) &&
                    CoreServices.InputSystem.EyeGazeProvider.IsEyeTrackingEnabled &&
                    CoreServices.InputSystem.EyeGazeProvider.IsEyeTrackingDataValid && isPlaying)
            {
                if (lastEyeSignalUpdateTimeFromET != CoreServices.InputSystem?.EyeGazeProvider?.Timestamp)
                {
                    //update Direction
                    RaycastHit hit;
                    // Does the ray intersect any objects excluding the player layer
                    if (Physics.Raycast(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin, CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized, out hit, Mathf.Infinity))
                    {
                        var piece = hit.transform.gameObject.GetComponent<XRayScan.Screen.XrayReadingPiece>();
                        if (piece != null)
                        {
                            piece.ClearPiece();
                        }
                    }

                    var right = Vector3.Cross(Vector3.up, CoreServices.InputSystem.EyeGazeProvider.GazeDirection).normalized;
                    var up = Vector3.up;

                    /* 
                    //Left Upper
                    if (Physics.Raycast(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin - 0.03f * right + 0.03f * up, CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized, out hit, Mathf.Infinity))
                    {
                        var piece = hit.transform.gameObject.GetComponent<XRayScan.Screen.XrayReadingPiece>();
                        if (piece != null)
                        {
                            piece.ClearPiece();
                        }
                    }
                    */


                    //Upper
                    if (Physics.Raycast(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin + 0.03f * up, CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized, out hit, Mathf.Infinity))
                    {
                        var piece = hit.transform.gameObject.GetComponent<XRayScan.Screen.XrayReadingPiece>();
                        if (piece != null)
                        {
                            piece.ClearPiece();
                        }
                    }

                    //Right Upper
                    /*
                    if (Physics.Raycast(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin + 0.03f * right + 0.03f * up, CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized, out hit, Mathf.Infinity))
                    {
                        var piece = hit.transform.gameObject.GetComponent<XRayScan.Screen.XrayReadingPiece>();
                        if (piece != null)
                        {
                            piece.ClearPiece();
                        }
                    }
                    */

                    //Left
                    if (Physics.Raycast(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin - 0.03f * right, CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized, out hit, Mathf.Infinity))
                    {
                        var piece = hit.transform.gameObject.GetComponent<XRayScan.Screen.XrayReadingPiece>();
                        if (piece != null)
                        {
                            piece.ClearPiece();
                        }
                    }

                    //Right
                    if (Physics.Raycast(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin + 0.03f * right, CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized, out hit, Mathf.Infinity))
                    {
                        var piece = hit.transform.gameObject.GetComponent<XRayScan.Screen.XrayReadingPiece>();
                        if (piece != null)
                        {
                            piece.ClearPiece();
                        }
                    }

                    //Left Down
                    /*
                    if (Physics.Raycast(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin - 0.03f * right - 0.03f * up, CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized, out hit, Mathf.Infinity))
                    {
                        var piece = hit.transform.gameObject.GetComponent<XRayScan.Screen.XrayReadingPiece>();
                        if (piece != null)
                        {
                            piece.ClearPiece();
                        }
                    }
                    */

                    //Down
                    if (Physics.Raycast(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin - 0.03f * up, CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized, out hit, Mathf.Infinity))
                    {
                        var piece = hit.transform.gameObject.GetComponent<XRayScan.Screen.XrayReadingPiece>();
                        if (piece != null)
                        {
                            piece.ClearPiece();
                        }
                    }

                    //Right Down
                    /*
                    if (Physics.Raycast(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin + 0.03f * right - 0.03f * up, CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized, out hit, Mathf.Infinity))
                    {
                        var piece = hit.transform.gameObject.GetComponent<XRayScan.Screen.XrayReadingPiece>();
                        if (piece != null)
                        {
                            piece.ClearPiece();
                        }
                    }
                    */

                    bool hitPiece = false;
                    var hitPoint = Vector3.zero;
                    if (Physics.Raycast(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin, CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized, out hit, Mathf.Infinity))
                    {
                        var piece = hit.transform.gameObject.GetComponent<XRayScan.Screen.XrayReadingPiece>();
                        if (piece != null)
                        {
                            hitPiece = true;
                            hitPoint = hit.point;
                        }
                    }
                    this.OnTrackRecord(hitPiece, hit.point);

                    //update Timestamp
                    lastEyeSignalUpdateTimeFromET = (CoreServices.InputSystem?.EyeGazeProvider?.Timestamp).Value;
                }
                else
                {

                }
            }
            else
            {
            }

        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SwitchIsPlaying(bool enable)
        {
            this.isPlaying = enable;
        }
    }
}