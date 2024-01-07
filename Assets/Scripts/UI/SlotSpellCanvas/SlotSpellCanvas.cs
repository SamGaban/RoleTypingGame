using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Interact script for the spell book
/// </summary>
public class SlotSpellCanvas : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private Interractable interactScript;

    [TabGroup("references", "References")] [SerializeField]
    private Canvas _spellBookCanvas;
    
    private void Start()
    {
        interactScript.OnInteract += OnInteract;
        interactScript.OnStopInteract += OnStopInteract;
    }

    private void OnInteract()
    {
        SoundMaster.Instance.OpenPanel();
        _spellBookCanvas.gameObject.SetActive(true);
    }
    
    private void OnStopInteract()
    {
        _spellBookCanvas.gameObject.SetActive(false);
    }
}
