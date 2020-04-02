using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerSun : MonoBehaviour
{
    [Header("Sun control")]
    private LightController _sun;
    [SerializeField, Range(0,100)] private float _maxRotateSpeed = 100f;
    private float _rotateSpeed;
    [SerializeField, Range(0,100)] private float _speedVelocity = 5;
    private float _rotateInput;
    private float _rotateWheel;
    private bool _directionWheel;
    
    
    [Header("Player Reaction with Sun")]
    private float _life = 1;
    public float Life => _life;
    [SerializeField] private List<Point> _points;
    [SerializeField] private Gradient fx;
    [SerializeField] private AnimationCurve _pulsate;
    private float _pulsateSpeed = 2;
    [SerializeField] private Image _fxUI;

    
    
    // Start is called before the first frame update
    void Awake()
    {
        _sun = FindObjectOfType<LightController>();
        _points = new List<Point>();
        GetPoints(this.gameObject);
    }

    // Update is called once per frame
    private float _time;
    void Update()
    {
        
        _rotateSpeed = _rotateInput * _maxRotateSpeed;
        
        Vector3 sunEuler = _sun.transform.eulerAngles;
        sunEuler.y += _rotateSpeed * Time.deltaTime;
        _sun.transform.eulerAngles = sunEuler;
        
        SetLife();
        _time = (_time + Time.deltaTime * _pulsateSpeed*(1-_life))%1;
        _fxUI.color = fx.Evaluate(1-_life) * new Color(1,1,1,0.8f + _pulsate.Evaluate(_time) * 0.2f);
    }
    

    private void SetLife()
    {
        _life = 1;
        foreach (Point p in _points)
        {
            _life -= p.TestLight(_sun)/_points.Count;
        }
    }
    
    private void GetPoints(GameObject obj)
    {
        
        if(obj.TryGetComponent<Point>(out var p)) _points.Add(p);
        for (int i = 0; i < obj.transform.childCount; ++i)
        {
            GetPoints(obj.transform.GetChild(i).gameObject);
        }
    }

    public void Rotate(float rotate)
    {
        _rotateInput = rotate;
    }
}
