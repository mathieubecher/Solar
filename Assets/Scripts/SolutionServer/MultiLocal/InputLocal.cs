using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputLocal : MonoBehaviour
{
    private enum InputType
    {
        PLAYER,SUN
    }

    private Controller _controller;
    private PlayerInput _controls;
    [SerializeField]
    private Vector2 _move;
    [SerializeField]
    private Vector3 _rotateMouse;
    [SerializeField]
    private Vector2 _rotate;
    [SerializeField]
    private float _gotoAngleVelocity;
    [SerializeField]
    private InputType _type;

    public Vector2 Move => _move;
    public Vector3 RotateMouse => _rotateMouse;
    public Vector2 Rotate => _rotate;
    public float AngleVelocity => _gotoAngleVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _controller = FindObjectOfType<Controller>();
        Local inputs = (Local)FindObjectOfType<Controller>().inputs;
        _type = (inputs.SetInput(this) == 0)? InputType.PLAYER:InputType.SUN;
        
        _controls = GetComponent<PlayerInput>();
        Debug.Log(_controls.currentControlScheme);
        
        _controls.currentActionMap["Movement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Movement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
            
        _controls.currentActionMap["KeyMovement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["KeyMovement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
            
        _controls.currentActionMap["Rotate"].performed += ctx => VelocityCam(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Rotate"].canceled += ctx => VelocityCam(ctx.ReadValue<Vector2>());
        
        _controls.currentActionMap["RotateSun"].performed += ctx => RotateSun(ctx.ReadValue<float>());
        _controls.currentActionMap["RotateSun"].canceled += ctx => RotateSun(ctx.ReadValue<float>());
        
        _controls.currentActionMap["ProgressPlatform"].performed += ctx => ProgressPlatform(ctx.ReadValue<float>());
        _controls.currentActionMap["ProgressPlatform"].canceled += ctx => ProgressPlatform(ctx.ReadValue<float>());
    }

    public void Update()
    {
        if(_type == InputType.PLAYER && _controls.currentControlScheme.Equals("Keyboard and mouse")) MouseCamera();
    }

    private void MouseCamera()
    {
        _rotate = new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")) * 0.8f;
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
        _rotateMouse = velocity;
    }
    private void ProgressPlatform(float readValue)
    {
        _controller.puzzle.cmActual.SetPlatformProgress(readValue);
    }
}
