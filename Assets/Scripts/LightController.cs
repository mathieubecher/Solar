using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public bool gizmos = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[CustomEditor(typeof(LightController))]
public class LightEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var light = (LightController) target;
        if (light.gizmos && GUILayout.Button("Disable Gizmos")) light.gizmos = false;
        else if (!light.gizmos && GUILayout.Button("Enable Gizmos")) light.gizmos = true;
    }
}