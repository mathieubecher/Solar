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
    public List<Platform> platforms;
    [HideInInspector] public int id;
    void Awake()
    {
        
        gameObject.SetActive(false);
        foreach (Platform p in platforms)
        {
            p.enabled = false;
        }
        if (!follow) virtualCamera = GetComponent<CinemachineVirtualCamera>();
        else virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    public void SetPlatformProgress(float velocity)
    {
        foreach (Platform p in platforms)
        {
            p.SetProgress(velocity);
        }
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
        foreach (Platform p in platforms)
        {
            p.enabled = true;
        }
    }
    public void Disable()
    {
        _active = false;
        gameObject.SetActive(false);
        virtualCamera.Priority = 0;
        foreach (Platform p in platforms)
        {
            p.enabled = false;
        }
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
