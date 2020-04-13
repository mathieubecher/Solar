using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
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
    [HideInInspector] public  CameraController _camera;
    
    // Infos
    public float speed = 5f;
    [SerializeField] public Animator animator;
    [HideInInspector] public  Rigidbody _rigidbody;
    [HideInInspector] public ControllerSun _sun;
    private CameraTarget _target;

    private Vector2 _moveCamera;
    private Vector2 _move;
    private bool isMoving = false;
    
    
    public Vector3 Target {  get => _target.gameObject.transform.position;}
    
    // Start is called before the first frame update
    void Awake()
    {
        
        _rigidbody = GetComponent<Rigidbody>();
        _camera = FindObjectOfType<CameraController>();
        _target = FindObjectOfType<CameraTarget>();
        _sun = GetComponent<ControllerSun>();
    }

    void Start()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        if (manager._gameType == GameManager.GameType.SOLO) inputs = new Solo(this);
        else
        {
            if(manager._gameType == GameManager.GameType.SERVER) NetworkManager.Instance.InstantiateController();
            inputs = new Solo(this);
            
        }

    }



    // Update is called once per frame
    void Update()
    {
        inputs.InputUpdate();
    }

    void FixedUpdate()
    {
        inputs.InputFixed();
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