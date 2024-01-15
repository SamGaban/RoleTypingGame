using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TpSpotScript : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private TMP_Text letterText;

    

    private KeyCode _keyCode;

    private string _letter;

    private Player _player;
    
    private Caster _casterScript;

    private ClockTeleportScript clockScript;

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
        _player = FindObjectOfType<Player>();

        clockScript = FindObjectOfType<ClockTeleportScript>();
        
        _casterScript = FindObjectOfType<Caster>();
    }

    private void Update()
    {
        CheckForPress();
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
        SoundMaster.Instance.TeleportSpell();
        _player.transform.position = this.transform.position;
        _casterScript.isTeleporting = false;
    }

    private void SelfDestruct()
    {
        Destroy(clockScript.gameObject);
    }
    
}
