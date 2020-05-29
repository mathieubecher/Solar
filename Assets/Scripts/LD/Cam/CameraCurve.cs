using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCurve : MonoBehaviour
{
    private GizmosPoint[] points;

    public GizmosPoint[] Points
    {
        get
        {
            if (points == null) points = GetComponentsInChildren<GizmosPoint>();
            return points;
        }
    }

    void Start()
    {
        points = GetComponentsInChildren<GizmosPoint>();
    }
}
