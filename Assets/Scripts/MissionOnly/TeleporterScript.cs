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

    [TabGroup("references", "References")] [SerializeField]
    private GameObject explosion;

    [TabGroup("references", "References")] [SerializeField]
    private GameObject forcefield;

    private Forcefield _fieldScript;
    

    /// <summary>
    /// Starts the method by finding and assigning the Player instance in the scene
    /// </summary>
    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// Moves the player to a new position.
    /// <para>Generates an explosion on player teleport to get rid of potential enemies</para>
    /// </summary>
    /// <remarks>
    /// This method is used to move the player to a new position based on the current object's position.
    /// </remarks>
    [ButtonGroup]
    public void GoTo()
    {
        GameObject newExplosion = Instantiate(explosion);
        newExplosion.transform.SetParent(this.transform);
        newExplosion.transform.localPosition = Vector3.zero;
        Explosion script = newExplosion.GetComponent<Explosion>();

        if (script != null)
        {
            script.Initialize(1000, false);
        }

        GameObject newField = Instantiate(forcefield);
        newField.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);

        Forcefield fieldScript = newField.GetComponent<Forcefield>();

        if (fieldScript is not null)
        {
            fieldScript.ActivateShield(20);
            _fieldScript = fieldScript;
            StartCoroutine(RepeatedCall());
        }
        
        _player.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1.5f);

    }

    IEnumerator RepeatedCall()
    {
        for (int i = 0; i < 20; i++)
        {
            _fieldScript.Grow();
            yield return new WaitForSeconds(0.10f);
        }
    }
}
