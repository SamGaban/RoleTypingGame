using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;


/// <summary>
/// Class serving as direct information about the game session, be it town mode or game mode
/// </summary>
public class GameSession : MonoBehaviour
{

    
    
    [TabGroup("references", "Data")] [ShowInInspector]
    private int omenCount = 0;

    [TabGroup("references", "references")] [SerializeField]
    private HUD hud;

    [TabGroup("references", "references")] [SerializeField]
    private Transform playerTransform;

    [TabGroup("references", "references")] [SerializeField]
    private Player player;

    [TabGroup("references", "references")] [SerializeField]
    private Canvas buildPanel;

    [TabGroup("references", "references")] [SerializeField]
    private CinemachineVirtualCamera mainCamera;

    [TabGroup("references", "references")] [SerializeField]
    private GameObject panelItemPrefab;

    [TabGroup("references", "references")] [SerializeField]
    private GameObject contentPanel;

    [TabGroup("references", "references")] [SerializeField]
    private Transform TownParent;

    [TabGroup("references", "references")] [SerializeField]
    private Canvas pressToInteractCanvas;
    
    public int OmenCount
    {
        get => omenCount;
        private set => omenCount = value;
    }
    
    [TabGroup("references", "Data")] [ShowInInspector]
    private int killCount = 0;

    public int KillCount
    {
        get => killCount;
        private set => killCount = value;
    }

    [TabGroup("references", "Data")] [ShowInInspector]
    private int omenKillCount = 0;

    public int OmenKillCount
    {
        get => omenKillCount;
        private set => omenKillCount = value;
    }

    [TabGroup("references", "Data")] [ShowInInspector]
    private List<Omen> omenList = new List<Omen>();

    [TabGroup("references", "Data")] [SerializeField]
    private GameObject objectToPlace;

    [TabGroup("references", "Data")] [ShowInInspector]
    public bool buildPanelOpen = false;

    [TabGroup("references", "Data")] [ShowInInspector]
    public bool inEditMode = false;

    [TabGroup("references", "Data")] [ShowInInspector]
    private int currentlyPlacingIndex;

    [TabGroup("references", "Data")] [ShowInInspector]
    private GameObject currentlyPlacingObject;

    [TabGroup("references", "Data")] [ShowInInspector]
    private Interractable currentlyFocused;

    [TabGroup("references", "Data")] [SerializeField]
    public bool inTown;
    
    
    public void SetInterractable(Interractable subject)
    {
        currentlyFocused = subject;
    }

    public void ForgetInterractable()
    {
        currentlyFocused = null;
    }
    

    /// <summary>
    /// Counts the number of omen killed in the current game
    /// </summary>
    public void KilledOmen()
    {
        omenCount--;
        omenKillCount++;
        hud.MinusOneOmen();

        if (omenCount <= 0)
        {
            Debug.Log("You Won!");
        }
        
    }


    
    void Start()
    {
        
        foreach (Omen omen in FindObjectsOfType<Omen>())
        {
            omenList.Add(omen);
            omenCount++;
        }

        buildPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets parameter inputted object as the currently edited object
    /// </summary>
    public void SetAsEditedObject(GameObject newObject, int newIndex)
    {
        currentlyPlacingObject = newObject;
        currentlyPlacingIndex = newIndex;
    }


    private void Update()
    {
        pressToInteractCanvas.gameObject.SetActive(currentlyFocused != null);

        if (!inEditMode) return;
        
        
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentlyPlacingObject);
            GameManager.Instance.AddToBuildables(currentlyPlacingIndex);
            DeactivateEditMove();
        }
        else if (Input.GetMouseButtonDown(2))
        {
            currentlyPlacingObject.transform.localScale = new Vector3(currentlyPlacingObject.transform.localScale.x * -1,
                currentlyPlacingObject.transform.localScale.y,
                currentlyPlacingObject.transform.localScale.z);
        }
    }

    /// <summary>
    /// Activates edit mode / deactivates player view
    /// </summary>
    public void ActivateEditMove(GameObject objectToEdit)
    {
        playerTransform.gameObject.SetActive(false);
        mainCamera.Follow = objectToEdit.transform;
        Cursor.visible = false;
        inEditMode = true;
    }

    /// <summary>
    /// Turns edit mode off
    /// </summary>
    public void DeactivateEditMove()
    {
        playerTransform.gameObject.SetActive(true);
        mainCamera.Follow = playerTransform;
        Cursor.visible = true;
        inEditMode = false;
    }

    /// <summary>
    /// Opens player inventory of buildables / Panel
    /// <para>Helper called within the caster because of centralization of input calling
    /// </summary>
    public void OnBuild()
    {
        
        if (inTown && player.ActualState() != Player.state.Casting)
        {
            if (buildPanelOpen)
            {
                CloseBuildPanel();
            }
            else
            {
                buildPanel.gameObject.SetActive(true);
                buildPanelOpen = true;

                int counter = 0;

                foreach (KeyValuePair<int, int> entry in GameManager.Instance.Buildables)
                {
                    CreateBuildList(entry.Key, entry.Value, counter);
                    counter++;
                }
                
            }
        }
    }

    public void OnInteraction()
    {
        if (currentlyFocused != null)
        {
            currentlyFocused.Interact();
        }
    }

    /// <summary>
    /// Closing the build panel
    /// </summary>
    private void CloseBuildPanel()
    {
        buildPanel.gameObject.SetActive(false);
        buildPanelOpen = false;

        // Collect all children first
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in contentPanel.transform)
        {
            children.Add(child.gameObject);
        }

        // Destroy them
        foreach (GameObject child in children)
        {
            Destroy(child);
        }
    }

    /// <summary>
    /// Initializes the buildables list on opening the inventory
    /// </summary>
    public void CreateBuildList(int itemIndex, int itemAmount, int spaceModifier)
    {
        GameObject item = Instantiate(panelItemPrefab, contentPanel.transform);
        item.transform.localPosition = new Vector3(-250, 2675 - (125 * spaceModifier), 0);
        PanelItem panelItem = item.GetComponent<PanelItem>();
        panelItem.itemImage.sprite = BuildDico.Instance.dico[itemIndex].GetComponent<SpriteRenderer>().sprite;
        panelItem.amount.text = $"X{itemAmount}";
        // Assigning the button click listener
        panelItem.button.onClick.AddListener(() => OnPanelItemButtonClick(itemIndex));
    }

    /// <summary>
    /// Custom event serving as the button click within the buildable inventory
    /// </summary>
    public void OnPanelItemButtonClick(int buildIndex)
    {
        CloseBuildPanel();
        GameObject itemToPlace = Instantiate(BuildDico.Instance.dico[buildIndex], TownParent);

        currentlyPlacingObject = itemToPlace;
        
        Buildable script = itemToPlace.GetComponent<Buildable>();
        
        Vector3 mousePosition = Input.mousePosition;
        
        itemToPlace.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        
        if (script != null)
        {
            script.SetEditingTrue();
        }
        ActivateEditMove(itemToPlace);
        currentlyPlacingIndex = buildIndex;
        
        GameManager.Instance.RemoveBuildable(buildIndex);
    }
    
    
}
