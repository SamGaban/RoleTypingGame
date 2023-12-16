using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float direction;

    [SerializeField] Transform _transform;
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


    private bool _megaJump;

    public bool MegaJump
    {
        get { return _megaJump; }
        private set { _megaJump = value; }
    }



    private void Update()
    {
        direction = Mathf.Sign(_transform.localScale.x);
    }


    public void ActivateMegaJump()
    {
        _megaJump = true;
    }
    public void DeactivateMegaJump()
    {
        _megaJump = false;
    }

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
