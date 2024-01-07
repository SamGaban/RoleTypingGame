using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script linked to the whole equipped panel
/// </summary>
public class EquippedPanel : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private List<GameObject> _slots;
    

    [TabGroup("references", "References")] [SerializeField]
    private GameObject _itemPrefab;

    [TabGroup("references", "References")] [SerializeField]
    private Button _equipSpellButton;

    [TabGroup("references", "References")] [SerializeField]
    private SwitchPanel _switchPanel;

    [TabGroup("references", "References")] [SerializeField]
    private AvailablePanel _availablePanel;

    [TabGroup("references", "data")] [ShowInInspector]
    private Caster caster;

    /// <summary>
    /// Created panel item put in a list, for deletion upon calling the method to get new ones
    /// </summary>
    [TabGroup("references", "data")] [ShowInInspector]
    private List<GameObject> createdItems;

    [TabGroup("references", "data")] [ShowInInspector]
    private List<GameObject> createdButtons;

    [TabGroup("references", "data")] [ShowInInspector]
    private HUD _hud;

    private void Start()
    {
        _hud = FindObjectOfType<HUD>();
        createdButtons = new List<GameObject>();
        createdItems = new List<GameObject>();
        caster = FindObjectOfType<Caster>();
        Display();
    }

    /// <summary>
    /// Method used for refreshing the equipped panel items and display them
    /// </summary>
    public void Display()
    {

        if (createdItems.Count > 0)
        {
            while (createdItems.Count > 0)
            {
                Destroy(createdItems[0]);
                createdItems.RemoveAt(0);
            }
        }
        
        foreach (KeyValuePair<int, int> entry in GameManager.Instance.slotToSpellDic)
        {
            GameObject panelItem = Instantiate(_itemPrefab, _slots[entry.Key - 1].transform);

            EquippedPanelItem script = panelItem.GetComponent<EquippedPanelItem>();

            if (script != null)
            {
                script._spellLogo.sprite = caster.ReturnLogoList()[entry.Value - 1];
                script._spellName.text = caster.spellNames[entry.Value];
                
                script._removeButton.onClick.AddListener(() => // Button to remove an equipped ability
                {
                    SoundMaster.Instance.MenuClick();

                    GameManager.Instance.slotToSpellDic.Remove(entry.Key);
                    Display();
                    _availablePanel.Display();
                    _hud.WholeUpdate();
                });
                
                script._switchButton.onClick.AddListener(() => // Button to switch an equipped ability
                {
                    SoundMaster.Instance.MenuClick();

                    GameManager.Instance.slotToSpellDic.Remove(entry.Key);
                    
                    _switchPanel.Display(caster.ReturnLogoList()[entry.Value - 1], caster.spellNames[entry.Value], entry.Value);
                    
                    Display();
                    _availablePanel.Display();
                    _hud.WholeUpdate();
                });
                
            }
            
            createdItems.Add(panelItem);
        }
    }

    /// <summary>
    /// Displays the "Equip" button on all slots that do not have an assigned ability
    /// </summary>
    public void ShowFreeSlots()
    {
        HideFreeSlots();
        List<int> allSlots = new List<int>()
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8
        };
        
        foreach (KeyValuePair<int, int> entry in GameManager.Instance.slotToSpellDic)
        {
            allSlots.Remove(entry.Key);
        }

        foreach (int entry in allSlots)
        {
            Button newButton = Instantiate(_equipSpellButton, _slots[entry - 1].transform);
            newButton.onClick.AddListener(() =>
            {
                SoundMaster.Instance.MenuClick();

                GameManager.Instance.slotToSpellDic.Add(entry, _switchPanel.currentID);
                HideFreeSlots();
                _switchPanel.Hide();
                _availablePanel.Display();
                _hud.WholeUpdate();
                Display();
            });
            
            createdButtons.Add(newButton.gameObject);
        }
    }

    /// <summary>
    /// Hides/deletes the "Equip" buttons, to be used when a skill has been asigned just now
    /// </summary>
    public void HideFreeSlots()
    {
        if (createdButtons.Count > 0)
        {
            while (createdButtons.Count > 0)
            {
                Destroy(createdButtons[0]);
                createdButtons.RemoveAt(0);
            }
        }
    }
}
