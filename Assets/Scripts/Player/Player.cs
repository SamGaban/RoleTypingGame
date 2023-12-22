using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Main player script
/// </summary>
public class Player : MonoBehaviour
{
    [TabGroup("references", "References")]
    [SerializeField] Transform _transform;
    [TabGroup("references", "References")]
    [SerializeField] HealthManager _healthManager;
    [TabGroup("references", "References")]
    [SerializeField] private Move moveScript;
    [TabGroup("references", "Data")]
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
        Rolling,
        Attacking
    }
    
    [TabGroup("references", "Data")]
    private state _state = state.Idling;
    
    [TabGroup("references", "Data")]
    private equipped _equipped = equipped.Spear;

    [TabGroup("references", "Data")] [ShowInInspector]
    private bool canBeHurt = true;
    
    [TabGroup("references", "Data")]
    private bool canRoll = true;

    [TabGroup("references", "Data")]
    public bool canSwitchEquipped = true;

    [TabGroup("references", "Data")] [ShowInInspector]
    private bool isDead = false;
    
    private void Update()
    {
        DirectionTrack();
        DeathCheck();
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
    
    public void SetState(state stateToSet)
    {
        _state = stateToSet;
    }

    
    #endregion
    

    
    #region RollRelated

    
    /// <summary>
    /// Tracks the availability of the player's roll
    /// </summary>
    /// <returns>true if the player can roll</returns>
    public bool CanRoll()
    {
        return canRoll;
    }
    
    /// <summary>
    /// Makes canroll true
    /// </summary>
    public void TurnRollOn()
    {
        canRoll = true;
    }
    
    /// <summary>
    /// Makes canRoll false
    /// </summary>
    public void TurnRollOff()
    {
        canRoll = false;
    }

    

    #endregion
    
    #region LifeRelated

    
    /// <summary>
    /// Hurts the player by triggering it's hpManager
    /// </summary>
    /// <param name="hpAmount"></param>
    public void Hurt(int hpAmount, Vector2 knckBack)
    {
        if (!canBeHurt) { return; }

        moveScript.KnockBack(knckBack);
        
        
        _healthManager.DownHp(hpAmount);
        canBeHurt = false;
        Invoke("InvincibilityToggle", 1f);
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
    /// Turns canbehurt => false
    /// </summary>
    public void InvincibleOn()
    {
        canBeHurt = false;
    }
    
    /// <summary>
    /// Turns canbehurt to true
    /// </summary>
    public void InvincibleOff()
    {
        canBeHurt = true;
    }

    private void DeathCheck()
    {
        if (isDead) return;
        
        if (_healthManager.isDead())
        {
            isDead = true;
        }
    }

    public bool IsDead()
    {
        return isDead;
    }


    #endregion

    #region Toggling Region

    

    
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
    
    /// <summary>
    /// Toggles between spear and magic
    /// </summary>
    public void ToggleEquipped()
    {
        if (!canSwitchEquipped) return;
        
        if (_equipped == equipped.Spear)
        {
            _equipped = equipped.Magic;
        }
        else if (_equipped == equipped.Magic)
        {
            _equipped = equipped.Spear;
        }
    }
    
    #endregion

    #region MoveRelated

    /// <summary>
    /// Tracks the direction the player is facing
    /// </summary>
    private void DirectionTrack()
    {
        direction = Mathf.Sign(_transform.localScale.x);
    }

    #endregion
    

}
