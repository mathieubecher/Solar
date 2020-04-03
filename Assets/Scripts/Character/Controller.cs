using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Controller : MonoBehaviour
{
    private PlayerInput _controls;
    // External
    private CameraController _camera;
    
    // Infos
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator animator;
    private Rigidbody _rigidbody;
    private ControllerSun _sun;
    private CameraTarget _target;

    private Vector2 _moveCamera;
    private Vector2 _move;
    
    
    
    public Vector3 Target {  get => _target.gameObject.transform.position;}
    
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = FindObjectOfType<CameraController>();
        _target = FindObjectOfType<CameraTarget>();
        _sun = GetComponent<ControllerSun>();


    }

    void OnEnable()
    {
        //Bind input
    }
    private Vector3 mousePos;

    void Start()
    {
        
        _controls = GetComponent<PlayerInput>();
        _controls.currentActionMap["Movement"].performed += ctx => Move(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Movement"].canceled += ctx => Move(ctx.ReadValue<Vector2>());
        
        _controls.currentActionMap["KeyMovement"].performed += ctx => Move(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["KeyMovement"].canceled += ctx => Move(ctx.ReadValue<Vector2>());
        
        _controls.currentActionMap["RotateSun"].performed += ctx => _sun.Rotate(ctx.ReadValue<float>());
        _controls.currentActionMap["RotateSun"].canceled += ctx => _sun.Rotate(ctx.ReadValue<float>());
        
        _controls.currentActionMap["Rotate"].performed += ctx => _camera.Rotate(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Rotate"].canceled += ctx => _camera.Rotate(ctx.ReadValue<Vector2>());
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
        
        Vector3 velocity = Quaternion.Euler(0,_camera.transform.eulerAngles.y,0) * (new Vector3(_move.x,0,_move.y) * speed);
        Vector3 camDir = Vector3.forward;
        
        _camera.RotateMouse(new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")) * 0.8f);

        if(velocity.magnitude>0) transform.rotation = Quaternion.LookRotation(velocity);
        animator.SetFloat("velocity",velocity.magnitude);
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;

    }
    private void Move(Vector2 readValue)
    {
        _move = new Vector2(readValue.x, readValue.y);
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