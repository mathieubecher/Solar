using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Cinemachine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public AbstractInput inputs;
    private PlayerInput _controls;
    protected Rigidbody _rigidbody;

    public UIInterface UiInterface;
    // External
    [HideInInspector] public CameraController cam;

    // Infos
    [SerializeField] private GameObject MultiLocalPrefab;
    public float speed = 5f;
    [SerializeField] public Animator animator;
    [HideInInspector] public ControllerSun sun;
    [HideInInspector] public ControllerPuzzle puzzle;
    private CameraTarget _target;
    
    private Vector2 _moveCamera;
    private Vector2 _move;
    private bool isMoving = false;
    public Vector3 velocity;
    public GameObject poncho;

    [Header("Gestion Mort")]
    [Range(0,5)]
    public float DeadTimer = 2;
    
    private float _deadTimer;
    private float _respawnTimer;
    [SerializeField] private AnimationCurve _deadCurve;
    [SerializeField] private AnimationCurve _respawnCurve;
    
    [SerializeField] public bool activeDead = true;
    [SerializeField] private Image deadImg;
    public Vector3 Target {  get => _target.gameObject.transform.position;}



    void Awake()
    {
        // Active la mort quelque soit la valeur défini d'activeDead en dehors de l'editor.
#if !UNITY_EDITOR
        activeDead = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
        
        cam = FindObjectOfType<CameraController>();
        _target = FindObjectOfType<CameraTarget>();
        sun = GetComponent<ControllerSun>();
        puzzle = GetComponent<ControllerPuzzle>();
        
        FindObjectOfType<AnimEvent>().ResetBurn();
        _rigidbody = GetComponent<Rigidbody>();
        GameManager manager = FindObjectOfType<GameManager>();
        // Met en place les différents gestionnaires d'input en fonction des paramètres choisis par le joueur
        inputs = new Solo(this);
        if (manager.gameType == GameManager.GameType.SOLO)
        {
        }
        else if (manager.gameType == GameManager.GameType.LOCAL)
        {
            GetComponent<PlayerInput>().enabled = false;
            inputs = new Local(this);
            Instantiate(MultiLocalPrefab, Vector3.zero, Quaternion.identity);
        }
        else if (manager.gameType == GameManager.GameType.SERVER) NetworkManager.Instance.InstantiateController();
    }

    // Update is called once per frame
    void Update()
    {
        // Met à jour les variable par le gestionnaires d'input
        inputs.InputUpdate();

        // Si le joueur n'est pas mort
        if(_deadTimer <= 0)
        {
            if (_respawnTimer > 0)
            {
                _respawnTimer -= Time.deltaTime * 2;
                deadImg.color = new Color(deadImg.color.r,deadImg.color.g,deadImg.color.b,_respawnCurve.Evaluate((_respawnTimer)));
                
            }
            // Retours visuels et sonores lié aux déplacements du personnage
            animator.SetFloat("velocity", velocity.magnitude);
            // RESPIRATION
            
            //if (velocity.magnitude > 1f) AkSoundEngine.PostEvent("Cha_Run", this.gameObject); 
            if(sun.Life >= 1){
                if (velocity.magnitude > 1f) AkSoundEngine.PostEvent("Cha_Run", this.gameObject); 
                // else if(velocity.magnitude> 0.1f) AkSoundEngine.PostEvent("Cha_Walk", this.gameObject);
                else AkSoundEngine.PostEvent("Cha_IDLE", this.gameObject);
            }
            else AkSoundEngine.PostEvent("Cha_Hurt", this.gameObject);
            
            velocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = velocity;
        }
        else
        {
            // A la mort du personnage, laisse un temps défini avant le respawn
            _deadTimer -= Time.deltaTime;
            sun.ppeffect.Interpolate(Mathf.Max(0,_deadTimer - (DeadTimer-1)));
            if (_deadTimer < 2)
            {
                deadImg.color = new Color(deadImg.color.r,deadImg.color.g,deadImg.color.b,_deadCurve.Evaluate((2 - _deadTimer)/2));
            }
            else deadImg.color = new Color(deadImg.color.r,deadImg.color.g,deadImg.color.b,0);

            if (_deadTimer <= 0)
            {
                Respawn();
            }
        }
        
    }

    void FixedUpdate()
    {
        if (_deadTimer <= 0)
        {
            inputs.InputFixed();
        }
    }


    /// <summary>
    ///  Active la mort du joueur
    /// </summary>
    public void Dying()
    {
        // TODO Feedback death
        // si la mort est activé
        if(activeDead){
            
            AkSoundEngine.SetRTPCValue("RTPC_Distance_Sun", 0);
            AkSoundEngine.SetRTPCValue("RTPC_Sun_Velocity", 0);
            AkSoundEngine.PostEvent("Cha_Death_Play", this.gameObject);
            animator.SetBool("die", true);
            
            //animator.SetFloat("velocity", 0);
            _deadTimer = DeadTimer;
            velocity = Vector3.zero;
            _rigidbody.velocity = velocity;
            
            //sun.ResetPoints();
        }
    }
    
    /// <summary>
    ///  Active le respawn
    /// </summary>
    public void Respawn()
    {
        _respawnTimer = 1;
        AkSoundEngine.PostEvent("Cha_Respawn", this.gameObject);
        animator.SetBool("die", false);
        puzzle.Respawn();
        FindObjectOfType<AnimEvent>().ResetBurn();
    }
    
    /// <summary>
    ///  Active la mort du joueur
    /// </summary>
    public bool IsDead()
    {
        return _deadTimer > 0;   
    }
}
