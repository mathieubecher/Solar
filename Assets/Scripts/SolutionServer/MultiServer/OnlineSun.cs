using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.InputSystem;


public class OnlineSun: AbstractInput
{

    private InputManager _manager;
    private Vector3 _goto;

    /// <summary>
    /// Active la structure serveur.
    /// </summary>
    public override void SetManager(InputManager manager)
    {
        isManager = true;
        _manager = manager;
    }

    private Action<InputAction.CallbackContext> rotateSun;
    private Action<InputAction.CallbackContext> rotateStickSun;
    
    private Action<InputAction.CallbackContext> progressPlatform;
    private Action<InputAction.CallbackContext> progressStickPlatform;
    /// <summary>
    /// Met en place la structure serveur.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="manager"></param>
    public OnlineSun(Controller controller, InputManager manager) : base(controller)
    {
        
        rotateSun = ctx => RotateSun(ctx.ReadValue<float>());
        rotateStickSun = ctx => RotateStickSun(ctx.ReadValue<Vector2>());
        
        
        progressPlatform = ctx => ProgressPlatform(ctx.ReadValue<float>());
        progressStickPlatform = ctx => ProgressStickPlatform(ctx.ReadValue<Vector2>());
        
        _controls = controller.GetComponent<PlayerInput>();
        SetManager(manager);
        _goto = controller.transform.position;
        _controller.GetComponent<Rigidbody>().isKinematic = true;
        
        // Abonne la classe aux evennements d'InputSystem
        _controls.currentActionMap["RotateSun"].performed += rotateSun;
        _controls.currentActionMap["RotateSun"].canceled += rotateSun;
        
        _controls.currentActionMap["ProgressPlatform"].performed += progressPlatform;
        _controls.currentActionMap["ProgressPlatform"].canceled += progressPlatform;
        
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
        if(!_controller.IsDead()){
            MovePlayer();
            if (!_controller.UiInterface.gameObject.active)
            {
                _controller.sun._gotoAngle += _gotoAngleVelocity * _controller.sun._maxRotateSpeed * Time.deltaTime;
                if (isManager) _manager.CallSetSunRotate(_controller.sun._gotoAngle);
            }
        }
        _controller.sun.vibrate = _controls.currentControlScheme.Equals("Gamepad");
    }

    /// <summary>
    /// Déplacement du personnage.
    /// </summary>
    public override void MovePlayer()
    {
        if(isManager){
            _goto = _manager.position;
            _controller.transform.rotation = _manager.rotation;
        }

        if ((_goto - _controller.transform.position).magnitude > _controller.speed * Time.deltaTime && (_goto - _controller.transform.position).magnitude < 10)
        {
            Vector3 move = (_goto - _controller.transform.position).normalized * _controller.speed;
            _controller.transform.position += move * Time.deltaTime;
        }
        else
        {
            _controller.transform.position = _goto;
        }

        _controller.velocity = _manager.velocity;
    }

    /// <summary>
    /// Récupère la vélocité à appliquer au soleil.
    /// </summary>
    public void RotateSun(float angle)
    {
        _gotoAngleVelocity = angle  * _controller.UiInterface.player2Settings.sunSensitivity * ((_controller.UiInterface.player2Settings.invertSun)?-1:1);
    }
    
    /// <summary>
    ///  Déplacement des plateformes.
    /// </summary>
    private void ProgressPlatform(float readValue)
    {
        _controller.puzzle.cmActual.SetPlatformProgress(readValue * _controller.UiInterface.player2Settings.platformSensitivity);
        Debug.Log("platform");
    }
    public override void Dead()
    {
        _manager.CallSetPosition(_controller.transform.position);
        _manager.CallSetRotate(_controller.transform.rotation);
        _manager.CallSetVelocity(Vector3.zero);
        _manager.CallSetSunRotate(_controller.sun._gotoAngle);
        InputFixed();
    }

    public override bool CouldDie()
    {
        return false;
    }
    
    private UIInterface.Bind lastPlatform = UIInterface.Bind.L1R1;
    private UIInterface.Bind lastSun = UIInterface.Bind.L2R2;
    public override void BindPlatform(UIInterface.Bind bind)
    {
        if (lastPlatform != bind) BindPlatform(lastPlatform, bind);
        lastPlatform = bind;
    }

