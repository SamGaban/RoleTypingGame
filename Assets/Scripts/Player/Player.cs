using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum state
    {
        Idling,
        Running,
        Jumping,
        Casting,
        Flying
    }

    private state _state = state.Idling;

    public state ActualState()
    {
        return _state;
    }
    public void IdlingState()
    {
        _state = state.Idling;
    }
    public void RunningState()
    {
        _state = state.Running;
    }
    public void JumpingState()
    {
        _state = state.Jumping;
    }
    public void CastingState()
    {
        _state = state.Casting;
    }
    public void FlyingState()
    {
        _state = state.Flying;
    }
}
