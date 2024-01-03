using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SwitchPanel : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private GameObject _itemPrefab;

    [TabGroup("references", "References")] [SerializeField]
    private Transform _parentContainer;


    [TabGroup("references", "References")] [SerializeField]
    private EquippedPanel _equippedPanel;

    [TabGroup("references", "References")] [SerializeField]
    private AvailablePanel _availablePanel;
    

    [TabGroup("references", "data")] [ShowInInspector]
    private GameObject currentlyDisplayed;

    [TabGroup("references", "data")] [ShowInInspector]
    public int currentID;

    /// <summary>
    /// Displays the desired spell in the switch panel
    /// </summary>
    /// <param name="logo">Logo of spell</param>
    /// <param name="name">Name of spell</param>
    /// <param name="spellID">Spell ID</param>
    public void Display(Sprite logo, string name, int spellID)
    {
        Hide();

        GameObject newItem = Instantiate(_itemPrefab, _parentContainer);

        SwitchPanelItem script = newItem.GetComponent<SwitchPanelItem>();

        if (script != null)
        {
            script._spellName.text = name;
            script._spellLogo.sprite = logo;
            currentID = spellID;
            _equippedPanel.ShowFreeSlots();
        }

        currentlyDisplayed = newItem;

    }

    public void Hide()
    {
        if (currentlyDisplayed != null)
        {
            Destroy(currentlyDisplayed);
            currentlyDisplayed = null;
        }
    }
}
