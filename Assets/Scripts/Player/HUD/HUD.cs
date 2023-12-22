using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    [TabGroup("references", "data")] [ShowInInspector]
    private List<Sprite> logoList;

    [TabGroup("references", "data")] [ShowInInspector]
    private Dictionary<int, int> SlotSpellDictionary;
    
    

    #endregion

    /// <summary>
    /// Gets info from the lists of logo and slot dictionaries in the cast
    /// </summary>
    private void Feed()
    {
        logoList = casterScript.ReturnLogoList();
        SlotSpellDictionary = casterScript.ReturnSlotDictionary();
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
    private void WholeUpdate()
    {
        Feed();
        UpdateLogos();
    }

    /// <summary>
    /// Looks if a slot has an assigned spell, and if so, puts the logo of said spell onto the slot
    /// </summary>
    private void IndividualUpdate(int index)
    {
        if (SlotSpellDictionary.ContainsKey(index))
        {
            logoSlotList[index - 1].enabled = true;
            logoSlotList[index - 1].sprite = logoList[SlotSpellDictionary[index] - 1];
        }
        else
        {
            logoSlotList[index - 1].enabled = false;
        }
    }
    
    private void Start()
    {
        logoList = new List<Sprite>();
        SlotSpellDictionary = new Dictionary<int, int>();
        
        Invoke("WholeUpdate", 1f);
    }
}
