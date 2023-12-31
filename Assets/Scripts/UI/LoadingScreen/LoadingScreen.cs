using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private Image image;

    [TabGroup("references", "References")] [SerializeField]
    private SpriteRenderer spriteRenderer;    

    void Update()
    {
        image.sprite = spriteRenderer.sprite;
    }
}
