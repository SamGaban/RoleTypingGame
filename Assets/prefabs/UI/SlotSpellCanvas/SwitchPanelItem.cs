using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanelItem : MonoBehaviour
{
    [TabGroup("references", "References")] public Image _spellLogo;

    [TabGroup("references", "References")] public TMP_Text _spellName;
}
