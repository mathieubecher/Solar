using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnlinePlayer : AbstractInput
{
    protected InputManager _manager;
    private bool isMoving;
    
    public override void SetManager(InputManager manager)
    {
        isManager = true;
        _manager = manager;
    }
    public OnlinePlayer(Controller controller, InputManager manager) : base(controller)
    {
        
        _controls = controller.GetComponent<PlayerInput>();
        SetManager(manager);
        _controls.currentActionMap["Movement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Movement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
        
        _controls.currentActionMap["KeyMovement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["KeyMovement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
        
        _controls.currentActionMap["Rotate"].performed += ctx => _controller._camera.Rotate(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Rotate"].canceled += ctx => _controller._camera.Rotate(ctx.ReadValue<Vector2>());
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
        if(isManager) _controller._sun._gotoAngle = _manager.sunRotation;

    }



    public override void InputFixed()
    {
        if (isMoving)
        {
            isMoving = false;
            // TODO
            if (isManager)
            {
                _manager.CallSetPosition(_controller.transform.position);
                _manager.CallSetRotate(_controller.transform.rotation);
            }
        }
    }
    public override void MovePlayer()
    {
        Vector3 velocity = Quaternion.Euler(0,_controller._camera.transform.eulerAngles.y,0) * (new Vector3(_move.x,0,_move.y) * _controller.speed);

        if (velocity.magnitude > 0)
        {
            _controller.transform.rotation = Quaternion.LookRotation(velocity);
            isMoving = true;
        }
        else
        {
            velocity = Vector3.zero;
        }
        _controller.animator.SetFloat("velocity",velocity.magnitude);
        velocity.y = _controller._rigidbody.velocity.y;
        _controller._rigidbody.velocity = velocity;
        _manager.CallSetVelocity(velocity);
    }
    
    private void MouseCamera()
    {
    
        Vector3 camDir = Vector3.forward;
        _controller._camera.RotateMouse(new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")) * 0.8f);
    }
    
    
    public void Velocity(Vector2 readValue)
    {
        _move = new Vector2(readValue.x, readValue.y);
    }
}
