using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TpSpotScript : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private TMP_Text letterText;

    private Collider2D _ownCollider;
    

    private KeyCode _keyCode;

    private string _letter;

    private Player _player;
    
    private Caster _casterScript;

    private ClockTeleportScript clockScript;

    private List<Tilemap> tilemaps;

    [SerializeField]
    private GameObject tpParticlesPrefab;

    public void Initialize(Transform newParent, KeyCode kcode, Vector2 position)
    {
        this.transform.SetParent(newParent);
        
        _keyCode = kcode;
        
        _letter = _keyCode.ToString();

        letterText.text = _letter;

        this.transform.localPosition = position;
    }

    private void Start()
    {
        // Find all GameObjects with the tag "Ground" and get their Tilemap components
        tilemaps = new List<Tilemap>();
        GameObject[] tilemapObjects = GameObject.FindGameObjectsWithTag("Ground");
        foreach (GameObject tmObject in tilemapObjects) {
            Tilemap tm = tmObject.GetComponent<Tilemap>();
            if (tm != null) {
                tilemaps.Add(tm);
            }
        }
        
        
        _ownCollider = GetComponent<Collider2D>();
        
        _player = FindObjectOfType<Player>();

        clockScript = FindObjectOfType<ClockTeleportScript>();
        
        _casterScript = FindObjectOfType<Caster>();
        
    }

    private void Update()
    {
        CheckForPress();
        
        foreach (Tilemap tilemap in tilemaps) {
            // Convert the object's world position to a cell position in the tilemap
            Vector3Int cellPosition = tilemap.WorldToCell(this.transform.position);

            // Check if there is a tile at the given cell position
            if (tilemap.HasTile(cellPosition)) {
                Destroy(this.gameObject);
                break;
            }
        }
        
        
    }

    private void CheckForPress()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            Teleport();
            SelfDestruct();
        }
        
    }

    private void Teleport()
    {
        GameObject particles = Instantiate(tpParticlesPrefab, this.transform.position, Quaternion.identity);
        SoundMaster.Instance.TeleportSpell();
        _player.transform.position = this.transform.position;
        _casterScript.isTeleporting = false;
    }

    private void SelfDestruct()
    {
        Destroy(clockScript.gameObject);
    }
    
}
