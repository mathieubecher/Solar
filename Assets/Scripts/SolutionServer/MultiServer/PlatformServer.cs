using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

/// <summary>
/// Gestion serveur des plateformes
/// </summary>
public class PlatformServer : PlatformBehavior
{
    /// <summary>
    /// Met à jour le progress de l'autre joueur
    /// </summary>
    /// <param name="p"></param>
    public void CallSetProgress(float p)
    {
        if (networkObject.IsServer) networkObject.progress = p;
        else
        {
            networkObject.SendRpcUnreliable(RPC_SET_PROGRESS, Receivers.Server, p);
        }
    }
    /// <summary>
    /// Récupère le progress de l'autre joueur
    /// </summary>
    /// <param name="args"></param>
    public override void SetProgress(RpcArgs args)
    {
        if (networkObject.IsServer) networkObject.progress = args.GetNext<float>();
    }

    /// <summary>
    /// Récupère la valeur serveur de progress
    /// </summary>
    /// <returns></returns>
    public float GetProgress()
    {
        return networkObject.progress;
    }
}
