using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private bool touch;

    [SerializeField] private float dist = 20;
    // Start is called before the first frame update
    void Start()
    {
        
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

    public bool TestLight(Light sun)
    {
        int mask =~ LayerMask.GetMask("Character");
        RaycastHit hit;
        if (Physics.Raycast(origin: transform.position, direction: sun.transform.rotation * Vector3.back, hitInfo: out hit, maxDistance:dist, layerMask: mask))
        {
            #if UNITY_EDITOR
                Debug.DrawLine(transform.position, hit.point, Color.green,Time.deltaTime);
            #endif
            touch = false;
        }
        else
        {
            #if UNITY_EDITOR
                Debug.DrawLine(transform.position, transform.position + sun.transform.rotation * Vector3.back * 1, Color.red,Time.deltaTime);
            #endif
            touch = true;
        }

        return touch;

    }
}
