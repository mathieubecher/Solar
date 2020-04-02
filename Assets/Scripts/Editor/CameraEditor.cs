using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(CameraController))]
public class CameraEditor : Editor
{
    float minVal   = -90;
    float minLimit = -90;
    float maxVal   =  90;
    float maxLimit =  90;
    double TOLERANCE = 0.02f;
    public override void OnInspectorGUI()
    {
        var camera = (CameraController) target;
        DrawCameraAngle(camera);
        camera.distance = EditorGUILayout.Slider("Distance", camera.distance, 0, 50);
        EditorGUILayout.MinMaxSlider("Rotation Limit", ref minVal, ref maxVal, minLimit, maxLimit);
        camera.RotateSpeed = EditorGUILayout.Slider("Rotate Speed", camera.RotateSpeed,0,200);
        camera.Speed = EditorGUILayout.Slider("Dezoom Speed", camera.Speed,0,10);

        if (Math.Abs(camera.BottomMaxAngle - minVal) > TOLERANCE)
        {
            camera.BottomMaxAngle = minVal;
        }

        if (Math.Abs(camera.UpMaxAngle - maxVal) > TOLERANCE)
        {
            camera.UpMaxAngle = maxVal;
        }
        

        EditorGUILayout.Space();
        
        if (camera.gizmos && GUILayout.Button("Disable Gizmos")) camera.gizmos = false;
        else if (!camera.gizmos && GUILayout.Button("Enable Gizmos")) camera.gizmos = true;
        
        if(GUI.changed && !Application.isPlaying){
            EditorUtility.SetDirty(camera);
            EditorSceneManager.MarkSceneDirty(camera.gameObject.scene);
        }

    }

    private Material mat;

    private void OnEnable()
    {
        var camera = (CameraController) target;
        mat = new Material(Shader.Find("Hidden/Internal-Colored"));
        minVal = camera.BottomMaxAngle;
        maxVal = camera.UpMaxAngle;
    }
    
