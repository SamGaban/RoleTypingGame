using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfMission : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private Canvas canvas;

    [TabGroup("references", "References")] [SerializeField]
    private TMP_Text winLoseText;

    [TabGroup("references", "References")] [SerializeField]
    private TMP_Text killsText;

    [TabGroup("references", "References")] [SerializeField]
    private TMP_Text goldText;

    [TabGroup("references", "References")] [SerializeField]
    private Button button;


    // DATABASE VARIABLES

    private List<int> wpmOfMission;
    private List<int> precisionOfMission;

    /// <summary>
    /// Adds a value to the list of words per minute (wpm) for the mission.
    /// </summary>
    /// <param name="wpm">The value to add to the list.</param>
    public void AddToWpm(int wpm)
    {
        wpmOfMission.Add(wpm);
    }

    /// <summary>
    /// Adds a value to the list of precision logs for the mission.
    /// </summary>
    /// <param name="precision">The value to be added to the precision of the mission.</param>
    public void AddToPrecision(int precision)
    {
        precisionOfMission.Add(precision);
    }

    private DateTime _startTime;

    private bool _aborted = false;


    /// <summary>
    /// Setting the main button to put the timescale back to normal and load the town
    /// </summary>
    private void Start()
    {
        _startTime = DateTime.Now;

        wpmOfMission = new List<int>();
        precisionOfMission = new List<int>();


        button.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        });
    }


    private int _killCount = 0;
    private int _goldCount = 0;

    /// <summary>
    /// Gives the gold to the player and displays the "win" screen at the end of the mission
    /// </summary>
    /// <param name="killCount">monster kill count</param>
    /// <param name="goldCount">mission's final gold reward</param>
    public void Win(int killCount, int goldCount)
    {
        _killCount = killCount;
        _goldCount = goldCount;
        
        Invoke("WinHelper", 2.5f);
    }

    /// <summary>
    /// Delayed display of the win screen running in the win method
    /// </summary>
    private void WinHelper()
    {
        SoundMaster.Instance.PlayerFootstepsLoopEnd();
        SoundMaster.Instance.StopMissionLoop();
        SoundMaster.Instance.PlayMissionEnding();

        Time.timeScale = 0f;
        
        HUD hud = FindObjectOfType<HUD>();
        if (hud != null)
        {
            hud.gameObject.SetActive(false);
        }
        
        canvas.gameObject.SetActive(true);

        winLoseText.text = "You Won !";

        killsText.text = $"{_killCount} Kills";

        goldText.text = $"+{_goldCount} Gold";

        GameManager.Instance.killCount += _killCount;

        // ####################### DATABASE ############################
        DBMaster.Instance.InsertIntoGameLogs(_startTime, Convert.ToInt32(wpmOfMission.Average()), Convert.ToInt32(precisionOfMission.Average()), (int)GameManager.Instance.difficultyLevel, true, _goldCount, _killCount);

        Debug.Log("Inserted won Game in DB");
        // #############################################################


    }

    /// <summary>
    /// Sets the monster kill count ready for lose screen
    /// </summary>
    /// <param name="KillCount">Actual monster kill count</param>
    public void Lose(int KillCount, bool aborted)
    {
        _killCount = KillCount;

        if (aborted)
        {
            _aborted = true;
        }
        
        Invoke("LoseHelper", 0.5f);
    }

    /// <summary>
    /// Delayed display of the lose screen running in the lose method
    /// </summary>
    private void LoseHelper()
    {
        SoundMaster.Instance.PlayerFootstepsLoopEnd();
        SoundMaster.Instance.StopMissionLoop();
        SoundMaster.Instance.PlayMissionEnding();

        Time.timeScale = 0f;
        
        HUD hud = FindObjectOfType<HUD>();
        if (hud != null)
        {
            hud.gameObject.SetActive(false);
        }
        
        canvas.gameObject.SetActive(true);

        winLoseText.text = _aborted ? "Abandoned" : "You Failed!";

        killsText.text = $"{_killCount} Kills";

        goldText.text = $"+0 Gold";

        // ####################### DATABASE ############################

        if (_aborted) return;

        DBMaster.Instance.InsertIntoGameLogs(_startTime, Convert.ToInt32(wpmOfMission.Average()), Convert.ToInt32(precisionOfMission.Average()), (int)GameManager.Instance.difficultyLevel, false, _goldCount, _killCount);

        Debug.Log("Inserted lost Game in DB");
    }

}
