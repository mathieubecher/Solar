using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CMCamera : MonoBehaviour
{
    [HideInInspector] public CinemachineVirtualCamera virtualCamera;
    private bool _active; 
    private ControllerPuzzle _controller;
    private static int ID = 0;
    [SerializeField] private bool follow;
    public int id;
    void Awake()
    {
        
        gameObject.SetActive(false);
        if (!follow) virtualCamera = GetComponent<CinemachineVirtualCamera>();
        else virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    void Start()
    {
        id = ++ID;
    }
    public void Enable(ControllerPuzzle c)
    {
        gameObject.SetActive(true);
        virtualCamera.Priority = 10;
        _controller = c;
        _active = true;
    }
    public void Disable()
    {
        _active = false;
        gameObject.SetActive(false);
        virtualCamera.Priority = 0;
    }

    void Update()
    {
        
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }
}
