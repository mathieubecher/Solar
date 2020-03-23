using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Controller follow;
    [HideInInspector]
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
    

    private void Awake()
    {
        float rotatex = transform.eulerAngles.x;
        transform.rotation = follow.transform.rotation;
        transform.eulerAngles = new Vector3(rotatex, transform.eulerAngles.y,transform.rotation.z);
    }

    void Update()
    {
        SetPos();
        LimitCam();
    }

    void SetPos()
    {
        transform.position =  follow.Target + transform.rotation * Vector3.back * distance;
        
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

    #region INSPECTOR
    void Reset()
    {
        follow = FindObjectOfType<Controller>();
        if (TryGetComponent<GizmosCamera>(out GizmosCamera gizmosCamera))
        {
            DestroyImmediate(gizmosCamera);
        }
        gameObject.AddComponent<GizmosCamera>();
    }
    

    #endregion
}


[ExecuteInEditMode]
internal class GizmosCamera : MonoBehaviour
{
    private CameraController camera;
    
    void Awake()
    {
        camera = GetComponent<CameraController>();
    }
    

}
