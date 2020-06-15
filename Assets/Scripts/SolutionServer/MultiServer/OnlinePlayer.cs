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
        
        _controls.currentActionMap["Rotate"].performed += ctx => VelocityCam(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Rotate"].canceled += ctx => VelocityCam(ctx.ReadValue<Vector2>());
        
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
        
    }
    
    
    /// <summary>
    /// Mise à jour des variables du personnage lors d'Update.
    /// </summary>
    public override void InputUpdate()
    {
        if (!_controller.UiInterface.gameObject.active)
        {
            MouseCamera();
            if (!_controller.IsDead())
            {
                MovePlayer();
            }
        }
        else
        {
            _controller.velocity = Vector3.zero;
            _controller.cam.RotateMouse(Vector3.zero);
            _controller.cam.Rotate(Vector3.zero);
        }

        if(isManager) _controller.sun._gotoAngle = _manager.sunRotation;
        _controller.sun.vibrate = _controls.currentControlScheme.Equals("Gamepad");
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
        _controller.cam.RotateMouse(new Vector3(Input.GetAxis("Mouse X") * _controller.UiInterface.player1Settings.xAxisSensitivity, 
                                        Input.GetAxis("Mouse Y") * _controller.UiInterface.player1Settings.yAxisSensitivity * ((_controller.UiInterface.player1Settings.invertVertical)?-1:1)) * 0.8f);
    }
    /// <summary>
    /// Récupère la vélocité à appliquer à la caméra
    /// </summary>
    /// <param name="velocity">valeur envoyé par InputSystem</param>
    public void VelocityCam(Vector2 velocity)
    {
        _controller.cam.Rotate(new Vector2(velocity.x * _controller.UiInterface.player1Settings.xAxisSensitivity, 
            velocity.y  * _controller.UiInterface.player1Settings.yAxisSensitivity * ((_controller.UiInterface.player1Settings.invertVertical)?-1:1)));
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
