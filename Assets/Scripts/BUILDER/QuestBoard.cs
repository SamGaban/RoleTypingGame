using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private Interractable interactScript;
    
    private void Start()
    {
        interactScript.OnInteract += OnInteract;
        interactScript.OnStopInteract += OnStopInteract;
    }

    private void OnInteract()
    {
        Debug.Log("Questboard interact");
    }

    private void OnStopInteract()
    {
        Debug.Log("Questboard stop interact");
    }
    
}
