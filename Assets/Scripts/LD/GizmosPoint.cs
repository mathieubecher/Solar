using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Points utilisable pour les graphs et trajectoires.
/// </summary>
public class GizmosPoint : MonoBehaviour
{
    #if UNITY_EDITOR
    private Vector3 lastpos;
    public bool change;
    private void OnDrawGizmos()
    {
        change = (lastpos != transform.position) ;
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position,0.5f);

        lastpos = transform.position;
    }
    #endif
}
