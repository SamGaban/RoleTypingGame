using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SmithStore : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private Interractable interactScript;
    
    private void Start()
    {
        // Subscribe to the onInteract event
        interactScript.OnInteract += OnInteractableInteract;
    }

    // Define what should happen when the event is triggered
    private void OnInteractableInteract()
    {
        // Do something, like show a message, open a door, etc.
        Debug.Log("Interacted with object!");
    }

    void OnDestroy()
    {
        // Unsubscribe to ensure no memory leaks
        interactScript.OnInteract -= OnInteractableInteract;
    }
}
