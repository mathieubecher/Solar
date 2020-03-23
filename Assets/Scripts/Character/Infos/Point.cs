using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private bool touch;
    private int mask;
    
    private float dist = 100;
    // Start is called before the first frame update
    void Start()
    {
        mask =~ LayerMask.GetMask("Character");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = (touch)?Color.red:Color.green;
        Gizmos.DrawSphere(transform.position,0.1f);
    }

    public bool TestLight(LightController sun)
    {
        
        if (Physics.Raycast(origin: transform.position, direction: sun.transform.rotation * Vector3.back, hitInfo: out RaycastHit hit, maxDistance:dist, layerMask: mask))
        {
            touch = false;
        }
        else
        {
            touch = true;
        }

        return touch;

    }
}
