using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Spell info to be displayed in the UI on the left of the screen
/// </summary>
public class SpellEffectsItem : MonoBehaviour
{
    [TabGroup("references", "References")]
    [SerializeField]
    public Image logo;

    [TabGroup("references", "References")]
    [SerializeField]
    public TMP_Text timeText;

    [TabGroup("references", "Data")]
    [SerializeField]
    private float _endOfSpell;

    /// <summary>
    /// Sends information for the creation of a new spell logo countdown
    /// </summary>
    /// <param name="time">Time of the spell</param>
    /// <param name="newLogo">Logo of spell</param>
    public void Initialize(float time, Sprite newLogo)
    {
        _endOfSpell = Time.time + time;
        logo.sprite = newLogo;
    }

    /// <summary>
    /// Updates the display text
    /// </summary>
    private void Update()
    {
        timeText.text = string.Format("{0:00.00}", (_endOfSpell - Time.time));
    }
}
