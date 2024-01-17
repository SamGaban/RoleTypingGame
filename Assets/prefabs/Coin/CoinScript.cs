using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private HUD _hud;

    public bool _toKeep = false;

    private void Start()
    {
        _hud = FindObjectOfType<HUD>();
    }


    public void Throw()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CoinPicker"))
        {
            SoundMaster.Instance.CoinPickUp();
            GameManager.Instance.streakModifier += 0.33f;
            if (_hud != null) _hud.UpdateGoldCount();
            GameManager.Instance.SaveStreakModifier();
            Destroy(this.gameObject);
        }
    }
}
