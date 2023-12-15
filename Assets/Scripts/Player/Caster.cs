using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TypingComparator;

public class Caster : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] Canvas _canvas;
    [SerializeField] TMP_Text _text;
    WordsGenerator _generator;

    private void Start()
    {
        _generator = new WordsGenerator();
    }

    private void Update()
    {
        FlipCanvas();

        if (_player.ActualState() == Player.state.Casting)
        {
            _canvas.gameObject.SetActive(true);

            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    if (IsRelevantKey(kcode))
                    {
                        char keyChar = (char)kcode;
                    }
                }
            }

        }
        else
        {
            _canvas.gameObject.SetActive(false);
        }
    }

    private void OnSkill1()
    {
        // Setting an active sentence
        // Toggling Casting with that active sentence
        // On correct => Activate skill 1
    }
    private void OnSkill2()
    {

    }
    private void OnSkill3()
    {

    }
    private void OnSkill4()
    {

    }
    /// <summary>
    /// Checks if the key pressed needs to be registered by the program
    /// </summary>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    bool IsRelevantKey(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.A:
                return true;
            case KeyCode.B:
                return true;
            case KeyCode.C:
                return true;
            case KeyCode.D:
                return true;
            case KeyCode.E:
                return true;
            case KeyCode.F:
                return true;
            case KeyCode.G:
                return true;
            case KeyCode.H:
                return true;
            case KeyCode.I:
                return true;
            case KeyCode.J:
                return true;
            case KeyCode.K:
                return true;
            case KeyCode.L:
                return true;
            case KeyCode.M:
                return true;
            case KeyCode.N:
                return true;
            case KeyCode.O:
                return true;
            case KeyCode.P:
                return true;
            case KeyCode.Q:
                return true;
            case KeyCode.R:
                return true;
            case KeyCode.S:
                return true;
            case KeyCode.T:
                return true;
            case KeyCode.U:
                return true;
            case KeyCode.V:
                return true;
            case KeyCode.W:
                return true;
            case KeyCode.X:
                return true;
            case KeyCode.Y:
                return true;
            case KeyCode.Z:
                return true;
            case KeyCode.Comma:
                return true;
            case KeyCode.Space:
                return true;
            default:
                return false;
        }
    }
    /// <summary>
    /// Flips canvas in relation to player's direction to keep it straight
    /// </summary>
    private void FlipCanvas()
    {
        if (_player.transform.localScale.x == 1)
        {
            _canvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
        else
        {
            _canvas.transform.localScale = new Vector3(-0.01f, 0.01f, 0.01f);
        }
    }

}
