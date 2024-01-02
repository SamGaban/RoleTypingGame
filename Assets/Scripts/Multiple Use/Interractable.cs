using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to attach to a gameobject supposed to be interactive, attaches an event to it, trigger by pressing a key
/// <para>Also displays a prompt on screen that the interaction is possible
/// <para>Link desired interactions to actions using the provided methods "OnInteract" "OnStopInteract"
/// </summary>
public class Interractable : MonoBehaviour
{
    public event Action OnInteract;
    public event Action OnStopInteract;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameSession script = FindObjectOfType<GameSession>();
            if (script != null)
            {
                script.SetInterractable(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameSession script = FindObjectOfType<GameSession>();
            if (script != null)
            {
                StopInteract();
                script.ForgetInterractable();
            }
        }
    }

    public void Interact()
    {
        OnInteract?.Invoke();
    }

    public void StopInteract()
    {
        OnStopInteract?.Invoke();
    }
}
