using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private Interractable interactScript;

    [TabGroup("references", "References")] [SerializeField]
    private Canvas questBoardPanel;

    /// <summary>
    /// This method is called to start the interaction script.
    /// <para>Subscribes to the interact event</para>
    /// </summary>
    private void Start()
    {
        interactScript.OnInteract += OnInteract;
        interactScript.OnStopInteract += OnStopInteract;
    }

    /// <summary>
    /// Called when the object is interacted with.
    /// </summary>
    private void OnInteract()
    {
        SoundMaster.Instance.OpenPanel();
        questBoardPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// Disables the Quest Board panel when the interaction is stopped.
    /// </summary>
    private void OnStopInteract()
    {
        questBoardPanel.gameObject.SetActive(false);
    }
    
}
