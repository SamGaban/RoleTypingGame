using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelItem : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    public Image itemImage;

    [TabGroup("references", "References")] [SerializeField]
    public TMP_Text amount;

    [TabGroup("references", "References")] [SerializeField]
    public Button button;
    
}
