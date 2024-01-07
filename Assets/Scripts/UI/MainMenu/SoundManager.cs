using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sounds manager for the main menu
/// </summary>
public class SoundManager : MonoBehaviour
{
    [TabGroup("references", "References")][SerializeField] private MainMenuScript _mainMenu;


    [TabGroup("references", "References")][SerializeField] private AudioSource _menuClick;
    [TabGroup("references", "References")][SerializeField] private AudioSource _openPanel;
    [TabGroup("references", "References")][SerializeField] private AudioSource _closePanel;

    [TabGroup("references", "References")][SerializeField] private Slider _musicSlider;
    [TabGroup("references", "References")][SerializeField] private Slider _effectsSlider;

    [TabGroup("references", "References")][SerializeField] private Button _applyButton;


    public float musicVolume;
    public float effectsVolume;

    public void SaveVolume()
    {
        ES3.Save("musicVolume", musicVolume);
        ES3.Save("effectsVolume", effectsVolume);
    }

    public void LoadVolume()
    {
        musicVolume = ES3.Load<float>("musicVolume", 1.0f);
        effectsVolume = ES3.Load<float>("effectsVolume", 1.0f);
    }

    private void Awake()
    {
        _mainMenu = FindObjectOfType<MainMenuScript>();
        LoadVolume();
    }

    private void Start()
    {
        _applyButton.onClick.AddListener(() =>
        {
            MenuClick();
            SaveVolume();
        });
    }

    public void AdjustMusicVolume()
    {
        _mainMenu.AdjustVolume(musicVolume);
        musicVolume = _musicSlider.value;
    }

    public void AdjustEffectsVolume()
    {
        effectsVolume = _effectsSlider.value;
    }


    public void MenuClick()
    {
        _menuClick.volume = effectsVolume;
        _menuClick.Play();
    }

    public void OpenPanel()
    {
        _openPanel.volume = effectsVolume;
        _openPanel.Play();
    }

    public void ClosePanel()
    {
        _closePanel.volume = effectsVolume;
        _closePanel.Play();
    }
}
