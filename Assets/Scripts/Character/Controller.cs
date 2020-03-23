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
    private LightController _sun;
    private CameraController _camera;
    
    // Infos
    [SerializeField] private float speed = 0.2f;
    private Rigidbody _rigidbody;
    [SerializeField] private List<Point> _points;
    private CameraTarget _target;

    private Vector2 _moveCamera;
    private Vector2 _move;
    
    
    public Vector3 Target {  get => _target.gameObject.transform.position;}

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = FindObjectOfType<CameraController>();
        
        _points = new List<Point>();
        GetPoints(this.gameObject);
        
    }

    void OnEnable()
    {
        //Bind input
    }
    private Vector3 mousePos;

    void Start()
    {
        _sun = FindObjectOfType<LightController>();
        _controls = GetComponent<PlayerInput>();
        _controls.currentActionMap["Movement"].performed += ctx => Move(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Movement"].canceled += ctx => Move(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["KeyMovement"].performed += ctx => Move(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["KeyMovement"].canceled += ctx => Move(ctx.ReadValue<Vector2>());
        
        _controls.currentActionMap["Rotate"].performed += ctx => _camera.Rotate(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Rotate"].canceled += ctx => _camera.Rotate(ctx.ReadValue<Vector2>());
        Cursor.lockState = CursorLockMode.Locked;
        mousePos = Input.mousePosition;
        Cursor.visible = false;
    }



    // Update is called once per frame
    void Update()
    {
        
        Vector3 velocity = Quaternion.Euler(0,_camera.transform.eulerAngles.y,0) * (new Vector3(_move.x,0,_move.y) * speed);
        Vector3 camDir = Vector3.forward;
        
        _camera.RotateMouse(new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")) * 0.8f);
        
        
        if(velocity.magnitude>0) transform.rotation = Quaternion.LookRotation(velocity);
        
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
        
        foreach (Point p in _points)
        {
            p.TestLight(_sun);
        }
        
        
    }
    
    
    private void GetPoints(GameObject obj)
    {
        
        if(obj.TryGetComponent<Point>(out var p)) _points.Add(p);
        if (obj.TryGetComponent<CameraTarget>(out var ct)) _target = ct;
        for (int i = 0; i < obj.transform.childCount; ++i)
        {
            GetPoints(obj.transform.GetChild(i).gameObject);
        }
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