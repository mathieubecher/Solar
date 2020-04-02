using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

public class CameraController : MonoBehaviour
{
    double TOLERANCE = 0.1; 
    [SerializeField]
    private Controller follow;
    [SerializeField]
    private float _rotateSpeed = 100;
    public float RotateSpeed {get => _rotateSpeed;set => _rotateSpeed = value;}
    [SerializeField]
    private float _speed = 2;
    public float Speed {get => _speed;set => _speed = value;}

    public bool gizmos = true;
    [SerializeField]
    private float upMaxAngle = 90;
    public float UpMaxAngle
    {
        get => upMaxAngle;
        set => upMaxAngle = value;
    }
    [SerializeField]
    private float bottomMaxAngle = -90;
    public float BottomMaxAngle
    {
        get => bottomMaxAngle;
        set => bottomMaxAngle = value;
    }

    private float angle;
    public float Angle {get => angle; }
    
    public float distance = 5;
    private float actualDistance;
    private Vector3 rotateFrame;
    private Vector3 rotateMouse;

    private void Awake()
    {
        actualDistance = distance;
        float rotatex = transform.eulerAngles.x;
        transform.rotation = follow.transform.rotation;
        transform.eulerAngles = new Vector3(rotatex, transform.eulerAngles.y,transform.rotation.z);
    }

    void Update()
    {
        transform.eulerAngles += (Time.deltaTime) * _rotateSpeed * (rotateFrame+rotateMouse);
        LimitCam();
        SetPos();
    }
    private void OnDrawGizmos()
    {
        LimitCam();
    }
    
    
    private void LimitCam()
    {
        float angleX = (transform.eulerAngles.x + 180) % 360 - 180;
        if (angleX > upMaxAngle) transform.eulerAngles = new Vector3(upMaxAngle,transform.eulerAngles.y,0);
        else if (angleX < bottomMaxAngle) transform.eulerAngles = new Vector3(bottomMaxAngle,transform.eulerAngles.y,0);
        else transform.eulerAngles = new Vector3(angleX,transform.eulerAngles.y,0);
    }
    
    void SetPos()
    {
        Vector3 direction = transform.rotation * Vector3.back;
        int mask =~ LayerMask.GetMask("Character");
        if (Physics.Raycast(follow.Target, direction, out RaycastHit ray, distance+0.5f, mask))
        {
            actualDistance = Mathf.Max(0.5f,ray.distance -0.5f);
        }
        else actualDistance += Time.deltaTime * _speed;

        if (actualDistance > distance) actualDistance = distance;
        
        #if UNITY_EDITOR
        Debug.DrawLine(follow.Target, follow.Target + direction * actualDistance , Color.green,Time.deltaTime);
        if(Math.Abs(actualDistance - distance) > TOLERANCE) Debug.DrawLine(follow.Target + direction * actualDistance, follow.Target + direction * distance , Color.red,Time.deltaTime);
        #endif
        
        transform.position = follow.Target + direction * actualDistance;
    }
    
    
    
    public void RotateMouse(Vector3 mousePosition)
    {
        rotateMouse = new Vector3(-mousePosition.y,mousePosition.x,0);    
    }

    public void Rotate(Vector2 rotate)
    {
        rotateFrame = new Vector3(-rotate.y,rotate.x,0)*2;
    }

}

