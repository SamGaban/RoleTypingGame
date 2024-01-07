using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [TabGroup("references", "References")][SerializeField] private Button _newGameButton;
    [TabGroup("references", "References")][SerializeField] private Button _continueGameButton;
    [TabGroup("references", "References")][SerializeField] private Button _optionsButton;
    [TabGroup("references", "References")][SerializeField] private Button _ExitButton;

    [TabGroup("references", "References")][SerializeField] private GameObject _mainPanel;
    [TabGroup("references", "References")][SerializeField] private GameObject _confirmPanel;


    private bool _hasLoadData;

    private void LoadData()
    {
        _hasLoadData = ES3.Load<bool>("LoadData", false);
    }

    private void Awake()
    {
        LoadData(); // DO NOT FORGET TO PUT LOADDATA ON TRUE ON NEW GAME / TUTORIAL INIT AND SAVE
    }

    private void Start()
    {
        if (!_hasLoadData)
        {
            _continueGameButton.gameObject.SetActive(false);
        }
    }

    public void ActivateMainCanvas()
    {
        _mainPanel.SetActive(true);
    }

    public void DeactivateMainCanvas()
    {
        _mainPanel.SetActive(false);
    }

    public void ActivateConfirmPanel()
    {
        _confirmPanel.SetActive(true);
    }

    public void DeactivateConfirmPanel()
    {
        _confirmPanel.SetActive(false);
    }


    public void ToConfirmPanel()
    {
        _confirmPanel.SetActive(true);
        _mainPanel.SetActive(false);
    }

    public void ToMainMenu()
    {
        _confirmPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }


    public void ExitGame()
    {
        #if UNITY_EDITOR

                EditorApplication.isPlaying = false;

        #else
                                Application.Quit();
        
        #endif
    }

    public void ResetAllSaveables()
    {
        // Load the total count of saveable objects
        int totalSaveables = ES3.Load<int>("Saveable_Count", 0); // Default to 0 if "Saveable_Count" doesn't exist

        // Loop through each saveable object key and delete it
        for (int counter = 0; counter < totalSaveables; counter++)
        {
            string key = "Saveable_" + counter;
            if (ES3.KeyExists(key))
            {
                ES3.DeleteKey(key);
            }
        }

        // Reset and save the total count of saveable objects to 0 or to a new default value
        ES3.Save("Saveable_Count", 0);

        // Reset other necessary variables and save them as well
        // For example, if you have other data to reset:
        // ES3.Save("SomeOtherKey", defaultValue);

        // Add any additional resetting of saved data or state here as needed
    }



    public void NewGame()
    {



        Dictionary<int, int> _buildables = new Dictionary<int, int>();
        int _playerGold = 0;
        long _killCount = 0;
        Dictionary<int, int> _slotToSpell = new Dictionary<int, int>() { { 1, 1 }, { 2, 3 } };

        ES3.Save("savedBuildables", _buildables);
        ES3.Save("savedGold", _playerGold);
        ES3.Save("savedKillCount", _killCount);
        ES3.Save("slotDico", _slotToSpell);
        ResetAllSaveables();

        _hasLoadData = true;

        ES3.Save("LoadData", _hasLoadData);

        SceneManager.LoadScene(3);
    }

    public void MainNewGameButton()
    {
        if (_hasLoadData)
        {
            DeactivateMainCanvas();
            ActivateConfirmPanel();
        }
        else
        {
            NewGame();
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene(1);
    }

}
