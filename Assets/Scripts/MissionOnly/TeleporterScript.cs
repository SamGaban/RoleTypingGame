using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Mission Map teleporter
/// </summary>
public class TeleporterScript : MonoBehaviour
{
    private Player _player;

    /// <summary>
    /// Starts the method by finding and assigning the Player instance in the scene
    /// </summary>
    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// Moves the player to a new position.
    /// </summary>
    /// <remarks>
    /// This method is used to move the player to a new position based on the current object's position.
    /// </remarks>
    [ButtonGroup]
    public void GoTo()
    {
        _player.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1.5f);
    }
}
