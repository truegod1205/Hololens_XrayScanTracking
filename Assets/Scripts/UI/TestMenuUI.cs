using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TestMenuUI : MonoBehaviour
    {
        [SerializeField]
        private TestUI testUI;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitPage()
        {
            //TODO init setting value
        }


        public void MinimizeTestMenu()
        {
            this.testUI.MinimizeTestMenu();
        }

    }
}