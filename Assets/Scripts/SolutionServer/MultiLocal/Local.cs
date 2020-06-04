using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Local : AbstractInput
{
    private bool _hasPlayer;
    private InputLocal _player;
    public void SetPlayer(InputLocal player) {_player = player; _hasPlayer = true;}
    
    private bool _hasSun;
    private InputLocal _sun;
    public void SetSun(InputLocal sun) {_sun = sun; _hasSun = true;}
    
    public Local(Controller controller) : base(controller)
    {
        
    }

    /// <summary>
    /// Mise à jour des variables du personnage lors d'Update.
    /// </summary>
    public override void InputUpdate()
    {
        if (!_controller.UiInterface.gameObject.active)
        {
            // Applique la velocité de la camera
            MoveCamera();
            // Applique la vélocité du personnage et du soleil
            if (!_controller.IsDead())
            {
                MovePlayer();
                RotateSun();
                ProgressPlatform();
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
    /// Déplacement du personnage.
    /// </summary>
    public override void MovePlayer()
    {
        // ignore cette étape si le joueur ne controle pas le personnage
        if (!_hasPlayer) return;
        
        // transforme la valeur de l'input en velocité pour le joueur
        _move = _player.Move;
        _controller.velocity = Quaternion.Euler(0,_controller.cam.transform.eulerAngles.y,0) * 
                               (new Vector3(_move.x,0,_move.y) * _controller.speed);
        
        // met à jour la rotation du joueur en fonction de la vélocité calculée
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
    /// Déplacement du soleil.
    /// </summary>
    private void RotateSun()
    {
        if (!_hasSun) return;
        _gotoAngleVelocity = _sun.AngleVelocity;
        _controller.sun._gotoAngle += _gotoAngleVelocity * _controller.sun._maxRotateSpeed * Time.deltaTime;
    }
    
    /// <summary>
    /// Déplacement de la caméra.
    /// </summary>
    private void MoveCamera()
    {
        if (!_hasPlayer) return;
        _controller.cam.RotateMouse(_player.RotateMouse);
        _controller.cam.Rotate(_player.Rotate);
    }
    
    /// <summary>
    /// Déplacement des plateformes.
    /// </summary>
    private void ProgressPlatform()
    {
        if (!_hasSun) return; 
        _controller.puzzle.cmActual.SetPlatformProgress(_sun.VelocityPlatform);
    }

    /// <summary>
    /// Défini les input du personnage.
    /// </summary>
    /// <param name="inputLocal"></param>
    /// <returns>Renvoie le type du player</returns>
    public int SetInput(InputLocal inputLocal)
    {
        if(_hasPlayer)
        {
            SetSun(inputLocal);
            return 1;
        }
        else SetPlayer(inputLocal);
        return 0;
    }

    private UIInterface.Bind lastPlatform = UIInterface.Bind.L1R1;
    private UIInterface.Bind lastSun = UIInterface.Bind.L2R2;
    public override void BindPlatform(UIInterface.Bind bind)
    {
        if (lastPlatform != bind) _sun.BindPlatform(lastPlatform, bind);
        lastPlatform = bind;
    }

    public override void BindSun(UIInterface.Bind bind)
    {
        if (lastSun != bind) _sun.BindSun(lastSun, bind);
        lastSun = bind;
    }
}
