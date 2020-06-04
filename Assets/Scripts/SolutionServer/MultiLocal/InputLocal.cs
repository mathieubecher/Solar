using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Récupère les inputs d'un controlleur (2 max)
/// </summary>
public class InputLocal : MonoBehaviour
{
    // Type du controlleur
    private enum InputType
    {
        PLAYER,SUN
    }

    public UIInterface UiInterface;
    
    private Controller _controller;
    private PlayerInput _controls;
    [SerializeField]
    private Vector2 _move;
    [SerializeField]
    private Vector3 _rotateMouse;
    [SerializeField]
    private Vector2 _rotate;
    [SerializeField]
    private float _gotoAngleVelocity;
    [SerializeField]
    private float _velocityPlatform;
    [SerializeField]
    private InputType _type;

    
    private Action<InputAction.CallbackContext> rotateSun;
    private Action<InputAction.CallbackContext> rotateStickSun;
    
    private Action<InputAction.CallbackContext> progressPlatform;
    private Action<InputAction.CallbackContext> progressStickPlatform;
    public Vector2 Move => _move;

    public Vector3 RotateMouse {get{
            return new Vector3(_rotateMouse.x * UiInterface.player1Settings.xAxisSensitivity,_rotateMouse.y * UiInterface.player1Settings.yAxisSensitivity * ((UiInterface.player1Settings.invertVertical)?-1:1),0); 
    }}

    public Vector2 Rotate{get{    
            return new Vector3(_rotate.x * UiInterface.player1Settings.xAxisSensitivity,_rotate.y * UiInterface.player1Settings.yAxisSensitivity * ((UiInterface.player1Settings.invertVertical)?-1:1));
    }}

    public float AngleVelocity { get{return _gotoAngleVelocity * UiInterface.player2Settings.sunSensitivity * ((UiInterface.player2Settings.invertSun)?-1:1); } }

    public float VelocityPlatform { get{return _velocityPlatform * UiInterface.player2Settings.platformSensitivity;} }

