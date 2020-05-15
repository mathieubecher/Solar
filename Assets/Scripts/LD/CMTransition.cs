using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transition entre les caméras
/// </summary>
public class CMTransition : MonoBehaviour
{
    public CMCamera _previous;
    public CMCamera _next;

    [HideInInspector] public Collider previousCollider;
    [HideInInspector] public Collider nextCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        Collider[] collides = GetComponents<Collider>();
        previousCollider = collides[0];
        nextCollider = collides[1];
    }

    
    

}