    public override void BindSun(UIInterface.Bind bind)
    {
        if (lastSun != bind) BindSun(lastSun, bind);
        lastSun = bind;
    }
    
    
    #region Biding Platform
    
    public void BindPlatform(UIInterface.Bind last, UIInterface.Bind bind)
    {
        ResetBind(last);
        if (bind == UIInterface.Bind.L1R1)
        {
            _controls.currentActionMap["ProgressPlatform"].performed += progressPlatform;
            _controls.currentActionMap["ProgressPlatform"].canceled += progressPlatform;
        }
        else if (bind == UIInterface.Bind.L2R2)
        {
            _controls.currentActionMap["RotateSun"].performed += progressPlatform;
            _controls.currentActionMap["RotateSun"].canceled += progressPlatform;
        }
        else if (bind == UIInterface.Bind.LeftStick)
        {
            _controls.currentActionMap["Movement"].performed += progressStickPlatform;
            _controls.currentActionMap["Movement"].canceled += progressStickPlatform;
        }
        else
        {
            _controls.currentActionMap["Rotate"].performed += progressStickPlatform;
            _controls.currentActionMap["Rotate"].canceled += progressStickPlatform;
        }
    }

    private void ProgressStickPlatform(Vector2 readValue)
    {
        _controller.puzzle.cmActual.SetPlatformProgress(readValue.y * _controller.UiInterface.player2Settings.platformSensitivity);
    }


    #endregion

    #region Biding Sun
    

    public void BindSun(UIInterface.Bind last, UIInterface.Bind bind)
    {
        ResetBind(last);
        if (bind == UIInterface.Bind.L1R1)
        {
            _controls.currentActionMap["ProgressPlatform"].performed += rotateSun;
            _controls.currentActionMap["ProgressPlatform"].canceled += rotateSun;
        }
        else if (bind == UIInterface.Bind.L2R2)
        {
            
            _controls.currentActionMap["RotateSun"].performed += rotateSun;
            _controls.currentActionMap["RotateSun"].canceled += rotateSun;
        }
        else if (bind == UIInterface.Bind.LeftStick)
        {
            _controls.currentActionMap["Movement"].performed += rotateStickSun;
            _controls.currentActionMap["Movement"].canceled += rotateStickSun;
        }
        else
        {
            _controls.currentActionMap["Rotate"].performed += rotateStickSun;
            _controls.currentActionMap["Rotate"].canceled += rotateStickSun;
        }
        
    }

    private void RotateStickSun(Vector2 readValue)
    {
        _gotoAngleVelocity = readValue.y  * _controller.UiInterface.player2Settings.sunSensitivity * ((_controller.UiInterface.player2Settings.invertSun)?-1:1);
    }

    #endregion
    
    private void ResetBind(UIInterface.Bind last)
    {
        
        if (last == UIInterface.Bind.L1R1)
        {
            _controls.currentActionMap["ProgressPlatform"].performed -= rotateSun;
            _controls.currentActionMap["ProgressPlatform"].canceled -= rotateSun;
            _controls.currentActionMap["ProgressPlatform"].performed -= progressPlatform;
            _controls.currentActionMap["ProgressPlatform"].canceled -= progressPlatform;
            
        }
        else if (last == UIInterface.Bind.L2R2)
        {
            _controls.currentActionMap["RotateSun"].performed -= rotateSun;
            _controls.currentActionMap["RotateSun"].canceled -= rotateSun;
            _controls.currentActionMap["RotateSun"].performed -= progressPlatform;
            _controls.currentActionMap["RotateSun"].canceled -= progressPlatform;
        }
        else if (last == UIInterface.Bind.LeftStick)
        {
            _controls.currentActionMap["Movement"].performed -= rotateStickSun;
            _controls.currentActionMap["Movement"].canceled -= rotateStickSun;
            _controls.currentActionMap["Movement"].performed -= progressStickPlatform;
            _controls.currentActionMap["Movement"].canceled -= progressStickPlatform;
        }
        else
        {
            _controls.currentActionMap["Rotate"].performed -= rotateStickSun;
            _controls.currentActionMap["Rotate"].canceled -= rotateStickSun;
            _controls.currentActionMap["Rotate"].performed -= progressStickPlatform;
            _controls.currentActionMap["Rotate"].canceled -= progressStickPlatform;
        }
    }

    public void UpdateSunSensitivity(float value)
    {
        
    }
}