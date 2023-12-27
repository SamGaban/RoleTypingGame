using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SmithStore : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private Interractable interactScript;

    [TabGroup("references", "References")] [SerializeField]
    private Canvas smithStoreCanvas;

    [TabGroup("references", "References")] [SerializeField]
    private Transform parentContainer;

    [TabGroup("references", "References")] [SerializeField]
    private GameObject smithStoreItemPrefab;
    
    private void Start()
    {
        interactScript.OnInteract += OnInteractableInteract;
        interactScript.OnStopInteract += OnInteractableStop;
    }

    // Define what should happen when the event is triggered
    private void OnInteractableInteract()
    {
        PopulateCanvas();
        smithStoreCanvas.gameObject.SetActive(true);
    }

    private void OnInteractableStop()
    {
        EmptyCanvas();
        smithStoreCanvas.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // Unsubscribe to ensure no memory leaks
        interactScript.OnInteract -= OnInteractableInteract;
        interactScript.OnStopInteract -= OnInteractableStop;
    }

    private void PopulateCanvas()
    {
        int counter = 0;
        
        foreach (KeyValuePair<int, GameObject> entry in  BuildDico.Instance.dico)
        {
            GameObject newItem = Instantiate(smithStoreItemPrefab, parentContainer);
            newItem.transform.localPosition = new Vector3(-183.2f, 3604.1f - (150 * counter), 0f);
            SmithStoreItem script = newItem.GetComponent<SmithStoreItem>();

            if (script != null)
            {
                script.logo.sprite = entry.Value.GetComponent<SpriteRenderer>().sprite;
                script.priceText.text = entry.Value.GetComponent<Buildable>().goldValue.ToString();
            }
            
            counter++;
        }
    }

    private void EmptyCanvas()
    {
        // Collect all children first
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parentContainer.transform)
        {
            children.Add(child.gameObject);
        }

        // Destroy them
        foreach (GameObject child in children)
        {
            Destroy(child);
        }
    }
}
