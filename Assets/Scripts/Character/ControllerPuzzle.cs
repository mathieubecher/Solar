﻿using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ControllerPuzzle : MonoBehaviour
{
    public CMCamera cmActual;
    public CinemachineBrain brain;
    [SerializeField] private Puzzle _puzzle;

    private Controller _controller;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<Controller>();
        brain = FindObjectOfType<CinemachineBrain>();
        if(FindObjectOfType<GameManager>().gameType != GameManager.GameType.CLIENT) transform.position = _puzzle.GetRespawnPoint();
        cmActual = _puzzle.cam;
        cmActual.Enable(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Entre dans un puzzle
        if (other.gameObject.layer == 12)
        {
            _puzzle = other.gameObject.GetComponentInParent<Puzzle>();
            _puzzle.Enter(_controller.sun._gotoAngle);
            ChangeCam(_puzzle.cam);
        }
        // Transition de camera
        else if (other.gameObject.layer == 14)
        {
            CMTransition transition = other.gameObject.GetComponent<CMTransition>();
            if (other == transition.nextCollider) ChangeCam(transition._next);
            else ChangeCam(transition._previous);
        }
    }
    
    public void Dead()
    {
        _controller.sun.ResetRotate(_puzzle.beginRotate);
        ResetPlatform(_puzzle);
        transform.position = _puzzle.GetRespawnPoint();
        _controller.inputs.Dead();
        ChangeCam(_puzzle.cam);
        
        _controller.sun.ResetPoints();
    }

    private void ResetPlatform(Puzzle puzzle)
    {
        foreach (Platform p in puzzle.gameObject.GetComponentsInChildren<Platform>())
        {
            p.ResetProgress();
        }
    }

    public void ChangeCam(CMCamera cam)
    {
        cmActual.Disable();
        cmActual = cam;
        cam.Enable(this);
    }
}
