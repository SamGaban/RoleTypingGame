using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform _transform;
    [SerializeField] HealthManager _healthManager;

    public float direction;
    public enum equipped
    {
        Spear,
        Magic
    }
    public enum state
    {
        Idling,
        Running,
        JumpingUp,
        JumpingDown,
        Casting,
    }

    private state _state = state.Idling;
    private equipped _equipped = equipped.Spear;

    private bool canBeHurt = true;

    private void Update()
    {
        direction = Mathf.Sign(_transform.localScale.x);
    }


    #region States triggers region
    public state ActualState()
    {
        return _state;
    }
    public equipped ActualEquipped()
    {
        return _equipped;
    }
    public void IdlingState()
    {
        _state = state.Idling;
    }
    public void RunningState()
    {
        _state = state.Running;
    }
    public void JumpingUpState()
    {
        _state = state.JumpingUp;
    }
    public void JumpingDownState()
    {
        _state = state.JumpingDown;
    }
    public void CastingState()
    {
        _state = state.Casting;
    }
    #endregion


    public void ToggleEquipped()
    {
        if (_equipped == equipped.Spear)
        {
            _equipped = equipped.Magic;
        }
        else if (_equipped == equipped.Magic)
        {
            _equipped = equipped.Spear;
        }
    }
    /// <summary>
    /// Hurts the player by triggering it's hpManager
    /// </summary>
    /// <param name="hpAmount"></param>
    public void Hurt(int hpAmount)
    {
        if (!canBeHurt) { return; }
        _healthManager.DownHp(hpAmount);
        canBeHurt = false;
        Invoke("InvincibilityToggle", 2f);
    }
    /// <summary>
    /// Toggles player invicibility (which triggers after hit) off => To be called after a little invicibility time
    /// </summary>
    private void InvincibilityToggle()
    {
        canBeHurt = true;
    }
    /// <summary>
    /// Heals the player by triggering its hpManager
    /// </summary>
    /// <param name="hpAmount"></param>
    public void Heal(int hpAmount)
    {
        _healthManager.UpHp(hpAmount);
    }
    /// <summary>
    /// Method called by pressing shift, to cancel a spellcast before it's finished
    /// </summary>
    private void OnToggleEquipped()
    {
        if (_state == state.Casting) { return; } // If actually casting, return

        ToggleEquipped();
    }
    /// <summary>
    /// Casting state on or off if the actual state is using magic
    /// </summary>
    public void ToggleCasting()
    {
        if (_equipped != equipped.Magic) { return; }

        if (_state != state.Casting)
        {
            _state = state.Casting;
        }
        else if (_state == state.Casting)
        {
            _state = state.Idling;
        }
    }
}
