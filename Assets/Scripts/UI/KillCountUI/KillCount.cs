using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCount : MonoBehaviour
{
    [TabGroup("references", "References")]
    [SerializeField]
    private TMP_Text killCountText;

    public void UpdateDisplay()
    {
        killCountText.text = string.Format("{0:0000000}", GameManager.Instance.killCount);
    }
}
