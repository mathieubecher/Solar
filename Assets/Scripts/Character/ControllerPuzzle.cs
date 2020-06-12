using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ControllerPuzzle : MonoBehaviour
{
    public struct InfosTransition
    {
        public float time;
        public CinemachineBlendDefinition.Style style;
        public CMCamera next;

        public InfosTransition(CMCamera next)
        {
            this.next = next;
            time = 1;
            style = CinemachineBlendDefinition.Style.EaseInOut;
        }

        public InfosTransition(CMCamera _next, CinemachineBlendDefinition.Style _style, float _time)
        {
            this.next = _next;
            this.time = _time;
            this.style = _style;
        }
    }
    
    public CMCamera cmActual;
    public CinemachineBrain brain;
    [SerializeField] private Puzzle _puzzle;

    private InfosTransition _infosTransition;
    
    private float _timer_CMnext = 0;
    
    private Controller _controller;
    public SphereController sphere;
    
    void Start()
    {
        _controller = GetComponent<Controller>();
        //brain = FindObjectOfType<CinemachineBrain>();
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
                //Debug.Log("change Cam");
                ChangeCam(_infosTransition);
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
            ChangeCam(new InfosTransition(_puzzle.cam, _puzzle.type, _puzzle.transitionTime));
            AkSoundEngine.PostEvent("Checkpoint_Found", gameObject);

        }
        // Transition de camera
        else if (other.gameObject.layer == 14)
        {
            CMTransition transition = other.gameObject.GetComponent<CMTransition>();
            if (other == transition.nextCollider) DecideChangeCam(transition);
            else DecideChangeCam(transition,false);
            
            
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
        ChangeCam(new InfosTransition(_puzzle.cam),true);
        
        _controller.sun.ResetPoints();
        _controller.poncho.GetComponent<Cloth>().ClearTransformMotion();
        
        _controller.inputs.Dead();
        
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
    public void ChangeCam(InfosTransition transition, bool respawn = false)
    {
        if(respawn) brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0);
        else
        {
            brain.m_DefaultBlend = new CinemachineBlendDefinition(transition.style, transition.time);
        }
        cmActual.Disable();
        cmActual = transition.next;
        transition.next.Enable(this);
        sphere.CMCam = transition.next.transform;
        
    }

    public void DecideChangeCam(CMTransition transition, bool next=true)
    {
        //Debug.Log("define Cam");
        _timer_CMnext = 1f;
        _infosTransition = new InfosTransition((next)?transition.next:transition.previous, transition.type, transition.transitionTime);
        //sphere.CMCam = cam.transform;
    }
}
