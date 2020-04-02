using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer2 : MonoBehaviour
{
    double TOLERANCE = 0.1; 
    [SerializeField]
    private Controller follow;

    [SerializeField, Range(5,100)] private float _minDistance = 5;
    [SerializeField, Range(5,100)] private float _optimizeDistance = 20;
    [SerializeField, Range(5,100)] private float _maxDistance = 50;
    private float actualDistance;
    private float gotoDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        actualDistance = _optimizeDistance;
        gotoDistance = _optimizeDistance;
    }

    // Update is called once per frame
    void Update()
    {
        SetPos();
    }

    private void SetPos()
    {
        float distance = _optimizeDistance;
        if (Physics.Raycast(follow.Target + transform.rotation * Vector3.back * _maxDistance, (transform.rotation * Vector3.forward), out RaycastHit ray, _maxDistance-_optimizeDistance))
        {
            distance =_maxDistance - ray.distance + _minDistance;
        }
        transform.position = follow.Target + (transform.rotation * Vector3.back) * distance;
    }
}
    