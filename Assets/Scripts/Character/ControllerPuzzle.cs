using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ControllerPuzzle : MonoBehaviour
{
    public CMCamera cmActual;
    public CinemachineBrain brain;
    [SerializeField] private Puzzle _puzzle;

    private CMCamera _CMnext;
    private float _timer_CMnext = 0;
    
    private Controller _controller;
    
    void Start()
    {
        _controller = GetComponent<Controller>();
        brain = FindObjectOfType<CinemachineBrain>();
        // Place le personnage au niveau du respawn du premier puzzle
        if (FindObjectOfType<GameManager>().gameType != GameManager.GameType.CLIENT)
        {
            transform.position = _puzzle.GetRespawnPoint();
            _controller.sun.ResetRotate(_puzzle.beginRotate);
        }
        cmActual = _puzzle.cam;
        cmActual.Enable(this);
    }

    void Update()
    {
        if (_timer_CMnext > 0)
        {
            _timer_CMnext -= Time.deltaTime;
            if (_timer_CMnext <= 0)
            {
                Debug.Log("change Cam");
                ChangeCam(_CMnext);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Entre dans un puzzle
        if (other.gameObject.layer == 12 && _puzzle != other.gameObject.GetComponentInParent<Puzzle>())
        {
            _puzzle = other.gameObject.GetComponentInParent<Puzzle>();
            _puzzle.Enter(_controller.sun._gotoAngle);
            ChangeCam(_puzzle.cam);
        }
        // Transition de camera
        else if (other.gameObject.layer == 14)
        {
            CMTransition transition = other.gameObject.GetComponent<CMTransition>();
            if (other == transition.nextCollider) DecideChangeCam(transition._next);
            else DecideChangeCam(transition._previous);
        }
    }
    
    /// <summary>
    /// Réinitialise position rotation et camera du joueur en fonction du puzzle actif.
    /// </summary>
    public void Respawn()
    {
        _controller.sun.ResetRotate(_puzzle.beginRotate);
        ResetPlatform(_puzzle);
        transform.position = _puzzle.GetRespawnPoint();
        _controller.inputs.Dead();
        ChangeCam(_puzzle.cam);
        
        _controller.sun.ResetPoints();
    }
    
    /// <summary>
    /// Réinitialise la position des plateformes du puzzles
    /// </summary>
    /// <param name="puzzle"></param>
    private void ResetPlatform(Puzzle puzzle)
    {
        foreach (Platform p in puzzle.gameObject.GetComponentsInChildren<Platform>())
        {
            p.ResetProgress();
        }
    }

    /// <summary>
    /// Met à jour la caméra du joueur 2
    /// </summary>
    /// <param name="cam"></param>
    public void ChangeCam(CMCamera cam)
    {
        cmActual.Disable();
        cmActual = cam;
        cam.Enable(this);
    }

    public void DecideChangeCam(CMCamera cam)
    {
        _timer_CMnext = 1f;
        _CMnext = cam;
    }
}
