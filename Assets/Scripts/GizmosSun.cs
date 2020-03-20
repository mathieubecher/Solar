using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
[ExecuteInEditMode]
public class GizmosSun : MonoBehaviour
{
    private Light light;
    [SerializeField] private Vector2 center= new Vector2(80,80);
    private Vector2 actualCenter;
    [SerializeField]
    private float radius = 50;
    [SerializeField] private float rectSize = 4;
    [Range(0,360)]
    [SerializeField] private float sunInclinason = 120;
    
    private bool clickRotate;
    private bool clickIncline;

    // Start is called before the first frame update
    void Awake()
    {
        //Destroy(gameObject);
        light = FindObjectOfType<Light>();
        actualCenter = center;
    }

    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Rect rect = DefineRotation();
        Rect inclineRect = DefineIncline();
        SetPos();
        
        // Draw interface 
        Handles.BeginGUI();
        DrawIncline(inclineRect);
        DrawRotation(rect);
        Handles.color = Color.white;
        Handles.DrawSolidDisc(actualCenter, Vector3.forward, 5);
        Handles.EndGUI();
    }

    private bool move;
    private void SetPos()
    {
        Vector2 mousePos = Event.current.mousePosition;
        if (!move && Vector2.Distance(actualCenter, mousePos) < 5 && Event.current.button == 0) move = true;
        else if (move && Event.current.button != 0) move = false;

        if (move)
        {
            clickIncline = false;
            clickRotate = false;
            actualCenter = mousePos;
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
        }
    }
    
    #region ROTATION
    Rect DefineRotation()
    {
        var eulerAngles = light.transform.eulerAngles;
        float angle = eulerAngles.y * Mathf.PI / 180;
        Vector2 rotatePos = RotatePos(eulerAngles.y);
        Rect rect = new Rect(rotatePos.x - rectSize/2, rotatePos.y - rectSize/2, rectSize, rectSize);
        Vector2 mousepos = Event.current.mousePosition;

        bool hover = rect.Contains(mousepos) || clickRotate;
        if (!clickRotate && hover && Event.current.button == 0)
        {
            clickRotate = true;
        }
        
        // Action on Click
        if (clickRotate)
        {
            Vector3 rotation = light.transform.eulerAngles;
            rotation.y = Vector2.SignedAngle(mousepos-actualCenter, Vector2.right);
            light.transform.eulerAngles = rotation;
            rotatePos = RotatePos(eulerAngles.y);
            rect = new Rect(rotatePos.x - rectSize/2, rotatePos.y - rectSize/2, rectSize, rectSize);
            
            // Force scene update until release click
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
            
            // Leave condition
            if(Event.current.button != 0) clickRotate = false;
            
        }

        return rect;
    }
    
    void DrawRotation(Rect rect)
    {
        Vector3 eulerAngles = light.transform.eulerAngles;
        Handles.color = Color.red;
        Handles.DrawWireArc(actualCenter,Vector3.forward, RotateDir(eulerAngles.y+rectSize/2), -360 + rectSize, radius);
        Handles.DrawLine(actualCenter,RotatePos(eulerAngles.y, radius-rectSize/2 ));
        Handles.DrawLine(actualCenter,RotatePos(0));
        UnityEditor.Handles.DrawSolidRectangleWithOutline(rect, clickRotate?Color.red:new Color(1,0,0,0.2f), Color.red);
        Handles.color = new Color(1,0,0,0.1f);
        if(light.transform.eulerAngles.y > 180) Handles.DrawSolidArc(actualCenter,Vector3.forward,RotateDir(eulerAngles.y) ,-360+eulerAngles.y, radius);
        else Handles.DrawSolidArc(actualCenter,Vector3.forward,Vector2.right ,-light.transform.eulerAngles.y, radius);
    }
    #endregion
    
    #region INCLINE
    private Rect DefineIncline()
    {   
        var eulerAngles = light.transform.eulerAngles;
        float incline = eulerAngles.x * Mathf.PI / 180;
        Vector2 inclinePos = RotatePos(eulerAngles.x, radius + 10);
        Rect rect = new Rect(inclinePos.x - rectSize/2, inclinePos.y - rectSize/2, rectSize, rectSize);
        Vector2 mousepos = Event.current.mousePosition;
        bool hover = rect.Contains(mousepos) || clickIncline;
        if (!clickRotate && hover && Event.current.button == 0)
        {
            clickIncline = true;
        }
        
        if (clickIncline)
        {
            Vector3 rotation = light.transform.eulerAngles;
            rotation.x = Mathf.Max(90 - sunInclinason/2,Mathf.Min(90, Vector2.SignedAngle(mousepos-actualCenter, Vector2.right)));
            light.transform.eulerAngles = rotation;
            inclinePos = RotatePos(eulerAngles.x, radius + 10);
            rect = new Rect(inclinePos.x - rectSize/2, inclinePos.y - rectSize/2, rectSize, rectSize);
            
            // Force scene update until release click
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
            
            // Leave condition
            if(Event.current.button != 0) clickIncline = false;
            
        }

        return rect;
    }
    void DrawIncline(Rect inclineRect)
    {
        Handles.color = new Color(0, 0, 1, 1);
        Handles.DrawWireArc(actualCenter,Vector3.forward, new Vector2(-Mathf.Sin(sunInclinason * Mathf.PI /360),-Mathf.Cos(sunInclinason * Mathf.PI / 360)) ,sunInclinason, radius+10);
        Handles.DrawLine(RotatePos(90 - sunInclinason/2),RotatePos(90 - sunInclinason/2,radius+15));
        Handles.DrawLine(actualCenter, RotatePos(light.transform.eulerAngles.x,radius + 10 - rectSize/2));
        
        Handles.DrawLine(RotatePos(90), RotatePos(90,radius + 15));
        UnityEditor.Handles.DrawSolidRectangleWithOutline(inclineRect, clickIncline?Color.blue:new Color(0,0,1,0.2f), Color.blue);
    }
    #endregion
    
    private Vector2 RotateDir(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.PI / 180), -Mathf.Sin(angle * Mathf.PI / 180));
    }
    private Vector2 RotatePos(float angle, float radius)
    {
        Vector2 pos = actualCenter;
        pos += RotateDir(angle) * radius;
        return pos;
    }

    private Vector2 RotatePos(float angle)
    {
        return RotatePos(angle, radius);
    }
}

