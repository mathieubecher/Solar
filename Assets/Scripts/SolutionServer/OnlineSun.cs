using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Character.Infos
{
    public class OnlineSun: ControllerBehavior, AbstractInput
    {
        protected Vector2 _move;
        protected Controller _controller;
        protected PlayerInput _controls;
        private float _gotoAngleVelocity = 0;
        
        public OnlineSun(Controller controller)
        {
            _controller = controller;
            _controls = _controller.GetComponent<PlayerInput>();
            _controls.currentActionMap["RotateSun"].performed += ctx => RotateSun(ctx.ReadValue<float>());
            _controls.currentActionMap["RotateSun"].canceled += ctx => RotateSun(ctx.ReadValue<float>());
        }

        public void MovePlayer()
        {
            Vector3 lastpos = transform.position;
            // TODO
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;

            _controller.animator.SetFloat("velocity", (lastpos - transform.position).magnitude > 0.01f ? 1 : 0);
        }

        public void RotateSun(float angle)
        {
            _controller._sun.Rotate(angle);
        }

        public void Update()
        {
            MovePlayer();
        }

        public void FixedUpdate()
        {
            
        }

        public override void SetRotate(RpcArgs args)
        {
            
        }
    }
}