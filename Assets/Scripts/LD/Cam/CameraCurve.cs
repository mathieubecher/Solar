using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCurve : MonoBehaviour
{
    public GizmosPoint[] points;
    public float speed = 1;
    public AnimationCurve progressCurve = AnimationCurve.Linear(0,0,1,1);

    public GizmosPoint[] Points
    {
        get
        {
            //if (points == null)
            points = GetComponentsInChildren<GizmosPoint>();
            return points;
        }
    }

    void Start()
    {
        points = GetComponentsInChildren<GizmosPoint>();
    }
    
    public static Vector3 Bezier(List<Vector3> list, float progress)
    {
        if (list.Count == 1) return list[0];
        
        List<Vector3> newlist = new List<Vector3>();
        for (int i = 1; i < list.Count; ++i)
        {
            newlist.Add(Vector3.Lerp(list[i - 1], list[i],progress));
        }

        return Bezier(newlist, progress);
    }
}
