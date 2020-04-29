using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Solo : AbstractInput
{
    

    public Solo(Controller controller) : base(controller)
    {
        _controller = controller;
        _controls = _controller.GetComponent<PlayerInput>();
        
        _controls.currentActionMap["Movement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Movement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
            
        _controls.currentActionMap["KeyMovement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["KeyMovement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
            
        _controls.currentActionMap["Rotate"].performed += ctx => VelocityCam(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Rotate"].canceled += ctx => VelocityCam(ctx.ReadValue<Vector2>());
        
        _controls.currentActionMap["RotateSun"].performed += ctx => RotateSun(ctx.ReadValue<float>());
        _controls.currentActionMap["RotateSun"].canceled += ctx => RotateSun(ctx.ReadValue<float>());
                
        
#if UNITY_EDITOR
#else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }

    public override void InputUpdate()
    {

        MouseCamera();
        MovePlayer();
        _controller._sun._gotoAngle += _gotoAngleVelocity * _controller._sun._maxRotateSpeed * Time.deltaTime;
    }

    public override void InputFixed() {}


    public override void MovePlayer()
    {
        _controller.velocity = Quaternion.Euler(0,_controller.cam.transform.eulerAngles.y,0) * (new Vector3(_move.x,0,_move.y) * _controller.speed);

        if (_controller.velocity.magnitude > 0)
        {
            _controller.transform.rotation = Quaternion.LookRotation(_controller.velocity);
        }
        else
        {
            _controller.velocity = Vector3.zero;
        }
        _controller.velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = _controller.velocity;
    }


 
    private void MouseCamera()
    {
        
        Vector3 camDir = Vector3.forward;
        _controller.cam.RotateMouse(new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")) * 0.8f);
    }
    
    
    // Input Manager
    public void Velocity(Vector2 readValue)
    {
        _move = new Vector2(readValue.x, readValue.y);
    }
    public void RotateSun(float angle)
    {
        _gotoAngleVelocity = angle;
    }
    public void VelocityCam(Vector2 velocity)
    {
        _controller.cam.Rotate(velocity);
    }

}
