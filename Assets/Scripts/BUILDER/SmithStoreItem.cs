using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmithStoreItem : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    public Image logo;

    [TabGroup("references", "References")] [SerializeField]
    public TMP_Text priceText;

    [TabGroup("references", "References")] [SerializeField]
    public Button button;
}
