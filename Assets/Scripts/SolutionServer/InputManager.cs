using Unity;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class InputManager : ControllerBehavior
{
    public Quaternion rotation;
    public Vector3 position;
    public float sunRotation;

    void Start()
    {
        rotation = Quaternion.identity;
        position = Vector3.one;
        sunRotation = 0;
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
            }
            else
            {
                controller.inputs = new OnlineSun(controller,this);
            }
        }
    }

    public void CallSetRotate(Quaternion r)
    {
        networkObject.SendRpc(RPC_SET_ROTATE, (networkObject.IsServer)?Receivers.Others:Receivers.Server, r);
    }
    public override void SetRotate(RpcArgs args)
    {
        rotation = args.GetNext<Quaternion>();
        
    }
    
    public void CallSetPosition(Vector3 p)
    {
        networkObject.SendRpc(RPC_SET_POSITION, (networkObject.IsServer)?Receivers.Others:Receivers.Server, p);
    }
    public override void SetPosition(RpcArgs args)
    {
        position = args.GetNext<Vector3>();
        Debug.Log("Set Position");
    }
    
    public void CallSetSunRotate(float f)
    {
        networkObject.SendRpc(RPC_SET_SUN_ROTATE, (networkObject.IsServer)?Receivers.Others:Receivers.Server, f);
    }
    public override void SetSunRotate(RpcArgs args)
    {
        sunRotation = args.GetNext<float>();
        Debug.Log("Set Rotation");
    }

}
