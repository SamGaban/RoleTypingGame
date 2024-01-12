using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Script to attach to a gameobject supposed to be interactive, attaches an event to it, trigger by pressing a key
/// <para>Also displays a prompt on screen that the interaction is possible
/// <para>Link desired interactions to actions using the provided methods "OnInteract" "OnStopInteract"
/// </summary>
public class Interractable : MonoBehaviour
{
    public string interactName;
    public event Action OnInteract;
    public event Action OnStopInteract;

    private void Start()
    {
    }

    /// <summary>
    /// Called when a Collider2D enters a trigger.
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
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

    /// <summary>
    /// Turns the interract canvas off
    /// </summary>
    /// <param name="other"></param>
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

    /// <summary>
    /// Calls the <see cref="OnInteract"/> event.
    /// </summary>
    public void Interact()
    {
        OnInteract?.Invoke();
    }

    /// <summary>
    /// Stops the interaction process.
    /// </summary>
    public void StopInteract()
    {
        OnStopInteract?.Invoke();
    }
}
