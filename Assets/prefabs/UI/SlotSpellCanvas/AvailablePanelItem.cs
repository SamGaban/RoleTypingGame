using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvailablePanelItem : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    public TMP_Text _spellName;

    [TabGroup("references", "References")] [SerializeField]
    public Image _spellLogo;

    [TabGroup("references", "References")] [SerializeField]
    public Button _equipButton;
    
    [TabGroup("references", "References")] [SerializeField]
    public TMP_Text _equipButtonText;

    [TabGroup("references", "References")] [SerializeField]
    private Sprite _notBoughtSprite;

    [TabGroup("references", "References")] [SerializeField]
    private Image _omenImage;


    public void ChangeImageOfButtonNotBought()
    {
        _equipButton.image.sprite = _notBoughtSprite;
        _equipButtonText.color = Color.white;
        _omenImage.gameObject.SetActive(true);
    }
}
