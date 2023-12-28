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
    
    private void Start()
    {
        interactScript.OnInteract += OnInteract;
        interactScript.OnStopInteract += OnStopInteract;
    }

    private void OnInteract()
    {
        questBoardPanel.gameObject.SetActive(true);
    }

    private void OnStopInteract()
    {
        questBoardPanel.gameObject.SetActive(false);
    }
    
}
