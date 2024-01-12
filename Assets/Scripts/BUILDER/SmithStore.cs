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

    [TabGroup("references", "Sound")]
    [SerializeField]
    private AudioSource _smithingSound;

    private Player _player;


    /// <summary>
    /// Initiates the starting sequence.
    /// </summary>
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _smithingSound.Play();
        interactScript.OnInteract += OnInteractableInteract;
        interactScript.OnStopInteract += OnInteractableStop;
    }

    /// Calculates the distance between the player and the current object, and updates the volume of the _smithingSound based on the distance and the effects volume set in the game manager
    /// .
    /// The method is private and does not return any value.
    /// /
    private void Update()
    {
        float distance = Vector2.Distance(_player.transform.position, this.transform.position);

        _smithingSound.volume = GameManager.Instance.effectsVolume / (GameManager.Instance.effectsVolume + (distance));
    }

    // Define what should happen when the event is triggered
    /// <summary>
    /// Handles the interaction event for the interactable object.
    /// </summary>
    private void OnInteractableInteract()
    {
        SoundMaster.Instance.OpenPanel();
        PopulateCanvas();
        smithStoreCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// This method is called when the interaction with the object is stopped.
    /// It empties the canvas and deactivates the Smith Store canvas.
    /// </summary>
    private void OnInteractableStop()
    {
        EmptyCanvas();
        smithStoreCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Performs necessary cleanup from the event subscription before the object is destroyed.
    /// </summary>
    void OnDestroy()
    {
        // Unsubscribe to ensure no memory leaks
        interactScript.OnInteract -= OnInteractableInteract;
        interactScript.OnStopInteract -= OnInteractableStop;
    }

    /// <summary>
    /// Populates the smith store canvas with items based on the dictionary of buildables.
    /// </summary>
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
                Buildable buildable = entry.Value.GetComponent<Buildable>();
                script.logo.sprite = entry.Value.GetComponent<SpriteRenderer>().sprite;
                script.priceText.text = buildable.goldValue.ToString();
                
                script.button.onClick.AddListener(() =>
                {
                    SoundMaster.Instance.MenuClick();

                    if (GameManager.Instance.SubstractGold(buildable.goldValue))
                    {
                        HUD hud = FindObjectOfType<HUD>();
                        hud.UpdateGoldCount();
                        GameManager.Instance.AddToBuildables(entry.Key);
                    }
                });

            }
            
            counter++;
        }
    }

    /// <summary>
    /// Empties the canvas by destroying all child game objects.
    /// </summary>
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