    void Start()
    {
        
        rotateSun = ctx => RotateSun(ctx.ReadValue<float>());
        rotateStickSun = ctx => RotateStickSun(ctx.ReadValue<Vector2>());
        
        
        progressPlatform = ctx => ProgressPlatform(ctx.ReadValue<float>());
        progressStickPlatform = ctx => ProgressStickPlatform(ctx.ReadValue<Vector2>());

        
        UiInterface = FindObjectOfType<Controller>().UiInterface;
        
        // Récupère le controlleur du personnage
        _controller = FindObjectOfType<Controller>();
        
        Local inputs = (Local)_controller.inputs;
        _type = (inputs.SetInput(this) == 0)? InputType.PLAYER:InputType.SUN;
        
        _controls = GetComponent<PlayerInput>();
        Debug.Log(_controls.currentControlScheme);
        
        if(_type == InputType.PLAYER){
        // Abonne la classe aux evennements d'InputSystem
            _controls.currentActionMap["Movement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
            _controls.currentActionMap["Movement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
                
            _controls.currentActionMap["KeyMovement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
            _controls.currentActionMap["KeyMovement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
                
            _controls.currentActionMap["Rotate"].performed += ctx => VelocityCam(ctx.ReadValue<Vector2>());
            _controls.currentActionMap["Rotate"].canceled += ctx => VelocityCam(ctx.ReadValue<Vector2>());
        }
        _controls.currentActionMap["RotateSun"].performed += rotateSun;
        _controls.currentActionMap["RotateSun"].canceled += rotateSun;
        
        _controls.currentActionMap["ProgressPlatform"].performed += progressPlatform;
        _controls.currentActionMap["ProgressPlatform"].canceled += progressPlatform;
    }

    public void Update()
    {
        // Récupère la vélocité de la souris si l'Input est le clavier&souris
        if(_type == InputType.PLAYER && _controls.currentControlScheme.Equals("Keyboard and mouse")) MouseCamera();
    }
    
    /// <summary>
    /// Détecte la vélocité de la souris
    /// </summary>
    private void MouseCamera()
    {
        _rotate = new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")) * 0.8f;
    }

    /// <summary>
    /// Récupère la vélocité à appliquer au personnage
    /// </summary>
    /// <param name="readValue">valeur envoyé par InputSystem</param>
    public void Velocity(Vector2 readValue)
    {
        _move = new Vector2(readValue.x, readValue.y);
    }
    /// <summary>
    /// Récupère la vélocité à appliquer au soleil
    /// </summary>
    /// <param name="angle">valeur envoyé par InputSystem</param>
    public void RotateSun(float angle)
    {
        _gotoAngleVelocity = angle;
    }
    /// <summary>
    /// Récupère la vélocité à appliquer à la caméra
    /// </summary>
    /// <param name="velocity">valeur envoyé par InputSystem</param>
    public void VelocityCam(Vector2 velocity)
    {
        _rotateMouse = velocity;
    }
    /// <summary>
    /// Récupère la vélocité à appliquer à la plateforme
    /// </summary>
    /// <param name="readValue">valeur envoyé par InputSystem</param>
    private void ProgressPlatform(float readValue)
    {
        _velocityPlatform = readValue;
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
        _velocityPlatform = readValue.y;
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
            Debug.Log("Rebind Sun L1R1 " + last);
        }
        else if (bind == UIInterface.Bind.L2R2)
        {
            _controls.currentActionMap["RotateSun"].performed += rotateSun;
            _controls.currentActionMap["RotateSun"].canceled += rotateSun;
            Debug.Log("Rebind Sun L2R2 " + last);
        }
        else if (bind == UIInterface.Bind.LeftStick)
        {
            _controls.currentActionMap["Movement"].performed += rotateStickSun;
            _controls.currentActionMap["Movement"].canceled += rotateStickSun;
            Debug.Log("Rebind Sun LeftStick " + last);
        }
        else
        {
            _controls.currentActionMap["Rotate"].performed += rotateStickSun;
            _controls.currentActionMap["Rotate"].canceled += rotateStickSun;
            Debug.Log("Rebind Sun RightStick " + last);
        }
        
    }

    private void RotateStickSun(Vector2 readValue)
    {
        _gotoAngleVelocity = readValue.y;
    }

    #endregion
    
    private void ResetBind(UIInterface.Bind last)
    {
        // TODO isolé les fonctions ciblé
        if (last == UIInterface.Bind.L1R1)
        {
            Debug.Log("Reset L1R1");
            _controls.currentActionMap["ProgressPlatform"].performed -= rotateSun;
            _controls.currentActionMap["ProgressPlatform"].canceled -= rotateSun;
            _controls.currentActionMap["ProgressPlatform"].performed -= progressPlatform;
            _controls.currentActionMap["ProgressPlatform"].canceled -= progressPlatform;
            
        }
        else if (last == UIInterface.Bind.L2R2)
        {
            
            Debug.Log("Reset L2R2");
            _controls.currentActionMap["RotateSun"].performed -= rotateSun;
            _controls.currentActionMap["RotateSun"].canceled -= rotateSun;
            _controls.currentActionMap["RotateSun"].performed -= progressPlatform;
            _controls.currentActionMap["RotateSun"].canceled -= progressPlatform;
        }
        else if (last == UIInterface.Bind.LeftStick)
        {
            
            Debug.Log("Reset LeftStick");
            _controls.currentActionMap["Movement"].performed -= rotateStickSun;
            _controls.currentActionMap["Movement"].canceled -= rotateStickSun;
            _controls.currentActionMap["Movement"].performed -= progressStickPlatform;
            _controls.currentActionMap["Movement"].canceled -= progressStickPlatform;
        }
        else
        {
            
            Debug.Log("Reset RigthStick");
            _controls.currentActionMap["Rotate"].performed -= rotateStickSun;
            _controls.currentActionMap["Rotate"].canceled -= rotateStickSun;
            _controls.currentActionMap["Rotate"].performed -= progressStickPlatform;
            _controls.currentActionMap["Rotate"].canceled -= progressStickPlatform;
        }
    }
}
