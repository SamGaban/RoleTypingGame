using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script linked to equipped panel item prefab
/// </summary>
public class EquippedPanelItem : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    public Image _spellLogo;

    [TabGroup("references", "References")] [SerializeField]
    public TMP_Text _spellName;

    [TabGroup("references", "References")] [SerializeField]
    public Button _removeButton;

    [TabGroup("references", "References")] [SerializeField]
    public Button _switchButton;

}
