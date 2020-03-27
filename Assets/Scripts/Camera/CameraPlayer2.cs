using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer2 : MonoBehaviour
{
    double TOLERANCE = 0.1; 
    [SerializeField]
    private Controller follow;

    [SerializeField, Range(5,100)] private float distance = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetPos();
    }

    private void SetPos()
    {
        transform.position = follow.Target + (transform.rotation * Vector3.back) * distance;
    }
}
