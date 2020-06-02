using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnlinePlayer : AbstractInput
{
    protected InputManager _manager;
    private bool isMoving;
    
    /// <summary>
    /// Active la structure serveur
    /// </summary>
    public override void SetManager(InputManager manager)
    {
        isManager = true;
        _manager = manager;
    }
    
    /// <summary>
    /// Met en place la structure serveur.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="manager"></param>
    public OnlinePlayer(Controller controller, InputManager manager) : base(controller)
    {
        
        _controls = controller.GetComponent<PlayerInput>();
        SetManager(manager);
        
        // Abonne la classe aux evennements d'InputSystem
        _controls.currentActionMap["Movement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Movement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
        
        _controls.currentActionMap["KeyMovement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["KeyMovement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
        
        _controls.currentActionMap["Rotate"].performed += ctx => _controller.cam.Rotate(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Rotate"].canceled += ctx => _controller.cam.Rotate(ctx.ReadValue<Vector2>());
        
        // Défini la position et rotation de départ du personnage
        if (GameObject.FindObjectOfType<GameManager>().gameType == GameManager.GameType.CLIENT)
        {
            _controller.transform.position = _manager.position;
            _controller.transform.rotation = _manager.rotation;
            _controller.sun.ResetRotate(_manager.sunRotation);

        }
        else
        {
            _manager.CallSetPosition(_controller.transform.position);
            _manager.CallSetSunRotate(_controller.sun._gotoAngle);
        }
        
#if UNITY_EDITOR
#else
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
#endif
    }
    
    
    /// <summary>
    /// Mise à jour des variables du personnage lors d'Update.
    /// </summary>
    public override void InputUpdate()
    {
        MouseCamera();
        if (!_controller.IsDead())
        {
            MovePlayer();
        }
        if(isManager) _controller.sun._gotoAngle = _manager.sunRotation;
    }



    /// <summary>
    /// Mise à jour des variables du personnage lors de FixedUpdate.
    /// </summary>
    public override void InputFixed()
    {
        if (isMoving)
        {
            isMoving = false;
            // TODO
            if (isManager)
            {
                _manager.CallSetPosition(_controller.transform.position);
                _manager.CallSetRotate(_controller.transform.rotation);
            }
        }
    }
    
    /// <summary>
    /// Déplacement du personnage.
    /// </summary>
    public override void MovePlayer()
    {
        _controller.velocity = Quaternion.Euler(0,_controller.cam.transform.eulerAngles.y,0) * (new Vector3(_move.x,0,_move.y) * _controller.speed);

        if (_controller.velocity.magnitude > 0)
        {
            _controller.transform.rotation = Quaternion.LookRotation(_controller.velocity);
            isMoving = true;
        }
        else
        {
            _controller.velocity = Vector3.zero;
        }
        _manager.CallSetVelocity(_controller.velocity);
    }
    
    /// <summary>
    /// Récupère la vélocité à appliquer à la caméra.
    /// </summary>
    private void MouseCamera()
    {
        Vector3 camDir = Vector3.forward;
        _controller.cam.RotateMouse(new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")) * 0.8f);
    }
    
    /// <summary>
    /// Récupère la vélocité à appliqué au personnage.
    /// </summary>
    /// <param name="readValue"></param>
    public void Velocity(Vector2 readValue)
    {
        _move = new Vector2(readValue.x, readValue.y);
    }
    
    /// <summary>
    /// Comportement du système à la mort du personnage.
    /// </summary>
    public override void Dead()
    {
        // force la mise à jour des données à l'autre joueur
        isMoving = true;
        _manager.CallSetPosition(_controller.transform.position);
        _manager.CallSetRotate(_controller.transform.rotation);
        _manager.CallSetVelocity(Vector3.zero);
        _manager.CallSetSunRotate(_controller.sun._gotoAngle);
        InputFixed();
    }
    
    public override bool CouldDie()
    {
        _manager.CallDie();
        return true;
    }
}
