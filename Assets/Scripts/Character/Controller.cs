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
    protected Rigidbody _rigidbody;
    // External
    [HideInInspector] public CameraController cam;

    // Infos
    public float speed = 5f;
    [SerializeField] public Animator animator;
    [HideInInspector] public ControllerSun sun;
    [HideInInspector] public ControllerPuzzle puzzle;
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
        sun = GetComponent<ControllerSun>();
        puzzle = GetComponent<ControllerPuzzle>();
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
        
        inputs.InputUpdate();
        animator.SetFloat("velocity", velocity.magnitude);
        if(_deatTimer <= 0){
            velocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = velocity;
            
            if(velocity.magnitude> 0.1f) AkSoundEngine.PostEvent("isWalking_Play", this.gameObject);
            else AkSoundEngine.PostEvent("isIDLE_Play", this.gameObject);
        }
        else
        {
            _deatTimer -= Time.deltaTime;
            if (_deatTimer <= 0) puzzle.Dead();
        }
        
    }

    void FixedUpdate()
    {
        if (_deatTimer <= 0)
        {
            inputs.InputFixed();
        }
    }



    public void Dying()
    {
        
        // TODO Feedback death
        _deatTimer = DeadTimer;
        velocity = Vector3.zero;
        _rigidbody.velocity = velocity;
        animator.SetFloat("velocity", 0);
        
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