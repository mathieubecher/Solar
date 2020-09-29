using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// Transition entre les caméras
/// </summary>
public class CMTransition : MonoBehaviour
{

    public CMCamera previous;
    public CMCamera next;

    [Header("Transition info")]
    public CinemachineBlendDefinition.Style type = CinemachineBlendDefinition.Style.EaseInOut;
    [Range(0,5)] public float transitionTime = 1;

    [HideInInspector] public Collider previousCollider;
    [HideInInspector] public Collider nextCollider;

    private List<Vector3> bezier;

    public bool direct = false;
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
        if (previous != null && next != null)
        {
            CameraCurve curve = GetComponent<CameraCurve>();
            if(curve.Points.Length>0){
                
                if(bezier == null || bezier.Count == 0 || PointChange()) UpdateBezier();
                for (int i = 1; i < bezier.Count; ++i)
                {
                    Gizmos.DrawLine(bezier[i-1], bezier[i]);
                }
            }
            else
            {
                Gizmos.DrawLine(previous.transform.position, next.transform.position);
            }

        }
    }

    
    void UpdateBezier()
    {
        bezier = new List<Vector3>();
        CameraCurve curve = GetComponent<CameraCurve>();
        List<Vector3> points = new List<Vector3>();
        points.Add(previous.transform.position);
        foreach (GizmosPoint point in curve.Points)
        {
            points.Add(point.transform.position);
        }
        points.Add(next.transform.position);

        bezier.Add(previous.transform.position);
        for (float i = 0.1f; i <= 1; i += 0.1f)
        {
            bezier.Add(CameraCurve.Bezier(points,i));
        }
        bezier.Add(next.transform.position);
    }

    bool PointChange()
    {
        bool change = false;
        int i = 0;
        CameraCurve curve = GetComponent<CameraCurve>();
        while (!change && i < curve.Points.Length)
        {
            change |= curve.Points[i].change;
            ++i;
        }
        
        return change;
    }
#endif

}
