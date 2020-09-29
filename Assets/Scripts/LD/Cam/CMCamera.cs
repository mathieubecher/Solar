using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Camera Cinemachine
/// </summary>
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
        
        gameObject.SetActive(true);
        foreach (Platform p in platforms)
        {
            p.enabled = false;
        }
        if (!follow) virtualCamera = GetComponent<CinemachineVirtualCamera>();
        else virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    /// <summary>
    /// Met à jour les plateformes en fonction d'une vélocité précise.
    /// </summary>
    /// <param name="velocity">Vélocité à appliquer</param>
    public void SetPlatformProgress(float velocity)
    {
        Debug.Log(velocity);
        foreach (Platform p in platforms)
        {
            p.SetProgress(velocity);
        }
    }
    
    void Start()
    {
        id = ++ID;
    }
    
    /// <summary>
    /// Active la caméra.
    /// </summary>
    /// <param name="c">le controller en charge de la transition des puzzles</param>
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
    /// <summary>
    /// Désactive la caméra.
    /// </summary>
    public void Disable()
    {
        //_active = false;
        //gameObject.SetActive(false);
        virtualCamera.Priority = 0;
        
        foreach (Platform p in platforms)
        {
            p.enabled = false;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position,0.5f);
    }
#endif
}
