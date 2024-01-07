using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AvailablePanel : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private GameObject _itemPrefab;

    [TabGroup("references", "References")] [SerializeField]
    private Transform parentContainer;

    [TabGroup("references", "References")] [SerializeField]
    private SwitchPanel _switchPanel;

    [TabGroup("references", "Data")] [ShowInInspector]
    private Caster caster;
    
    [TabGroup("references", "data")] [ShowInInspector]
    private List<GameObject> createdItems;

    private void Start()
    {
        createdItems = new List<GameObject>();
        caster = FindObjectOfType<Caster>();
        Display();
    }

    /// <summary>
    /// Displays available spells that are not actually equipped
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
        
        
        List<int> alreadyEquipped = new List<int>();

        foreach (KeyValuePair<int, int> entry in GameManager.Instance.slotToSpellDic)
        {
            alreadyEquipped.Add(entry.Value);
        }

        int counter = 0;

        foreach (KeyValuePair<int, string> entry in caster.spellNames)
        {
            if (!alreadyEquipped.Contains(entry.Key))
            {
                GameObject newItem = Instantiate(_itemPrefab, parentContainer);
                newItem.transform.localPosition = new Vector3(0, 1800 - (counter++ * 200), 0);

                AvailablePanelItem script = newItem.GetComponent<AvailablePanelItem>();

                if (script != null)
                {
                    script._spellLogo.sprite = caster.ReturnLogoList()[entry.Key - 1];
                    script._spellName.text = caster.spellNames[entry.Key];
                    script._equipButton.onClick.AddListener(() =>
                    {
                        SoundMaster.Instance.MenuClick();
                        _switchPanel.Display(caster.ReturnLogoList()[entry.Key - 1], caster.spellNames[entry.Key], entry.Key);
                    });
                }
                
                createdItems.Add(newItem);
            }
        }
    }
}
