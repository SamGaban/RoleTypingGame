using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] Animator _mainAnimator;
    [SerializeField] RuntimeAnimatorController _spearAnimator;
    [SerializeField] RuntimeAnimatorController _magicAnimator;
    
    [SerializeField] private Hitter hitterScript;

    private bool canAttack = true;

    private void Start()
    {
        _mainAnimator.runtimeAnimatorController = _spearAnimator;
    }

    /// <summary>
    /// Checks if the player actually is in the input state, if so, sets the inputted animator bool true, else, false;
    /// </summary>
    /// <param name="state">Player.state to check</param>
    /// <param name="animatorBool">bool linked to the state</param>
    private void PlayerStateCheck(Player.state state, string animatorBool)
    {
        if (_player.ActualState() == state)
        {
            _mainAnimator.SetBool(animatorBool, true);
        }
        else
        {
            _mainAnimator.SetBool(animatorBool, false);
        }
    }
    /// <summary>
    /// If not in casting mode, activates the roll
    /// </summary>
    private void OnCast()
    {
        if (_player.ActualState() == Player.state.Casting) return;

        if (!_player.CanRoll()) return;
        
        _mainAnimator.SetBool("isRolling", true);
        _player.InvincibleOn();
        _player.TurnRollOff();
        Invoke("TurnRollOnAgain", 3f);
        Invoke("TurnRollingOff", 0.5f);
        Invoke("InvincibleOff", 1.4f);
    }
    /// <summary>
    /// Turns player roll on
    /// </summary>
    private void TurnRollOnAgain()
    {
        _player.TurnRollOn();
    }
    /// <summary>
    /// Turns player invincibility off
    /// </summary>
    private void InvincibleOff()
    {
        _player.InvincibleOff();
    }
    /// <summary>
    /// Turns player's roll off
    /// </summary>
    private void TurnRollingOff()
    {
        _mainAnimator.SetBool("isRolling", false);
    }
    /// <summary>
    /// Switches animator to the corresponding one, either if casting or not
    /// </summary>
    private void PlayerEquippedCheck()
    {
        if (_player.ActualEquipped() == Player.equipped.Spear)
        {
            _mainAnimator.runtimeAnimatorController = _spearAnimator;
        }
        else if (_player.ActualEquipped() == Player.equipped.Magic)
        {
            _mainAnimator.runtimeAnimatorController = _magicAnimator;
        }
    }
    /// <summary>
    /// Checks for states to activate the animations (First method used, not the most efficient apparently)
    /// </summary>
    private void Update()
    {
        PlayerEquippedCheck();

        PlayerStateCheck(Player.state.Running, "isRunning");

        PlayerStateCheck(Player.state.JumpingUp, "isJumpingUp");

        PlayerStateCheck(Player.state.JumpingDown, "isJumpingDown");

        if (_player.ActualEquipped() != Player.equipped.Magic) { return; } // If player is in magic mode only vvv

        PlayerStateCheck(Player.state.Casting, "isCasting");
    }
    /// <summary>
    /// This iteration of OnSkill1() is for the spear attack, so, if not in magic mode
    /// </summary>
    private void OnSkill1() // Spear attack animation + state trigger
    {
        if (!canAttack) return;
        
        if (_player.ActualEquipped() == Player.equipped.Magic) return;

        if (_player.ActualState() == Player.state.Rolling) return;

        if (_player.ActualState() == Player.state.JumpingUp) return;

        if (_player.ActualState() == Player.state.JumpingDown) return;

        _player.canSwitchEquipped = false;

        canAttack = false;
        
        _mainAnimator.SetBool("isHitting", true);
        
        Invoke("ActivateSpear", 0.4f);
        
        Invoke("TurnSkillOneOff", 1f);
        
        Invoke("CanAttackAgain", 1.5f);
        
        Invoke("CanSwitchEquippedAgain", 1.5f);
    }
    /// <summary>
    /// To take from the player the ability to switch equipment directly after attacking with spear
    /// </summary>
    private void CanSwitchEquippedAgain()
    {
        _player.canSwitchEquipped = true;
    }
    /// <summary>
    /// Can attack with the spear
    /// </summary>
    private void ActivateSpear()
    {
        hitterScript.Activate();
    }
    /// <summary>
    /// Exits spear animation
    /// </summary>
    private void TurnSkillOneOff()
    {
        _mainAnimator.SetBool("isHitting", false);
    }
    /// <summary>
    /// timer related variable to be able to attack again
    /// </summary>
    private void CanAttackAgain()
    {
        canAttack = true;
    }


}
