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

    public override void InputUpdate()
    {
        MoveCamera();
        if (!_controller.IsDead())
        {
            MovePlayer();
            RotateSun();
        }
    }
    
    public override void MovePlayer()
    {
        if (!_hasPlayer) return;
        _move = _player.Move;
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
    private void RotateSun()
    {
        if (!_hasSun) return;
        _gotoAngleVelocity = _sun.AngleVelocity;
        _controller.sun._gotoAngle += _gotoAngleVelocity * _controller.sun._maxRotateSpeed * Time.deltaTime;
    }

    private void MoveCamera()
    {
        if (!_hasPlayer) return;
        _controller.cam.RotateMouse(_player.RotateMouse);
        _controller.cam.Rotate(_player.Rotate);
    }

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
}
