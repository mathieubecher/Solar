using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Cinemachine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public AbstractInput inputs;
    private PlayerInput _controls;
    // External
    [HideInInspector] public  CameraController cam;
    public  CinemachineBrain brain;

    [SerializeField] private Puzzle _puzzle;
    // Infos
    public float speed = 5f;
    [SerializeField] public Animator animator;
    [HideInInspector] public ControllerSun _sun;
    private CameraTarget _target;

    private Vector2 _moveCamera;
    private Vector2 _move;
    private bool isMoving = false;
    public Vector3 velocity;

    [Range(0,5)]
    public float DeadTimer = 2;
    [SerializeField]
    private float _deatTimer;
    
    public Vector3 Target {  get => _target.gameObject.transform.position;}
    
    // Start is called before the first frame update
    void Awake()
    {
        cam = FindObjectOfType<CameraController>();
        _target = FindObjectOfType<CameraTarget>();
        _sun = GetComponent<ControllerSun>();
    }

    void Start()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        if (manager.gameType == GameManager.GameType.SOLO) inputs = new Solo(this);
        else
        {
            if(manager.gameType == GameManager.GameType.SERVER) NetworkManager.Instance.InstantiateController();
            inputs = new Solo(this);
            
        }
        

    }



    // Update is called once per frame
    void Update()
    {
        if(_deatTimer <= 0){
            inputs.InputUpdate();
            animator.SetFloat("velocity", velocity.magnitude);
            if(velocity.magnitude> 0.1f) AkSoundEngine.PostEvent("isWalking_Play", this.gameObject);
            else AkSoundEngine.PostEvent("isIDLE_Play", this.gameObject);
        }
        else
        {
            _deatTimer -= Time.deltaTime;
            if (_deatTimer <= 0) Dead();
        }
    }

    void FixedUpdate()
    {
        if (_deatTimer <= 0)
        {
            inputs.InputFixed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            _puzzle = other.gameObject.GetComponentInParent<Puzzle>();
            _puzzle.Enter(_sun._gotoAngle);
            brain.ActiveVirtualCamera.Priority = 0;
            _puzzle.cam.Priority = 10;
        }
    }

    public void Dying()
    {
        // TODO Feedback death
        _deatTimer = DeadTimer;
        velocity = Vector3.zero;
        animator.SetFloat("velocity", 0);
        
    }
    public void Dead()
    {
        _sun.ResetRotate(_puzzle.beginRotate);
        transform.position = _puzzle.GetRespawnPoint();
        brain.ActiveVirtualCamera.Priority = 0;
        _puzzle.cam.Priority = 10;
        _sun.ResetPoints();
    }

    public bool IsDead()
    {
        return _deatTimer > 0;
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