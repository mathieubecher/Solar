using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float speed = 10;
    private Rigidbody _rigidbody;
    private List<Point> _points;
    private CameraTarget _target;

    private Vector2 _moveCamera;
    private Vector3 _move;
    
    
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


    void Start()
    {
        _sun = FindObjectOfType<LightController>();
        _controls = GetComponent<PlayerInput>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
        _rigidbody.velocity = _move * speed;
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

}
