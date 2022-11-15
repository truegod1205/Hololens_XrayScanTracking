using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.Screen
{
    public class FindScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject startButton;

        [SerializeField]
        private GameObject distanceText;

        [SerializeField]
        private TextMesh distanceTextMesh;

        [SerializeField]
        private Transform gridRoot;

        [SerializeField]
        private GameObject gridLineSample;

        private List<GameObject> gridCorners;

        

        // Start is called before the first frame update
        void Start()
        {
            gridCorners = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.distanceText.activeInHierarchy)
            {
                var center = Vector3.zero;
                for (int i = 0; i < 4; i++)
                {
                    center += this.gridCorners[i].transform.position;
                }
                this.distanceTextMesh.text = ((int)(Vector3.Distance(Camera.main.transform.position, center/4f) * 100f)).ToString();
            }
        }

        internal void AddGridCorner(GameObject obj)
        {
            gridCorners.Add(obj);

            if (gridCorners.Count >= 4)
            {
                this.startButton.SetActive(true);
                this.distanceText.SetActive(true);
            }
        }

        public void RenderGrid()
        {
            this.StartCoroutine(this.IterRenderGrid());
        }

        public IEnumerator IterRenderGrid()
        {
            var distance = float.MinValue;
            var index = 0;
            for (int i =1; i < 4; i++)
            {
                var dis = Vector3.Distance(this.gridCorners[0].transform.position, this.gridCorners[i].transform.position);
                if (dis > distance)
                {
                    index = i;
                }
            }

            var farObj = this.gridCorners[index];
            this.gridCorners.RemoveAt(index);
            this.gridCorners.Insert(2, farObj);


            for (int i = 1; i < 10; i++)
            {
                var lPoint = this.gridCorners[0].transform.position + (i / 10f) * (this.gridCorners[3].transform.position - this.gridCorners[0].transform.position);
                var rPoint = this.gridCorners[1].transform.position + (i / 10f) * (this.gridCorners[2].transform.position - this.gridCorners[1].transform.position);

                var line = Instantiate(gridLineSample, gridRoot);
                var lineRenderer = line.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, lPoint);
                lineRenderer.SetPosition(1, rPoint);
                line.gameObject.SetActive(true);
                yield return null;
            }

            for (int i = 1; i < 10; i++)
            {
                var tPoint = this.gridCorners[0].transform.position + (i / 10f) * (this.gridCorners[1].transform.position - this.gridCorners[0].transform.position);
                var bPoint = this.gridCorners[3].transform.position + (i / 10f) * (this.gridCorners[2].transform.position - this.gridCorners[3].transform.position);

                var line = Instantiate(gridLineSample, gridRoot);
                var lineRenderer = line.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, tPoint);
                lineRenderer.SetPosition(1, bPoint);
                line.gameObject.SetActive(true);
                yield return null;
            }
        }
    }
}