
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class GizmosSun : MonoBehaviour
{
#if UNITY_EDITOR
    private LightController _light;
    [SerializeField] private Vector2 center= new Vector2(80,80);
    private Vector2 actualCenter;
    [SerializeField]
    private float radius = 50;
    [SerializeField] private float rectSize = 4;
    [Range(0,360)]
    [SerializeField] private float sunInclinason = 120;
    
    private bool clickRotate;
    private bool clickIncline;
    private bool move;

    private bool hoverRotate;
    private bool hoverIncline;
    private bool hoverMove;
    
    private EventType type;

    // Start is called before the first frame update
    void Awake()
    {
        //Destroy(gameObject);
        _light = FindObjectOfType<LightController>();
        actualCenter = center;
        
        SceneView.onSceneGUIDelegate += UpdateSceneView;
    }

    void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= UpdateSceneView;
    }
    void Update()
    {
        
    }

    private Vector2 mousepos;
    private void OnDrawGizmos()
    {
        if (!_light.gizmos) return;
        // Draw interface 
        Handles.BeginGUI();
        
        DrawIncline();
        DrawRotation();
        Handles.color = Color.white;
        Handles.DrawSolidDisc(actualCenter, Vector3.forward, 5);
       
        Handles.EndGUI();
       
    }

    private void OnGUI()
    {

    }
    
    private void UpdateSceneView(SceneView sceneView)
    {
        if (!_light.gizmos) return;
        
        _light =  FindObjectOfType<LightController>();
        if (_light == null) return;
        mousepos = Event.current.mousePosition;
        DefineIncline();
        DefineRotation();
        SetPos();
        
        if(hoverIncline || hoverRotate || hoverMove || clickIncline || clickRotate || move){
            type = (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp)?Event.current.type:type;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else
        {
            type = EventType.Ignore;
        }
        
        
    }
    private void SetPos()
    {
        hoverMove = Vector2.Distance(actualCenter, mousepos) < 5; 
        if (!move && hoverMove && type == EventType.MouseDown) move = true;
        else if (move  && (type == EventType.MouseUp || type == EventType.MouseLeaveWindow)) move = false;
        
        
        if (move)
        {
            clickIncline = false;
            clickRotate = false;
            actualCenter = mousepos;
        }
    }

    /******************************************************************/
    /*                                                                */
    /*                            ROTATION                            */
    /*                                                                */
    /******************************************************************/ 
    #region ROTATION
    void DefineRotation()
    {
        var eulerAngles = _light.transform.eulerAngles;
        float angle = eulerAngles.y * Mathf.PI / 180;
        Vector2 rotatePos = RotatePos(eulerAngles.y);
        Rect rect = new Rect(rotatePos.x - rectSize/2, rotatePos.y - rectSize/2, rectSize, rectSize);
        

        hoverRotate = rect.Contains(mousepos) || clickRotate;
        if (!clickRotate && hoverRotate && type == EventType.MouseDown)
        {
            clickRotate = true;
            clickIncline = false;
        }
        
        // Action on Click
        if (clickRotate)
        {
            Vector3 rotation = _light.transform.eulerAngles;
            rotation.y = Vector2.SignedAngle(mousepos-actualCenter, Vector2.right);
            _light.transform.eulerAngles = rotation;
            rotatePos = RotatePos(eulerAngles.y);
            rect = new Rect(rotatePos.x - rectSize/2, rotatePos.y - rectSize/2, rectSize, rectSize);

            // Leave condition
            if( type == EventType.MouseUp || type == EventType.MouseLeaveWindow) clickRotate = false;
            
        }
    }
    
    void DrawRotation()
    {
        var eulerAngles = _light.transform.eulerAngles;
        Vector2 rotatePos = RotatePos(eulerAngles.y);
        Rect rect = new Rect(rotatePos.x - rectSize/2, rotatePos.y - rectSize/2, rectSize, rectSize);
        Handles.color = Color.red;
        Handles.DrawWireArc(actualCenter,Vector3.forward, RotateDir(eulerAngles.y+rectSize/2), -360 + rectSize, radius);
        Handles.DrawLine(actualCenter,RotatePos(eulerAngles.y, radius-rectSize/2 ));
        Handles.DrawLine(actualCenter,RotatePos(0));
        UnityEditor.Handles.DrawSolidRectangleWithOutline(rect, clickRotate?Color.red:new Color(1,0,0,0.2f), Color.red);
        Handles.color = new Color(1,0,0,0.1f);
        if(_light.transform.eulerAngles.y > 180) Handles.DrawSolidArc(actualCenter,Vector3.forward,RotateDir(eulerAngles.y) ,-360+eulerAngles.y, radius);
        else Handles.DrawSolidArc(actualCenter,Vector3.forward,Vector2.right ,-_light.transform.eulerAngles.y, radius);
    }
    #endregion
    
    
    /******************************************************************/
    /*                                                                */
    /*                            INCLINE                             */
    /*                                                                */
    /******************************************************************/
    #region INCLINE
    private void DefineIncline()
    {   
        var eulerAngles = _light.transform.eulerAngles;
        float incline = eulerAngles.x * Mathf.PI / 180;
        Vector2 inclinePos = RotatePos(eulerAngles.x, radius + 10);
        Rect rect = new Rect(inclinePos.x - rectSize/2, inclinePos.y - rectSize/2, rectSize, rectSize);
        hoverIncline = rect.Contains(mousepos) || clickIncline;
        if (!clickRotate && hoverIncline  && type == EventType.MouseDown)
        {
            clickIncline = true;
            clickRotate = false;
        }
        
        if (clickIncline)
        {
            Vector3 rotation = _light.transform.eulerAngles;
            rotation.x = Mathf.Max(90 - sunInclinason/2,Mathf.Min(90, Vector2.SignedAngle(mousepos-actualCenter, Vector2.right)));
            _light.transform.eulerAngles = rotation;
            inclinePos = RotatePos(eulerAngles.x, radius + 10);
            rect = new Rect(inclinePos.x - rectSize/2, inclinePos.y - rectSize/2, rectSize, rectSize);
            
            // Leave condition
            if(type == EventType.MouseUp || type == EventType.MouseLeaveWindow) clickIncline = false;
            
        }
    }
    void DrawIncline()
    {
        var eulerAngles = _light.transform.eulerAngles;
        Vector2 inclinePos = RotatePos(eulerAngles.x, radius + 10);
        Rect inclineRect = new Rect(inclinePos.x - rectSize/2, inclinePos.y - rectSize/2, rectSize, rectSize);
        Handles.color = new Color(0, 0, 1, 1);
        Handles.DrawWireArc(actualCenter,Vector3.forward, new Vector2(-Mathf.Sin(sunInclinason * Mathf.PI /360),-Mathf.Cos(sunInclinason * Mathf.PI / 360)) ,sunInclinason, radius+10);
        Handles.DrawLine(RotatePos(90 - sunInclinason/2),RotatePos(90 - sunInclinason/2,radius+15));
        Handles.DrawLine(actualCenter, RotatePos(_light.transform.eulerAngles.x,radius + 10 - rectSize/2));
        
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
#endif
}
