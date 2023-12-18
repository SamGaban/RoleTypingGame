using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] Animator _mainAnimator;
    [SerializeField] RuntimeAnimatorController _spearAnimator;
    [SerializeField] RuntimeAnimatorController _magicAnimator;

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

    private void Update()
    {
        PlayerEquippedCheck();

        PlayerStateCheck(Player.state.Running, "isRunning");

        PlayerStateCheck(Player.state.JumpingUp, "isJumpingUp");

        PlayerStateCheck(Player.state.JumpingDown, "isJumpingDown");

        if (_player.ActualEquipped() != Player.equipped.Magic) { return; } // If player is in magic mode only vvv

        PlayerStateCheck(Player.state.Casting, "isCasting");
    }




}
