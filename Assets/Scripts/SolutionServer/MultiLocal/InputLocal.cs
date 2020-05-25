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

    public Vector2 Move => _move;
    public Vector3 RotateMouse => _rotateMouse;
    public Vector2 Rotate => _rotate;
    public float AngleVelocity => _gotoAngleVelocity;

    public float VelocityPlatform => _velocityPlatform;

    void Start()
    {
        // Récupère le controlleur du personnage
        _controller = FindObjectOfType<Controller>();
        
        Local inputs = (Local)_controller.inputs;
        _type = (inputs.SetInput(this) == 0)? InputType.PLAYER:InputType.SUN;
        
        _controls = GetComponent<PlayerInput>();
        Debug.Log(_controls.currentControlScheme);
        
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
}
