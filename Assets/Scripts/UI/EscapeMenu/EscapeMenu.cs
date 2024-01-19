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

    [TabGroup("references", "References")] [SerializeField]
    private Canvas loadingScreen;

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
        if (session.inTown) GameManager.Instance.WholeSave();
        
        loadingScreen.gameObject.SetActive(true);
        
        Invoke("QuitHelper", 2f);
    }

    private void QuitHelper()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackToMainMenu()
    {
        SoundMaster.Instance.StopVillageLoop();
        SoundMaster.Instance.MenuClick();
        GameManager.Instance.WholeSave();
        SceneManager.LoadScene(0);
    }

    
    /// <summary>
    /// Triggers the lose screen to the current mission directly
    /// </summary>
    public void Abandon()
    {
        SoundMaster.Instance.MenuClick();

        if (session.inTown) return;

        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            player.LoseCurrentMission(true);
        }

    }
    
    
}
