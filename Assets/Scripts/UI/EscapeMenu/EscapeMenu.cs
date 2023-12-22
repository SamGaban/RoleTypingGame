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
    private Canvas difficultyCanvas;

    [TabGroup("references", "References")] [SerializeField]
    private Caster spellCasterScript;

    public void CloseTab()
    {
        this.gameObject.SetActive(false);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        
            EditorApplication.isPlaying = false;
            
        #else
        
            Application.Quit()
        
        #endif
    }

    public void DifficultyMenu()
    {
        difficultyCanvas.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
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