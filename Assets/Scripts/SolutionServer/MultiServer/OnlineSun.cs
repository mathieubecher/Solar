﻿using BeardedManStudios.Forge.Networking;
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
    
    /// <summary>
    /// Met en place la structure serveur.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="manager"></param>
    public OnlineSun(Controller controller, InputManager manager) : base(controller)
    {
        _controls = controller.GetComponent<PlayerInput>();
        SetManager(manager);
        _goto = controller.transform.position;
        _controller.GetComponent<Rigidbody>().isKinematic = true;
        
        // Abonne la classe aux evennements d'InputSystem
        _controls.currentActionMap["RotateSun"].performed += ctx => RotateSun(ctx.ReadValue<float>());
        _controls.currentActionMap["RotateSun"].canceled += ctx => RotateSun(ctx.ReadValue<float>());
        
        _controls.currentActionMap["ProgressPlatform"].performed += ctx => ProgressPlatform(ctx.ReadValue<float>());
        _controls.currentActionMap["ProgressPlatform"].canceled += ctx => ProgressPlatform(ctx.ReadValue<float>());
        
        // Défini la position et rotation de départ du personnage
        if (GameObject.FindObjectOfType<GameManager>().gameType == GameManager.GameType.CLIENT)
        {
            _controller.transform.position = _manager.position;
            _controller.transform.rotation = _manager.rotation;
        }
        else _manager.CallSetPosition(_controller.transform.position);
    }
    
    /// <summary>
    /// Mise à jour des variables du personnage lors d'Update.
    /// </summary>
    public override void InputUpdate()
    {
        if(!_controller.IsDead()){
            MovePlayer();
            _controller.sun._gotoAngle += _gotoAngleVelocity * _controller.sun._maxRotateSpeed * Time.deltaTime;
            if(isManager) _manager.CallSetSunRotate(_controller.sun._gotoAngle);
        }
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

        if ((_goto - _controller.transform.position).magnitude > _controller.speed * Time.deltaTime)
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
        _gotoAngleVelocity = angle;
    }
    
    /// <summary>
    ///  Déplacement des plateformes.
    /// </summary>
    private void ProgressPlatform(float readValue)
    {
        _controller.puzzle.cmActual.SetPlatformProgress(readValue);
    }
}