using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Singleton Handling all sounds in the game
/// </summary>
public class SoundMaster : MonoBehaviour
{
    #region SingletonInit
    private static SoundMaster instance = null;

    public static SoundMaster Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }
    #endregion


    [TabGroup("references", "Music")][SerializeField] private AudioSource _villageMusic;

    [TabGroup("references", "Music")][SerializeField] private AudioSource _missionMusic;

    [TabGroup("references", "Music")][SerializeField] private AudioSource _missionMusicEnding;

    [TabGroup("references", "Player")][SerializeField] private AudioSource _playerFootsteps;

    [TabGroup("references", "Player")][SerializeField] private AudioSource _toggleEquippped;

    [TabGroup("references", "Player")][SerializeField] private AudioSource _meleeHit;

    [TabGroup("references", "Spells")][SerializeField] private AudioSource _oneHundredPercent;

    [TabGroup("references", "Spells")][SerializeField] private AudioSource _fireballSpell;

    [TabGroup("references", "Spells")][SerializeField] private AudioSource _gooSpell;

    [TabGroup("references", "Spells")][SerializeField] private AudioSource _timeWarpIn;

    [TabGroup("references", "Spells")][SerializeField] private AudioSource _timeWarpOut;

    [TabGroup("references", "Spells")][SerializeField] private AudioSource _purifySpell;

    [TabGroup("references", "Spells")][SerializeField] private AudioSource _shieldSpell;

    [TabGroup("references", "Enemies")][SerializeField] private AudioSource _omenLifeDown;

    [TabGroup("references", "Effects")][SerializeField] private AudioSource _openPanel;

    [TabGroup("references", "Effects")][SerializeField] private AudioSource _menuClick;

    [TabGroup("references", "Effects")][SerializeField] private AudioSource _teleportZap;






    /// <summary>
    /// Village Music Loop
    /// </summary>
    public void PlayVillageLoop()
    {
        _villageMusic.volume = GameManager.Instance.musicVolume;
        _villageMusic.Play();
    }
    public void StopVillageLoop()
    {
        _villageMusic?.Stop();
    }

    public void PlayMissionLoop()
    {
        _missionMusic.volume = GameManager.Instance.musicVolume;
        _missionMusic.Play();
    }

    public void StopMissionLoop()
    {
        _missionMusic?.Stop();
    }

    public void PlayMissionEnding()
    {
        _missionMusicEnding.volume = GameManager.Instance.musicVolume;
        _missionMusicEnding.Play();
    }


    /// <summary>
    /// Player Footsteps loop start
    /// </summary>
    public void PlayerFootStepsLoopStart()
    {
        _playerFootsteps.volume = GameManager.Instance.effectsVolume;
        _playerFootsteps.Play();
    }
    /// <summary>
    /// Player footsteps loop end
    /// </summary>
    public void PlayerFootstepsLoopEnd()
    {
        _playerFootsteps?.Stop();
    }
    /// <summary>
    /// Fireball spell sound
    /// </summary>
    public void Fireball()
    {
        _fireballSpell.volume = GameManager.Instance.effectsVolume;
        _fireballSpell.Play();
    }
    /// <summary>
    /// 100% precision spellcast sound effect
    /// </summary>
    public void OneHundredPercent()
    {
        _oneHundredPercent.volume = GameManager.Instance.effectsVolume * 0.2f;
        _oneHundredPercent.Play();
    }
    /// <summary>
    /// Goo spell sound effect
    /// </summary>
    public void GooSpell()
    {
        _gooSpell.volume = GameManager.Instance.effectsVolume;
        _gooSpell.Play();
    }

    public void TimeWarpIn()
    {
        _timeWarpIn.volume = GameManager.Instance.effectsVolume;
        _timeWarpIn.Play();
    }

    public void TimeWarpOut()
    {
        _timeWarpOut.volume = GameManager.Instance.effectsVolume;
        _timeWarpOut.Play();
    }

    public void OmenLifeDown()
    {
        _omenLifeDown.volume = GameManager.Instance.effectsVolume;
        _omenLifeDown.Play();
    }

    public void PurifySpell()
    {
        _purifySpell.volume = GameManager.Instance.effectsVolume;
        _purifySpell.Play();
    }

    public void ShieldSpell()
    {
        _shieldSpell.volume = GameManager.Instance.effectsVolume;
        _shieldSpell.Play();
    }

    public void OpenPanel()
    {
        _openPanel.volume = GameManager.Instance.effectsVolume;
        _openPanel.Play();
    }

    public void MenuClick()
    {
        _menuClick.volume = GameManager.Instance.effectsVolume;
        _menuClick.Play();
    }

    public void ToggleEquipped()
    {
        _toggleEquippped.volume = GameManager.Instance.effectsVolume;
        _toggleEquippped.Play();
    }

    public void MeleeHit()
    {
        _meleeHit.volume = GameManager.Instance.effectsVolume;
        _meleeHit.Play();
    }

    public void TeleportZap()
    {
        _teleportZap.volume = GameManager.Instance.effectsVolume;
        _teleportZap.Play();
    }
}
