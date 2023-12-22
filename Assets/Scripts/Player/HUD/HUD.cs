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

    private void Feed()
    {
        logoList = casterScript.ReturnLogoList();
        SlotSpellDictionary = casterScript.ReturnSlotDictionary();
    }

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

    private void WholeUpdate()
    {
        Feed();
        UpdateLogos();
    }

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
