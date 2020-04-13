using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BeardedManStudios.Forge.Networking.Generated;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Controller : ControllerBehavior
{
    private PlayerInput _controls;
    // External
    private CameraController _camera;
    
    // Infos
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator animator;
    private Rigidbody _rigidbody;
    [HideInInspector] public ControllerSun _sun;
    private CameraTarget _target;

    private Vector2 _moveCamera;
    private Vector2 _move;
    private bool isMoving = false;
    
    
    public Vector3 Target {  get => _target.gameObject.transform.position;}
    
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = FindObjectOfType<CameraController>();
        _target = FindObjectOfType<CameraTarget>();
        _sun = GetComponent<ControllerSun>();
    }

    private Vector3 mousePos;

    void Start()
    {
        
        _controls = GetComponent<PlayerInput>();
        if(networkObject.IsServer){
            _controls.currentActionMap["Movement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
            _controls.currentActionMap["Movement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
            
            _controls.currentActionMap["KeyMovement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
            _controls.currentActionMap["KeyMovement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
            
            _controls.currentActionMap["Rotate"].performed += ctx => _camera.Rotate(ctx.ReadValue<Vector2>());
            _controls.currentActionMap["Rotate"].canceled += ctx => _camera.Rotate(ctx.ReadValue<Vector2>());
            return;
        }
        _controls.currentActionMap["RotateSun"].performed += ctx => _sun.Rotate(ctx.ReadValue<float>());
        _controls.currentActionMap["RotateSun"].canceled += ctx => _sun.Rotate(ctx.ReadValue<float>());
        
#if UNITY_EDITOR
#else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
        mousePos = Input.mousePosition;
    }



    // Update is called once per frame
    void Update()
    {
        if (!networkObject.IsServer)
        {
            Vector3 lastpos = transform.position;
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
            if((lastpos - transform.position).magnitude > 0.01f) animator.SetFloat("velocity",(lastpos - transform.position).magnitude);
            else animator.SetFloat("velocity",0);
            return;
        }
        MovePlayer();
    }
    
    private void Velocity(Vector2 readValue)
    {
        _move = new Vector2(readValue.x, readValue.y);
    }

    private void MovePlayer()
    {
        Vector3 camDir = Vector3.forward;
        _camera.RotateMouse(new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")) * 0.8f);
        Vector3 velocity = Quaternion.Euler(0,_camera.transform.eulerAngles.y,0) * (new Vector3(_move.x,0,_move.y) * speed);

        if (velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
            isMoving = true;
        }
        else
        {
            velocity = Vector3.zero;
        }
        animator.SetFloat("velocity",velocity.magnitude);
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
        
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            isMoving = false;
            networkObject.position = transform.position;
            networkObject.rotation = transform.rotation;
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(Controller))]
internal class ControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var controller = (Controller) target;
    }
}
#endif