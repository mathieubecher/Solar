using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractInput
{
    protected Vector2 _move;
    protected Controller _controller;
    protected PlayerInput _controls;
    protected float _gotoAngleVelocity = 0;
    public bool isManager;
    public AbstractInput(Controller controller)
    {
        _controller = controller;
    }

    public abstract void MovePlayer();
    
    public abstract void InputUpdate();
    public virtual void InputFixed(){}
    public virtual void SetManager(InputManager manager){}

    public virtual void BindSun(Options.Bind bind) {}

    public virtual void BindPlatform(Options.Bind bind) {}
    public virtual void Dead()
    {
    }

    public virtual bool CouldDie()
    {
        return true;
    }
}
