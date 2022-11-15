using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoveAway : MonoBehaviour
{
    private bool isStare = false;

    [SerializeField]
    private float triggerTime = 1f;

    [SerializeField]
    private Image progressImage;

    private float stareTime = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStare)
        {
            this.stareTime += Time.deltaTime;
            if (this.stareTime > triggerTime)
            {
                this.stareTime = triggerTime;
            }
            this.progressImage.fillAmount = this.stareTime / triggerTime;
        }
    }

    public void LookStart()
    {
        this.isStare = true;
        this.stareTime = 0f;
        progressImage.fillAmount = 0f;
    }

    public void LookAway()
    {
        this.isStare = false;
        this.stareTime = 0f;
        progressImage.fillAmount = 0f;
    }
}
