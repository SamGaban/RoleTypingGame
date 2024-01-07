using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialComingUp : MonoBehaviour
{
    [TabGroup("references", "References")][SerializeField] private Animator _animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _animator.Play("ComingUpAnim");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _animator.Play("ComingDown");
        }
    }
}
