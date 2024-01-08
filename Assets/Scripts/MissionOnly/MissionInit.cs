using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Class used to load a map randomly created based on the questboard item the player clicked in town
/// </summary>
public class MissionInit : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private List<GameObject> mapBlocksPrefabs;

    [TabGroup("references", "References")]
    [SerializeField]
    private Canvas _nextIslandCanvas;

    [TabGroup("references", "References")]
    [SerializeField]
    private Animator _flashAnimator;

    [TabGroup("references", "References")]
    [ShowInInspector] private List<MapBlockScript> mapBlocks;

    public bool readyToJump = false;

    private bool doneCheck = false;

    private Player _player;
    
    void Start()
    {
        mapBlocks = new List<MapBlockScript>();
        _player = FindObjectOfType<Player>();
        BuildMap();
    }

    /// <summary>
    /// Loads a number of map chunks determined by the amount of omen in the current mission
    /// </summary>
    private void BuildMap()
    {
        int i;
        
        for (i = 0; i < GameManager.Instance.OmenAmount(); i++)
        {
            GameObject map = Instantiate(mapBlocksPrefabs[Random.Range(0, mapBlocksPrefabs.Count)]);
            map.transform.position = new Vector3(0, 0 - (i * 170.00f), 0);

            MapBlockScript script = map.GetComponent<MapBlockScript>();

            if (script != null)
            {
                mapBlocks.Add(script);
            }

        }

        Invoke("TeleportToIndexZero", 0.5f);

    }

    public void TeleportToIndexZero()
    {
        mapBlocks[0].Teleport();
    }

    public void TeleportForPlayerInput()
    {
        _flashAnimator.Play("Flash", -1, 0f);

        doneCheck = false;
        _nextIslandCanvas.gameObject.SetActive(false);
        readyToJump = false;
        mapBlocks.RemoveAt(0);
        mapBlocks[0].Teleport();
    }

    private void Update()
    {
        if (mapBlocks[0].isDone && !doneCheck)
        {
            doneCheck = true;
            readyToJump = true;
            _nextIslandCanvas.gameObject.SetActive(true);
        }
    }

}
