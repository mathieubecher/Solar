using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Solo : AbstractInput
{
    

    public Solo(Controller controller) : base(controller)
    {
        
        // Récupère le controlleur du personnage
        _controller = controller;
        _controls = _controller.GetComponent<PlayerInput>();
        
        // Abonne la classe aux evennements d'InputSystem
        _controls.currentActionMap["Movement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Movement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
            
        _controls.currentActionMap["KeyMovement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["KeyMovement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
            
        _controls.currentActionMap["Rotate"].performed += ctx => VelocityCam(ctx.ReadValue<Vector2>());
        _controls.currentActionMap["Rotate"].canceled += ctx => VelocityCam(ctx.ReadValue<Vector2>());
        
        _controls.currentActionMap["RotateSun"].performed += ctx => RotateSun(ctx.ReadValue<float>());
        _controls.currentActionMap["RotateSun"].canceled += ctx => RotateSun(ctx.ReadValue<float>());
        
        _controls.currentActionMap["ProgressPlatform"].performed += ctx => ProgressPlatform(ctx.ReadValue<float>());
        _controls.currentActionMap["ProgressPlatform"].canceled += ctx => ProgressPlatform(ctx.ReadValue<float>());
                
        

    }

    /// <summary>
    /// Récupère la vélocité à appliquer à la plateforme
    /// </summary>
    /// <param name="readValue">valeur envoyé par InputSystem</param>
    private void ProgressPlatform(float readValue)
    {
        _controller.puzzle.cmActual.SetPlatformProgress(readValue * _controller.options.player2Settings.platformSensitivity);
    }

    /// <summary>
    /// Mise à jour des variables du personnage lors d'Update.
    /// </summary>
    public override void InputUpdate()
    {
        if (!_controller.options.gameObject.active)
        {
            MouseCamera();
            if (!_controller.IsDead())
            {
                MovePlayer();
                _controller.sun._gotoAngle += _gotoAngleVelocity * _controller.sun._maxRotateSpeed * Time.deltaTime;
            }
        }
        else
        {
            _controller.velocity = Vector3.zero;
            _controller.cam.RotateMouse(Vector3.zero);
            _controller.cam.Rotate(Vector3.zero);
            
        }
    }

    
    /// <summary>
    /// Mise à jour des variables du personnage lors de FixedUpdate.
    /// </summary>
    public override void InputFixed() {}


    
    /// <summary>
    /// Déplacement du personnage.
    /// </summary>
    public override void MovePlayer()
    {
        _controller.velocity = Quaternion.Euler(0,_controller.cam.transform.eulerAngles.y,0) * (new Vector3(_move.x,0,_move.y) * _controller.speed);

        if (_controller.velocity.magnitude > 0)
        {
            _controller.transform.rotation = Quaternion.LookRotation(_controller.velocity);
        }
        else
        {
            _controller.velocity = Vector3.zero;
        }
    }

    
    /// <summary>
    /// Détecte la vélocité de la souris
    /// </summary>
    private void MouseCamera()
    {

        _controller.cam.RotateMouse(new Vector3(Input.GetAxis("Mouse X") * _controller.options.player1Settings.xAxisSensitivity, 
                                        Input.GetAxis("Mouse Y") * _controller.options.player1Settings.yAxisSensitivity * ((_controller.options.player1Settings.invertVertical)?-1:1)) * 0.8f);
    }

    /// <summary>
    /// Récupère la vélocité à appliquer au personnage
    /// </summary>
    /// <param name="readValue">valeur envoyé par InputSystem</param>
    public void Velocity(Vector2 readValue)
    {
        _move = readValue;
    }
    
    /// <summary>
    /// Récupère la vélocité à appliquer au soleil
    /// </summary>
    /// <param name="angle">valeur envoyé par InputSystem</param>
    public void RotateSun(float angle)
    {
        _gotoAngleVelocity = angle * _controller.options.player2Settings.sunSensitivity * ((_controller.options.player2Settings.invertSun)?-1:1);
    }
    
    /// <summary>
    /// Récupère la vélocité à appliquer à la caméra
    /// </summary>
    /// <param name="velocity">valeur envoyé par InputSystem</param>
    public void VelocityCam(Vector2 velocity)
    {
        _controller.cam.Rotate(new Vector2(velocity.x * _controller.options.player1Settings.xAxisSensitivity, 
            velocity.y  * _controller.options.player1Settings.yAxisSensitivity * ((_controller.options.player1Settings.invertVertical)?-1:1)));
    }

}
