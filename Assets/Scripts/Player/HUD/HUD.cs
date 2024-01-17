using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class HUD : MonoBehaviour
{
    #region References

    [TabGroup("references", "script references")] [SerializeField]
    private Caster casterScript;
    
    [TabGroup("references", "references")] [SerializeField]
    private List<Image> logoSlotList;

    [TabGroup("references", "references")] [SerializeField]
    private List<GameObject> omenLogos;

    [TabGroup("references", "references")] [SerializeField]
    private GameSession gameSession;

    [TabGroup("references", "references")] [SerializeField]
    private TMP_Text goldText;

    [TabGroup("references", "references")] [SerializeField]
    private TMP_Text streakText;
    
    [TabGroup("references", "data")] [ShowInInspector]
    private List<Sprite> logoList;


    [TabGroup("references", "data")] [ShowInInspector]
    private int omenCount = 0;

    public int OmenCount()
    {
        return omenCount;
    }
    
    #endregion

    /// <summary>
    /// Gets info from the lists of logo and slot dictionaries in the cast
    /// </summary>
    private void Feed()
    {
        logoList = casterScript.ReturnLogoList();
    }

    /// <summary>
    /// Calls individualupdate() for each of the logos
    /// </summary>
    private void UpdateLogos()
    {
        IndividualUpdate(1);
        IndividualUpdate(2);
        IndividualUpdate(3);
        IndividualUpdate(4);
        IndividualUpdate(5);
        IndividualUpdate(6);
        IndividualUpdate(7);
        IndividualUpdate(8);
    }

    /// <summary>
    /// Whole sequence of update
    /// </summary>
    public void WholeUpdate()
    {
        Feed();
        UpdateLogos();
        foreach (GameObject omenLogo in omenLogos)
        {
            omenLogo.SetActive(false);
        }
        
        UpdateGoldCount();
        
        Invoke(nameof(UpdateOmenLogos), 2f);
        
    }

    public void UpdateGoldCount()
    {
        goldText.text = GameManager.Instance.PlayerGold.ToString();
        
        string formattedNumber = GameManager.Instance.streakModifier.ToString("F2");

        streakText.text = $"+{formattedNumber}%";
    }

    public void UpdateOmenLogos()
    {
        if (gameSession.OmenCount > 0)
        {
            for (int i = 0; i < gameSession.OmenCount; i++)
            {
                omenLogos[i].SetActive(true);
                omenCount++;
            }            
        }
    }

    public void MinusOneOmen()
    {
        omenCount--;
        omenLogos[omenCount].SetActive(false);
    }

    /// <summary>
    /// Looks if a slot has an assigned spell, and if so, puts the logo of said spell onto the slot
    /// </summary>
    private void IndividualUpdate(int index)
    {
        if (GameManager.Instance.slotToSpellDic.ContainsKey(index))
        {
            logoSlotList[index - 1].enabled = true;
            logoSlotList[index - 1].sprite = logoList[GameManager.Instance.slotToSpellDic[index] - 1];
        }
        else
        {
            logoSlotList[index - 1].enabled = false;
        }
    }
    
    private void Start()
    {
        logoList = new List<Sprite>();

        Invoke("WholeUpdate", 1.5f);
    }
    
}
