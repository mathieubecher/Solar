using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.InputSystem;


public class OnlineSun: AbstractInput
{

    private InputManager _manager;
    private Vector3 _goto;

    public override void SetManager(InputManager manager)
    {
        isManager = true;
        _manager = manager;
    }
    public OnlineSun(Controller controller, InputManager manager) : base(controller)
    {
        SetManager(manager);
        _controls = controller.GetComponent<PlayerInput>();
        _goto = controller.transform.position;
        _controller.GetComponent<Rigidbody>().isKinematic = true;
        _controls.currentActionMap["RotateSun"].performed += ctx => RotateSun(ctx.ReadValue<float>());
        _controls.currentActionMap["RotateSun"].canceled += ctx => RotateSun(ctx.ReadValue<float>());
        if (GameObject.FindObjectOfType<GameManager>().gameType == GameManager.GameType.CLIENT)
        {
            _controller.transform.position = _manager.position;
            _controller.transform.rotation = _manager.rotation;
        }
    }
    
    public override void InputUpdate()
    {
        if(!_controller.IsDead()){
            MovePlayer();
            _controller.sun._gotoAngle += _gotoAngleVelocity * _controller.sun._maxRotateSpeed * Time.deltaTime;
            if(isManager) _manager.CallSetSunRotate(_controller.sun._gotoAngle);
        }
    }

    public override void MovePlayer()
    {
        // TODO
        if(isManager){
            _goto = _manager.position;
            _controller.transform.rotation = _manager.rotation;
        }

        if ((_goto - _controller.transform.position).magnitude > _controller.speed * Time.deltaTime)
        {
            Vector3 move = (_goto - _controller.transform.position).normalized * _controller.speed;
            _controller.transform.position += move * Time.deltaTime;
        }
        else
        {
            _controller.transform.position = _goto;
        }

        _controller.velocity = _manager.velocity;

    }

    public void RotateSun(float angle)
    {
        _gotoAngleVelocity = angle;
    }
}