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
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (_previous != null && _next != null)
        {
            CameraCurve curve = GetComponent<CameraCurve>();
            Vector3 last = _previous.transform.position;
            foreach (GizmosPoint point in curve.Points)
            {
                Gizmos.DrawLine(last, point.transform.position);
                last = point.transform.position;

            }
            Gizmos.DrawLine(last, _next.transform.position);


        }
    }
#endif

}
