using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;

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
        else transform.position = cmCam.position;
    }

    void Transition()
    {
        progress += Time.deltaTime;
        transform.position = Vector3.Lerp(last.position, cmCam.position, progress);
        //TODO Gestion des points de la curve
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