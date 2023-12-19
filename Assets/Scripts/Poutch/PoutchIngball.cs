using TMPro;
using UnityEngine;

public class PoutchIngball : MonoBehaviour
{
    [SerializeField] HealthManager healthManager;
    [SerializeField] private Canvas feedBackCanvas;
    [SerializeField] private TMP_Text canvasText;

    private int hpPointOne;
    private int hpPointTwo;


    private void Start()
    {
        hpPointOne = healthManager._healthPoints;
        hpPointTwo = healthManager._healthPoints;
        
        InvokeRepeating("Attribute", 1f, 1f);
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
