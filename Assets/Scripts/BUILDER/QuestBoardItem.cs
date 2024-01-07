using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Clickable questboard link
/// </summary>
public class QuestBoardItem : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private TMP_Text diffText;

    [TabGroup("references", "References")] [SerializeField]
    private TMP_Text omenText;

    [TabGroup("references", "References")] [SerializeField]
    private TMP_Text goldText;

    [TabGroup("references", "References")] [SerializeField]
    private Button button;

    
    [TabGroup("references", "data")] [ShowInInspector]
    private Caster.DifficultyLevel _difficultyLevel;

    [TabGroup("references", "data")] [ShowInInspector]
    private int _omenAmount;

    [TabGroup("references", "data")] [ShowInInspector]
    private int _goldAmount;

    
    /// <summary>
    /// Creating a random set of quests
    /// </summary>
    private void Start()
    {
        int difficultyCount = Enum.GetValues(typeof(Caster.DifficultyLevel)).Length;
        int difficulty = Random.Range(0, difficultyCount);
        _difficultyLevel = (Caster.DifficultyLevel)difficulty;

        diffText.text = _difficultyLevel.ToString();

        _omenAmount = Random.Range(1, 13);
        omenText.text = _omenAmount.ToString();

        // Determine gold amount
        int baseGoldAmount = GetBaseGoldAmount();
        int difficultyMultiplier = ((int)_difficultyLevel + 1); // Simple multiplier based on difficulty level
        _goldAmount = baseGoldAmount * _omenAmount * difficultyMultiplier; // Now including _omenAmount in the calculation

        goldText.text = _goldAmount.ToString(); // Assuming goldText is a UI Text element for displaying gold
        
        button.onClick.AddListener(() =>
        {
            GameManager.Instance.difficultyLevel = _difficultyLevel;
            GameManager.Instance.SetGoldReward(_goldAmount);
            GameManager.Instance.SetOmenAmount(_omenAmount);
            
            GameManager.Instance.SaveTown();

            SceneManager.LoadScene(2);
        });
        
    }

    
    
    /// <summary>
    /// Calculates a base gold reward that's randomly a little higher for some missions
    /// </summary>
    /// <returns>Base gold amount before calculations based on difficulty</returns>
    private int GetBaseGoldAmount()
    {
        float rand = Random.Range(0f, 100f); // Random number between 0 and 100
        if (rand < 75f) // 75% chance
            return 1;
        else if (rand < 95f) // Next 20% chance
            return 2;
        else // Remaining 5% chance
            return 3;
    }

}
