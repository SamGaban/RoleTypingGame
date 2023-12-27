using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
