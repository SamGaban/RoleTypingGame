using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    
    [TabGroup("references", "References")] [SerializeField]
    private Canvas menuCanvas;


    [TabGroup("references", "References")] [SerializeField]
    private Caster spellCasterScript;

    private GameSession session;

    private void Start()
    {
        session = FindObjectOfType<GameSession>();
    }

    /// <summary>
    /// Closes menu
    /// </summary>
    public void CloseTab()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Reloads active scene
    /// </summary>
    public void TryAgain()
    {
        if (session.inTown) return;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Quitting editor game window or build application
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
        
            EditorApplication.isPlaying = false;
            
        #else

            GameManager.Instance.WholeSave();

            Application.Quit();
        
        #endif
    }

    
    
    public void Abandon()
    {
        if (session.inTown) return;

        SceneManager.LoadScene(0);

    }

    public void VeryEasy()
    {
        GameManager.Instance.difficultyLevel = Caster.DifficultyLevel.VeryEasy;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Easy()
    {
        GameManager.Instance.difficultyLevel = Caster.DifficultyLevel.Easy;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Normal()
    {
        GameManager.Instance.difficultyLevel = Caster.DifficultyLevel.Normal;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Hard()
    {
        GameManager.Instance.difficultyLevel = Caster.DifficultyLevel.Hard;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void VeryHard()
    {
        GameManager.Instance.difficultyLevel = Caster.DifficultyLevel.VeryHard;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    
}
