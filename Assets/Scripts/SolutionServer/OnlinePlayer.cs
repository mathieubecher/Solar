using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Character.Infos
{
    public class OnlinePlayer : ControllerBehavior, AbstractInput
    {
        protected Vector2 _move;
        protected Controller _controller;
        protected PlayerInput _controls;
        private bool isMoving;

        public OnlinePlayer(Controller controller)
        {
            _controller = controller;
            _controls = _controller.GetComponent<PlayerInput>();
            
            _controls.currentActionMap["Movement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
            _controls.currentActionMap["Movement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
            
            _controls.currentActionMap["KeyMovement"].performed += ctx => Velocity(ctx.ReadValue<Vector2>());
            _controls.currentActionMap["KeyMovement"].canceled += ctx => Velocity(ctx.ReadValue<Vector2>());
            
            _controls.currentActionMap["Rotate"].performed += ctx => _controller._camera.Rotate(ctx.ReadValue<Vector2>());
            _controls.currentActionMap["Rotate"].canceled += ctx => _controller._camera.Rotate(ctx.ReadValue<Vector2>());
        }
        public void Velocity(Vector2 readValue)
        {
            _move = new Vector2(readValue.x, readValue.y);
        }

        public void MovePlayer()
        {
            Vector3 velocity = Quaternion.Euler(0,_controller._camera.transform.eulerAngles.y,0) * (new Vector3(_move.x,0,_move.y) * _controller.speed);

            if (velocity.magnitude > 0)
            {
                _controller.transform.rotation = Quaternion.LookRotation(velocity);
            }
            else
            {
                velocity = Vector3.zero;
            }
            _controller.animator.SetFloat("velocity",velocity.magnitude);
            velocity.y = _controller._rigidbody.velocity.y;
            _controller._rigidbody.velocity = velocity;
        }

        public void RotateSun(float angle)
        {
            
        }

        public void VelocityCam(Vector2 velocity)
        {
            
        }

        public void Update()
        {
            MovePlayer();
        }

        public void FixedUpdate()
        {
            if (isMoving)
            {
                isMoving = false;
                // TODO
                networkObject.position = transform.position;
                networkObject.rotation = transform.rotation;
            }
        }

        public override void SetRotate(RpcArgs args)
        {
            
        }
    }
}