    private void DrawCameraAngle(CameraController camera)
    {
        
        
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        Rect rect = GUILayoutUtility.GetRect(10, 1000, 200, 200);
        if (Event.current.type == EventType.Repaint){
            GUI.BeginClip(rect);
            GL.PushMatrix();
            
            GL.Clear(true, false, Color.black);
            mat.SetPass(0);
            // Cadre
            GL.Begin(GL.QUADS);
            GL.Color(Color.black);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0,  rect.height, 0);
            GL.Vertex3(rect.width, rect.height, 0);
            GL.Vertex3(rect.width,0, 0);
            GL.End();    
            
            // Camera Limit
            float rectHeight = rect.height/2 - 20;
            
            Vector2 center = new Vector2(rect.width/2, rect.height/2);
            
            // Limit
            GL.Begin(GL.LINES);
            GL.Color(Color.white);
            GL.Vertex3(center.x, center.y, 0);
            GL.Vertex3(center.x + Mathf.Cos(maxVal*Mathf.PI/180) * (rectHeight+10), center.y - Mathf.Sin(maxVal*Mathf.PI/180) * (rectHeight+10), 0);
            GL.Vertex3(center.x, center.y, 0);
            GL.Vertex3(center.x + Mathf.Cos(minVal*Mathf.PI/180) * (rectHeight+10), center.y - Mathf.Sin(minVal*Mathf.PI/180) * (rectHeight+10), 0);
            GL.End();
            // Arc de cercle
            DrawArc(center.x,center.y,rectHeight,-camera.UpMaxAngle*Mathf.PI/180,(camera.UpMaxAngle-camera.BottomMaxAngle)*Mathf.PI/180,(int)(((camera.UpMaxAngle-camera.BottomMaxAngle)/180*50)),Color.white);
            DrawArc(center.x,center.y,rectHeight,-camera.UpMaxAngle*Mathf.PI/180,((camera.UpMaxAngle-camera.BottomMaxAngle)-360)*Mathf.PI/180, (int)((360-(camera.UpMaxAngle-camera.BottomMaxAngle))/180*50),Color.gray);
            
            // Cam Pos
            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex3(center.x, center.y, 0);
            float rotateCam = camera.transform.eulerAngles.x;
            Vector3 camPos = new Vector2(center.x + Mathf.Cos(rotateCam * Mathf.PI / 180) * (rectHeight-5), center.y - Mathf.Sin(rotateCam * Mathf.PI / 180) * (rectHeight-5));
            GL.Vertex3(camPos.x, camPos.y, 0);
            GL.End();
            
            // Cam Icon
            GL.Begin(GL.LINE_STRIP);
            GL.Color(Color.red);
            Vector3 camPos2 = new Vector2(center.x + Mathf.Cos(rotateCam * Mathf.PI / 180) * rectHeight, center.y - Mathf.Sin(rotateCam * Mathf.PI / 180) * rectHeight);
            GL.Vertex3(camPos2.x + Mathf.Sin(rotateCam*Mathf.PI/180)*2, camPos2.y + Mathf.Cos(rotateCam*Mathf.PI/180)*2, 0);
            GL.Vertex3(camPos.x + Mathf.Sin(rotateCam*Mathf.PI/180)*5, camPos.y + Mathf.Cos(rotateCam*Mathf.PI/180)*5, 0);
            GL.Vertex3(camPos.x - Mathf.Sin(rotateCam*Mathf.PI/180)*5, camPos.y - Mathf.Cos(rotateCam*Mathf.PI/180)*5, 0);
            GL.Vertex3(camPos2.x - Mathf.Sin(rotateCam*Mathf.PI/180)*2, camPos2.y - Mathf.Cos(rotateCam*Mathf.PI/180)*2, 0);
            GL.End();
            GL.Begin(GL.LINE_STRIP);
            GL.Color(Color.red);
            Vector3 camPos3 = new Vector2(center.x + Mathf.Cos(rotateCam * Mathf.PI / 180) * (rectHeight + 10), center.y - Mathf.Sin(rotateCam * Mathf.PI / 180) * (rectHeight + 10));
            GL.Vertex3(camPos2.x + Mathf.Sin(rotateCam*Mathf.PI/180)*5, camPos2.y + Mathf.Cos(rotateCam*Mathf.PI/180)*5, 0);
            GL.Vertex3(camPos3.x + Mathf.Sin(rotateCam*Mathf.PI/180)*5, camPos3.y + Mathf.Cos(rotateCam*Mathf.PI/180)*5, 0);
            GL.Vertex3(camPos3.x - Mathf.Sin(rotateCam*Mathf.PI/180)*5, camPos3.y - Mathf.Cos(rotateCam*Mathf.PI/180)*5, 0);
            GL.Vertex3(camPos2.x - Mathf.Sin(rotateCam*Mathf.PI/180)*5, camPos2.y - Mathf.Cos(rotateCam*Mathf.PI/180)*5, 0);
            GL.Vertex3(camPos2.x + Mathf.Sin(rotateCam*Mathf.PI/180)*5, camPos2.y + Mathf.Cos(rotateCam*Mathf.PI/180)*5, 0);
            GL.End();
            
            GL.PopMatrix();
            GUI.EndClip();
            
        }
        
        
        GUILayout.EndHorizontal();
    }
    void DrawArc(float cx, float cy, float r, float start_angle, float arc_angle, int num_segments, Color color)
    {
        float theta = arc_angle / (float)(num_segments - 1);//theta is now calculated from the arc angle instead, the - 1 bit comes from the fact that the arc is open

        float tangetial_factor = Mathf.Tan(theta);

        float radial_factor = Mathf.Cos(theta);


        float x = r * Mathf.Cos(start_angle);//we now start at the start angle
        float y = r * Mathf.Sin(start_angle); 

        GL.Begin(GL.LINES);//since the arc is not a closed curve, this is a strip now
        GL.Color(color);
        for(int ii = 0; ii < num_segments; ii++)
        { 
            GL.Vertex3(x + cx, y + cy,0);

            float tx = -y; 
            float ty = x; 

            x += tx * tangetial_factor; 
            y += ty * tangetial_factor; 

            x *= radial_factor; 
            y *= radial_factor; 
        } 
        GL.End(); 
    }
}
