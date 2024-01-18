using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private HUD _hud;

    private GameSession _sess;

    public bool _toKeep = false;

    private void Start()
    {
        _hud = FindObjectOfType<HUD>();
        _sess = FindObjectOfType<GameSession>();
    }


    public void Throw()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CoinPicker"))
        {
            Player playerScript = other.transform.parent.gameObject.GetComponent<Player>();

            if (playerScript != null)
            {
                playerScript.Heal(10);
            }
            
            SoundMaster.Instance.CoinPickUp();
            _sess.streakStorage += 0.33f;
            if (_hud != null) _hud.UpdateGoldCount();
            Destroy(this.gameObject);
        }
    }
}
