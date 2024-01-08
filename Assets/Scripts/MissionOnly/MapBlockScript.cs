using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapBlockScript : MonoBehaviour
{
    [TabGroup("references", "References")][SerializeField] private TeleporterScript _teleportScript;

    [TabGroup("references", "References")][SerializeField] private Omen _omen;

    public bool isDone = false;


    [ButtonGroup]
    public void Teleport()
    {
        _teleportScript.GoTo();
    }

    private void Update()
    {
        if (isDone) return;

        if (_omen != null)
        {
            if (_omen.isDestroying)
            {
                isDone = true;
            }
        }
    }
}
