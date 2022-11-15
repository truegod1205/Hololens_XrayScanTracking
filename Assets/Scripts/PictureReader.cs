using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureReader : MonoBehaviour
{
    [SerializeField]
    private Image[] images;

    private int currentIndex = 0;

    private int CurrentIndex
    {
        get 
        {
            return currentIndex;
        }
        set
        {
            currentIndex = value;
            this.ShowImage();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.CurrentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowImage()
    {
        if (this.currentIndex >= 0 && this.currentIndex < this.images.Length)
        {
            for (int i = 0; i < this.images.Length; i++)
            {
                if (i == this.currentIndex)
                {
                    this.images[i].gameObject.SetActive(true);
                }
                else
                {
                    this.images[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void MovePageIndex(int moveOffset)
    {
        var change = this.CurrentIndex + moveOffset;
        if (change >= this.images.Length)
        {
            this.CurrentIndex = this.images.Length - 1;
        }
        else if (change < 0)
        {
            this.CurrentIndex = 0;
        }
        else
        {
            this.CurrentIndex = change;
        }
    }
}
