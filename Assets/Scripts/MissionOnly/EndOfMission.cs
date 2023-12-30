using System;
using System.Collections;
using System.Collections.Generic;
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


    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        });
    }


    private int _killCount = 0;
    private int _goldCount = 0;

    public void Win(int killCount, int goldCount)
    {
        _killCount = killCount;
        _goldCount = goldCount;
        
        Invoke("WinHelper", 2.5f);
    }

    private void WinHelper()
    {
        Time.timeScale = 0.005f;
        
        HUD hud = FindObjectOfType<HUD>();
        if (hud != null)
        {
            hud.gameObject.SetActive(false);
        }
        
        canvas.gameObject.SetActive(true);

        winLoseText.text = "You Won !";

        killsText.text = $"{_killCount} Kills";

        goldText.text = $"+{_goldCount} Gold";
    }

    public void Lose(int KillCount)
    {
        _killCount = KillCount;
        
        Invoke("LoseHelper", 2.5f);
    }

    private void LoseHelper()
    {
        Time.timeScale = 0.005f;
        
        HUD hud = FindObjectOfType<HUD>();
        if (hud != null)
        {
            hud.gameObject.SetActive(false);
        }
        
        canvas.gameObject.SetActive(true);
        
        winLoseText.text = "You Lose !";

        killsText.text = $"{_killCount} Kills";

        goldText.text = $"+0 Gold";
    }

}
