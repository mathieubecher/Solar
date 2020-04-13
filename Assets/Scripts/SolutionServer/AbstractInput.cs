using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInput
{
    private Controller _controller;

    public AbstractInput(Controller controller)
    {
        _controller = controller;
    }

    public abstract void Velocity(Vector2 readValue);
    public abstract void MovePlayer();
    public abstract void RotateSun();
    public abstract void RotateCam();
    
    public void Update(){}
    public void FixedUpdate(){}
}
