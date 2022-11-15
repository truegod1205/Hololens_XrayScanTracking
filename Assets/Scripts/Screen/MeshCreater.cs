using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.Screen
{

    public class MeshCreater : MonoBehaviour
    {
        private List<GameObject> screenCorners;

        private List<GameObject> screenGrids;

        private List<XrayPiece> completeXrayPieces;

        [SerializeField]
        private Transform gridRoot;

        [SerializeField]
        private Material gridMat;

        [SerializeField]
        private Ray.ScanTimer timer;

        [SerializeField]
        private Ray.RayTest ray;

        [SerializeField]
        private TestScreen testScreen;

        public Action OnClickStartTest;

        public Action OnBackToWait;

        public Action OnClickRescanQRCode;

        private Log.TestXrayLog currentTestXrayLog = null;

        protected void Awake()
        {
            this.screenCorners = new List<GameObject>();
            this.screenGrids = new List<GameObject>();
            this.completeXrayPieces = new List<XrayPiece>();
            this.ray.OnTrackRecord += this.RecordTrack;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddGridCorner(Vector3 position, Quaternion pose)
        {
            this.testScreen.AddGridCorner(position, pose);
        }

        public void InitScreen()
        {
            this.testScreen.OnClickRescan += this.OnClickRescan;
            this.testScreen.OnClickStart += this.OnClickStart;
            this.testScreen.OnClickRestart += this.OnClickRestart;
            this.timer.OnCountDown += this.PlayTimesUp;
            this.timer.ResetTimer();
            this.timer.SwitchCountDown(false);
        }

        public void DeInitScreen()
        {
            this.DestroyScreen();
            this.ray.SwitchIsPlaying(false);
            this.testScreen.OnClickRescan -= this.OnClickRescan;
            this.testScreen.OnClickStart -= this.OnClickStart;
            this.testScreen.OnClickRestart -= this.OnClickRestart;
            this.timer.OnCountDown -= this.PlayTimesUp;
            if (this.currentTestXrayLog != null)
            {
                this.currentTestXrayLog.AddResult(Log.TestXrayLog.TestModeResultType.Interrupt);
                this.StartCoroutine(this.ProcessCurrentXrayLog());
            }
        }

        private IEnumerator CreateMeshs()
        {
            var corners = this.testScreen.GetCorners();
            if (corners.Count != 4)
            {
                Debug.LogError("wrong screen count.");
            }

            this.screenCorners.Clear();
            for (int i = 0; i < corners.Count; i++)
            {
                this.screenCorners.Add(corners[i]);
            }

            //decide every corner

            List<Vector3> gridPts = new List<Vector3>();
            this.DecideGridPoint(ref gridPts, 
                this.screenCorners[0].transform.position, this.screenCorners[1].transform.position,
                this.screenCorners[3].transform.position, this.screenCorners[2].transform.position);
            yield return null;

            this.CreateGrids(gridPts);
            yield return null;
        }

        private void DecideGridPoint(ref List<Vector3> gridPts, Vector3 LT, Vector3 RT, Vector3 LB, Vector3 RB)
        {
            var vPartitions = Config.ConfigManager.Instance.Config.verticalPartition;
            var hPartitions = Config.ConfigManager.Instance.Config.horizontalPartition;

            for (int i = 0; i <= vPartitions; i++)
            {
                var lPt = LT + (LB - LT) * ((float)i / (float)vPartitions);
                var rPt = RT + (RB - RT) * ((float)i / (float)vPartitions);
                for (int j = 0; j <= hPartitions; j++)
                {
                    gridPts.Add(lPt + (rPt - lPt) * ((float)j / (float)hPartitions));
                }
            }
        }

        private void CreateGrids(List<Vector3> gridPts)
        {
            this.screenGrids.Clear();
            var vPartitions = Config.ConfigManager.Instance.Config.verticalPartition;
            var hPartitions = Config.ConfigManager.Instance.Config.horizontalPartition;

            for (int i = 0; i < vPartitions; i++)
            {
                for (int j = 0; j < hPartitions; j++)
                {
                    var meshQuad = new GameObject();
                    meshQuad.transform.SetParent(gridRoot);
                    var filter = meshQuad.AddComponent<MeshFilter>();
                    var renderer = meshQuad.AddComponent<MeshRenderer>();
                    var mesh = filter.mesh;
                    var collider = meshQuad.AddComponent<MeshCollider>();

                    mesh.Clear();
                    mesh.vertices = new Vector3[] { 
                        gridPts[(j) + (i)*(hPartitions + 1)],
                        gridPts[(j+1) + (i)*(hPartitions + 1)],
                        gridPts[(j) + (i+1)*(hPartitions + 1)],
                        gridPts[(j+1) + (i+1)*(hPartitions + 1)],
                    };
                    mesh.uv = new Vector2[] {
                        new Vector2(0, 0), 
                        new Vector2(1, 0), 
                        new Vector2(0, 1),
                        new Vector2(1, 1),
                    };
                    mesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
                    renderer.material = new Material(this.gridMat);
                    collider.convex = true;
                    meshQuad.AddComponent<XrayPiece>().Init(renderer.material, j, i);
                    meshQuad.GetComponent<XrayPiece>().OnCompletePiece += this.CompletePiece;
                    this.screenGrids.Add(meshQuad);
                }
            }
        }

        private void DestroyGrids()
        {
            for (int i = this.screenGrids.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(this.screenGrids[i]);
            }
            this.screenGrids.Clear();
        }

        private void DestroyScreen()
        {
            this.DestroyGrids();
            this.screenCorners.Clear();
            this.testScreen.DestroyRectangle();
        }

        private void InitEveryPieces()
        {
            for (int i = 0; i < this.screenGrids.Count; i++)
            {
                var piece = this.screenGrids[i].GetComponent<XrayPiece>();
                if (piece != null)
                {
                    piece.ResetPiece();
                }
                var collider = this.screenGrids[i].GetComponent<MeshCollider>();
                if (collider != null)
                {
                    collider.enabled = true;
                }
            }
        }

        public void OnClickStart()
        {
            this.SwitchStartButton(false);
            this.SwitchRescanButton(false);
            this.StartCoroutine(this.PlayCoroutine());
        }

        private IEnumerator PlayCoroutine()
        {
            yield return this.ProcessCurrentXrayLog();
            this.currentTestXrayLog = new Log.TestXrayLog();

            this.DestroyGrids();
            this.screenCorners.Clear();
            yield return this.StartCoroutine(this.CreateMeshs());
            this.currentTestXrayLog.AddCornerPosition(this.screenCorners[0].transform.position, 
                this.screenCorners[1].transform.position,
                this.screenCorners[3].transform.position,
                this.screenCorners[2].transform.position);
            this.completeXrayPieces.Clear();
            this.InitEveryPieces();
            this.timer.ResetTimer();
            this.timer.SwitchCountDown(true);
            this.ray.SwitchIsPlaying(true);
            this.OnClickStartTest?.Invoke();

            yield return new WaitForSeconds(0.5f);
            this.SwitchRestartButton(true);
        }

        private void PlayFinished()
        {
            this.timer.SwitchCountDown(false);
            this.ray.SwitchIsPlaying(false);
            this.SwitchRescanButton(false);
            this.SwitchRestartButton(false);
            this.StartCoroutine(this.PlayFinishedCoroutine());
        }

        private IEnumerator PlayFinishedCoroutine()
        {
            if (this.currentTestXrayLog != null)
            {
                this.currentTestXrayLog.AddResult(Log.TestXrayLog.TestModeResultType.Success);
                yield return this.StartCoroutine(this.ProcessCurrentXrayLog());
            }

            this.OnBackToWait?.Invoke();
        }

        private void PlayTimesUp()
        {
            this.timer.SwitchCountDown(false);
            this.ray.SwitchIsPlaying(false);
            this.SwitchRescanButton(false);
            this.SwitchRestartButton(false);
            this.StartCoroutine(this.PlayTimesUpCoroutine());
        }

        private IEnumerator PlayTimesUpCoroutine()
        {
            if (this.currentTestXrayLog != null)
            {
                this.currentTestXrayLog.AddResult(Log.TestXrayLog.TestModeResultType.TimesUp);
                yield return this.StartCoroutine(this.ProcessCurrentXrayLog());
            }
            
            this.OnBackToWait?.Invoke();
        }

        public void OnClickRestart()
        {
            this.DestroyGrids();
            this.screenCorners.Clear();
            this.timer.SwitchCountDown(false);
            this.timer.ResetTimer();
            this.ray.SwitchIsPlaying(false);
            this.SwitchRestartButton(false);
            this.SwitchRescanButton(false);
            this.StartCoroutine(this.OnClickRestartCoroutine());
        }

        private IEnumerator OnClickRestartCoroutine()
        {
            if (this.currentTestXrayLog != null)
            {
                this.currentTestXrayLog.AddResult(Log.TestXrayLog.TestModeResultType.Interrupt);
                yield return this.StartCoroutine(this.ProcessCurrentXrayLog());
            }

            this.OnBackToWait?.Invoke();
        }

        public void OnClickRescan()
        {
            this.timer.SwitchCountDown(false);
            this.DeInitScreen();
            this.SwitchStartButton(false);
            this.SwitchRestartButton(false);
            this.SwitchRescanButton(false);
            this.StartCoroutine(this.OnClickRescantCoroutine());
        }

        private IEnumerator OnClickRescantCoroutine()
        {
            if (this.currentTestXrayLog != null)
            {
                this.currentTestXrayLog.AddResult(Log.TestXrayLog.TestModeResultType.Interrupt);
                yield return this.StartCoroutine(this.ProcessCurrentXrayLog());
            }

            this.OnClickRescanQRCode?.Invoke();
        }

        public void CompletePiece(XrayPiece piece)
        {
            if (this.currentTestXrayLog != null)
            {
                this.currentTestXrayLog.AddCompleteGrid(piece.HIndex, piece.VIndex);
            }
            this.completeXrayPieces.Add(piece);
            if (this.completeXrayPieces.Count >= this.screenGrids.Count)
            {
                this.PlayFinished();
            }
        }

        public void RegisterScreenSetupFinished(Action callback)
        {
            this.testScreen.ScreenSetupFinished += callback;
        }

        public void UnregisterScreenSetupFinished(Action callback)
        {
            this.testScreen.ScreenSetupFinished -= callback;
        }

        public void SwitchRescanButton(bool enable)
        {
            this.testScreen.SwitchRescanButton(enable);
        }

        public void SwitchStartButton(bool enable)
        {
            this.testScreen.SwitchStartButton(enable);
        }

        public void SwitchRestartButton(bool enable)
        {
            this.testScreen.SwitchRestartButton(enable);
        }

        public void SwitchTimer(bool enable)
        {
            this.testScreen.SwitchTimer(enable);
        }

        public void SwitchDistance(bool enable)
        {
            this.testScreen.SwitchDistance(enable);
        }

        public IEnumerator ProcessCurrentXrayLog()
        {
            while (this.currentTestXrayLog != null)
            {
                var writeResult = Log.XrayLogSystem.Instance.WriteXRayLog(this.currentTestXrayLog);
                if (writeResult)
                {
                    this.currentTestXrayLog = null;
                    break;
                }
                yield return null;
            }
        }

        private void RecordTrack(bool isLegal, Vector3 hitPoint)
        {
            this.currentTestXrayLog.AddTrackData(isLegal, hitPoint);
        }
    }
}