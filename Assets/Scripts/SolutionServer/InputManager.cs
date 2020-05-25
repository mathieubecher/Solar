using Unity;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class InputManager : ControllerBehavior
{
    public Quaternion rotation{
        get { return networkObject.rotation; }
    }
    public Vector3 position{
        get { return networkObject.position; }
    }
    public float sunRotation{
        get { return networkObject.sunrotation; }
    }

    public Vector3 velocity
    {
        get { return networkObject.velocity; }
    }
    
    void Start()
    {
        Controller controller = FindObjectOfType<Controller>();
        
        // Configure les Inputs Controllers
        if(networkObject.IsServer){
            if(StaticClass.serverType == StaticClass.ServerType.PLAYER) controller.inputs = new OnlinePlayer(controller,this);
            else controller.inputs = new OnlineSun(controller,this);
            networkObject.typeserver = (StaticClass.serverType == StaticClass.ServerType.PLAYER)?0:1;
        }
        else
        {
            if (networkObject.typeserver == 1)
            {
                controller.inputs = new OnlinePlayer(controller,this);
                FindObjectOfType<MultiMonitor>().OnlinePlayer();
                StaticClass.serverType = StaticClass.ServerType.SUN;
            }
            else
            {
                controller.inputs = new OnlineSun(controller,this);
                StaticClass.serverType = StaticClass.ServerType.PLAYER;
            }
        }
        
    }

    public void CallSetRotate(Quaternion r)
    {
        if (networkObject.IsServer) networkObject.rotation = r;
        else networkObject.SendRpcUnreliable(RPC_SET_ROTATE, Receivers.Server, r);
    }
    public override void SetRotate(RpcArgs args)
    {
        if(networkObject.IsServer) networkObject.rotation = args.GetNext<Quaternion>();
        
    }
    
    public void CallSetPosition(Vector3 p)
    {
        if (networkObject.IsServer) networkObject.position = p;
        else networkObject.SendRpcUnreliable(RPC_SET_POSITION, Receivers.Server, p);
    }
    public override void SetPosition(RpcArgs args)
    {
        if(networkObject.IsServer) networkObject.position = args.GetNext<Vector3>();
    }
    
    public void CallSetSunRotate(float f)
    {
        if (networkObject.IsServer) networkObject.sunrotation = f;
        else networkObject.SendRpcUnreliable(RPC_SET_SUN_ROTATE, Receivers.Server, f);
    }
    public override void SetSunRotate(RpcArgs args)
    {
        if(networkObject.IsServer) networkObject.sunrotation = args.GetNext<float>();
    }
    public void CallSetVelocity(Vector3 v)
    {
        if (networkObject.IsServer) networkObject.velocity = v;
        else networkObject.SendRpcUnreliable(RPC_SET_VELOCITY, Receivers.Server, v);
    }
    public override void SetVelocity(RpcArgs args)
    {
        if (networkObject.IsServer) networkObject.velocity = args.GetNext<Vector3>();
    }


}
