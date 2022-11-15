using System;
using System.Collections.Generic;
using UnityEngine;


namespace XRayScan.Screen
{
    public class XrayPiece : MonoBehaviour
    {
        public Action<XrayPiece> OnCompletePiece;

        private Material renderMat;

        private bool isScanned = false;

        public int HIndex {get; private set;}
        public int VIndex { get; private set; }

        public void Init(Material mat, int hIndex, int vIndex)
        {
            this.renderMat = mat;
            this.isScanned = false;
            this.HIndex = hIndex;
            this.VIndex = vIndex;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ClearPiece()
        {
            if (!this.isScanned)
            {
                this.renderMat.color = new Color(this.renderMat.color.r, this.renderMat.color.g, this.renderMat.color.b, 1);
                this.OnCompletePiece?.Invoke(this);
                this.isScanned = true;
            }
        }

        public void ResetPiece()
        {
            this.renderMat.color = new Color(this.renderMat.color.r, this.renderMat.color.g, this.renderMat.color.b, 0);
            this.isScanned = false;
        }
    }
}