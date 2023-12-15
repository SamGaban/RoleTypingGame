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
    }




}
