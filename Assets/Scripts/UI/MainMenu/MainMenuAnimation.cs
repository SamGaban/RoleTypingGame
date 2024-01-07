using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAnimation : MonoBehaviour
{
    [TabGroup("references", "References")]
    [SerializeField]
    private Image _mainSprite;

    [TabGroup("references", "References")]
    [SerializeField]
    private Animator _mainAnimator;

    private SpriteRenderer _mainSpriteRenderer;

    private void Start()
    {
        _mainSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _mainSprite.sprite = _mainSpriteRenderer.sprite;
    }
}
