using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.InputSystem;


public class OnlineSun: AbstractInput
{

    private InputManager _manager;

    public override void SetManager(InputManager manager)
    {
        isManager = true;
        _manager = manager;
    }
    public OnlineSun(Controller controller, InputManager manager) : base(controller)
    {
        SetManager(manager);
        _controls = controller.GetComponent<PlayerInput>();
        _controller._rigidbody.isKinematic = true;
        _controls.currentActionMap["RotateSun"].performed += ctx => RotateSun(ctx.ReadValue<float>());
        _controls.currentActionMap["RotateSun"].canceled += ctx => RotateSun(ctx.ReadValue<float>());
    }
    
    public override void InputUpdate()
    {
        MovePlayer();
        _controller._sun._gotoAngle += _gotoAngleVelocity * _controller._sun._maxRotateSpeed * Time.deltaTime;
        if(isManager) _manager.CallSetSunRotate(_controller._sun._gotoAngle);
        //networkObject.SendRpc(RPC_SET_ROTATE, Receivers.Server, _controller._sun._gotoAngle);
    }

    public override void MovePlayer()
    {
        Vector3 lastpos = _controller.transform.position;
        // TODO
        if(isManager){
            _controller.transform.position = _manager.position;
            _controller.transform.rotation = _manager.rotation;
        }

        _controller.animator.SetFloat("velocity", (lastpos - _controller.transform.position).magnitude > 0.01f ? 1 : 0);
    }

    public void RotateSun(float angle)
    {
        _gotoAngleVelocity = angle;
    }
}