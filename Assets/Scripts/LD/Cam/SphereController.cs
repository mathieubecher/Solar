using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.UI;

public class SphereController : MonoBehaviour
{
    private Transform last;
    [SerializeField]
    private Transform cmCam;
    public Transform CMCam
    {
        get => cmCam;
        set
        {
            if (value == cmCam) return;
            Debug.Log("Change cam sphere");
            last = cmCam;
            cmCam = value;
            progress = 0;
        }
    }

    private CameraCurve curve;
    public CameraCurve Curve
    {
        get => curve;
        set => curve = value;
    }
    private float progress = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameManager manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (progress < 1)
        {
            Transition();
        }
        else
        {
            transform.position = cmCam.position;
        }
    }

    void Transition()
    {
        progress += Time.deltaTime * ((curve != null)?curve.speed:1);
        List<Vector3> points = new List<Vector3>();
        points.Add(last.position);
        if(curve != null){
            //TODO Gestion des points de la curve
            foreach (GizmosPoint point in curve.Points)
            {
                points.Add(point.transform.position);
            }
        }
        points.Add(cmCam.position);
        transform.position = CameraCurve.Bezier(points, (curve != null)?curve.progressCurve.Evaluate(progress):progress);
       
    }

   
    
}
#if UNITY_EDITOR
[CustomEditor(typeof(SphereController))]
public class SphereControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
    }
}
#endif