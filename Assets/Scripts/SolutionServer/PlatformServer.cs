using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class PlatformServer : PlatformBehavior
{
    public void CallSetProgress(float p)
    {
        if (networkObject.IsServer) networkObject.progress = p;
        else
        {
            networkObject.SendRpcUnreliable(RPC_SET_PROGRESS, Receivers.Server, p);
        }
    }
    public override void SetProgress(RpcArgs args)
    {
        if (networkObject.IsServer) networkObject.progress = args.GetNext<float>();
    }

    public float GetProgress()
    {
        return networkObject.progress;
    }
}
