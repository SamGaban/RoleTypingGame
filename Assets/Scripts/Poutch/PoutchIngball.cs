using System;
using TMPro;
using UnityEngine;

public class PoutchIngball : MonoBehaviour
{
    [SerializeField] HealthManager healthManager;
    [SerializeField] private Canvas feedBackCanvas;
    [SerializeField] private TMP_Text canvasText;

    private int hpPointOne;
    private int hpPointTwo;

    private Vector3 baseScale;
    private Vector3 reversedScale;


    
 
    private void Start()
    {
        baseScale = new Vector3(feedBackCanvas.transform.localScale.x,
            feedBackCanvas.transform.localScale.y, feedBackCanvas.transform.localScale.z);
        
        reversedScale = new Vector3(-feedBackCanvas.transform.localScale.x,
            feedBackCanvas.transform.localScale.y, feedBackCanvas.transform.localScale.z);
        
        hpPointOne = healthManager._healthPoints;
        hpPointTwo = healthManager._healthPoints;
        
        InvokeRepeating("Attribute", 1f, 1f);
    }
    
    
    /// <summary>
    /// Reverses canvas based on poutch positioning so that text is always oriented nicely;
    /// </summary>
    private void Update()
    {
        feedBackCanvas.transform.localScale = Mathf.Sign(this.transform.localScale.x) == 1 ? baseScale :
        reversedScale;
    }

    /// <summary>
    /// Passes the HP from HPManager down the pyramid for comparison hp => hpOne => hpTwo
    /// </summary>
    private void Attribute()
    {
        hpPointTwo = hpPointOne;
        hpPointOne = healthManager._healthPoints;

        if (hpPointOne < hpPointTwo)
        {
            canvasText.text = $"{hpPointTwo - hpPointOne}";
            feedBackCanvas.gameObject.SetActive(true);
            Invoke("TurnCanvasOff", 1f);
            Invoke("Heal", 1f);
        }
        

    }
    /// <summary>
    /// Heals punching ball to full life
    /// </summary>
    private void Heal()
    {
        healthManager.UpHp(1000);
    }
    /// <summary>
    /// Turns damage display canvas off
    /// </summary>
    private void TurnCanvasOff()
    {
        feedBackCanvas.gameObject.SetActive(false);
    }
}
