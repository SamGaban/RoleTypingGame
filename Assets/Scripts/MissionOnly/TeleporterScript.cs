using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    [ButtonGroup]
    public void GoTo()
    {
        _player.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1.5f);
    }
}
