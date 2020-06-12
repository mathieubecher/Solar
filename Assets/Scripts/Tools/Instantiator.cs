#if UNITY_EDITOR
using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using  System.Collections.Generic;
using System.Runtime.InteropServices;
using AK.Wwise;
using UnityEngine.SceneManagement;
using Event = UnityEngine.Event;

[ExecuteInEditMode]
public class Instantiator : MonoBehaviour
{
    public struct IntPoint
    {
        public int x, y;
    }
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out IntPoint pos);
    
    private struct MeshRenderInfo
    {
        public Mesh mesh;
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;

        public MeshRenderInfo(Mesh _mesh, Vector3 _position, Quaternion _rotation, Vector3 _scale)
        {
            mesh = _mesh;
            position = _position;
            scale = _scale;
            rotation = _rotation;
        }
    }


   
    public bool active = false;

    public bool Active
    {
        get => active;
        set => active = value;
    }


    private bool _firstclick = true;
    [Range(0, 100)] public float margey;
    private GameObject last;

    [SerializeField] private List<GameObject> prefabs;
    private int indicePrefab;
    private GameObject prefab;
    
    [SerializeField]
    private GameObject parent;
    private List<GameObject> _instances;
    
    private bool change;
    
    private System.Collections.Generic.List<MeshRenderInfo> draw;
    
    
    void OnEnable()
    {
        if(prefabs.Count> 0) prefab = prefabs[0];
        active = false;
        scale = 1;
        
        Debug.Log("awake");
        SceneView.onSceneGUIDelegate += UpdateSceneView;
        _instances = new List<GameObject>();
        draw = new System.Collections.Generic.List<MeshRenderInfo>();
        change = true;

    }

    void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= UpdateSceneView;
    }
    
    
    //[Header("Infos control")]
    private EventType type;
    private bool move;
    private bool click;
    private int currentButton = 0;
    private bool alt;
    private bool shift;
    private bool ctrl;
    private Vector2 mousepos;
    private IntPoint mouseValuePos;
    private void UpdateSceneView(SceneView sceneview)
    {
        alt = Event.current.alt;
        shift = Event.current.shift;
        ctrl = Event.current.control;
        
        if (Event.current.keyCode == KeyCode.Escape) active = false;
        
        //_firstclick &= type == EventType.MouseDown && currentButton == 0;
        if (active) //&& !_firstclick)
        {
            //Reset if list change
            if (prefabs.Count == 0)
            {
                prefab = null;
                draw = new List<MeshRenderInfo>();
            }
            else if (prefabs.Count <= indicePrefab)
            {
                indicePrefab = 0;
                prefab = prefabs[indicePrefab];
            }
            else prefab = prefabs[indicePrefab];

            if (!move && type == EventType.MouseDown && currentButton == 0)
            {
                click = true;
                move = true;
                
                GetCursorPos(out mouseValuePos);
                mousepos = Event.current.mousePosition;
                hasInstance = false;
            }
            else if (move && type == EventType.MouseUp)
            {
                move = false;
                if (instance != null)
                {
                    Debug.Log("undo");
                }
            }

            type = (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp)?Event.current.type:type;
            if (Event.current.type == EventType.MouseDown) currentButton = Event.current.button;
        
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (Event.current.type == EventType.ScrollWheel && shift)
            {
                Event.current.Use();
                if (Event.current.delta.y > 0) scale -= 0.1f;
                else scale += 0.1f;
                scale = Math.Min(2, Math.Max(0, scale));
            }

            if (Event.current.isKey)
            {
                int newIndice = -1;
                switch(Event.current.keyCode)
                {
                    case KeyCode.Alpha1:
                        newIndice = 0;
                        break;
                    case KeyCode.Alpha2:
                        newIndice = 1;
                        break;
                    case KeyCode.Alpha3:
                        newIndice = 2;
                        break;
                    case KeyCode.Alpha4:
                        newIndice = 3;
                        break;
                    case KeyCode.Alpha5:
                        newIndice = 4;
                        break;
                    case KeyCode.Alpha6:
                        newIndice = 5;
                        break;
                    case KeyCode.Alpha7:
                        newIndice = 6;
                        break;
                    case KeyCode.Alpha8:
                        newIndice = 7;
                        break;
                    case KeyCode.Alpha9:
                        newIndice = 8;
                        break;
                    case KeyCode.Alpha0:
                        newIndice = 9;
                        break;
                }

                if (newIndice >= 0 && newIndice != indicePrefab && prefabs.Count > newIndice)
                {
                    indicePrefab = newIndice;
                    prefab = prefabs[indicePrefab];
                }
                if(newIndice>= 0) Event.current.Use();
            }
        }
        else if(_instances.Count > 0) _instances = new List<GameObject>();

        if (!active) _firstclick = true;
    }
    
    private GameObject instance;
    private Vector3 normalInstance;
    private bool hasInstance;
    [SerializeField,Range(0,2)]
    private float scale = 1;
    void OnDrawGizmos()
    {
        if (active)
        {
            Vector2 mouse = Event.current.mousePosition;
            mouse.y = Screen.height - margey - mouse.y;
            RaycastHit hit;
            Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(mouse);

            if (ctrl && click)
            {
                Debug.Log("Search prefab");
                SearchParent();
                
            }
            // 
            else if (move && !click && hasInstance)
            {
                if (ctrl)
                {
                    instance.transform.RotateAround(instance.transform.position,SceneView.lastActiveSceneView.camera.transform.rotation * Vector3.up,(mousepos.x - Event.current.mousePosition.x));
                    instance.transform.RotateAround(instance.transform.position,SceneView.lastActiveSceneView.camera.transform.rotation *Vector3.right,(mousepos.y - Event.current.mousePosition.y));
                    
                    UnityEditor.Handles.color = Color.blue;
                    UnityEditor.Handles.DrawWireDisc(instance.transform.position, SceneView.lastActiveSceneView.camera.transform.rotation *Vector3.right, 2);
                    UnityEditor.Handles.color = Color.red;
                    UnityEditor.Handles.DrawWireDisc(instance.transform.position, SceneView.lastActiveSceneView.camera.transform.rotation *Vector3.up, 2);
                    
                    SetCursorPos(mouseValuePos.x, mouseValuePos.y);
                    
                }
                else if (shift)
                {
                    Vector3 forward = Vector3.Cross(Vector3.up, SceneView.lastActiveSceneView.camera.transform.rotation * Vector3.right);
                    Vector3 right = Vector3.Cross(Vector3.up, forward);
                    instance.transform.position += right * (mousepos.x - Event.current.mousePosition.x)/40 + forward * (Event.current.mousePosition.y - mousepos.y)/40;
                    
                    
                    UnityEditor.Handles.color = Color.blue;
                    UnityEditor.Handles.DrawLine(instance.transform.position, instance.transform.position + Vector3.forward*2);
                    UnityEditor.Handles.color = Color.red;
                    UnityEditor.Handles.DrawLine(instance.transform.position, instance.transform.position + Vector3.right*2);
                    
                    SetCursorPos(mouseValuePos.x, mouseValuePos.y);
                }
                else
                {
                    Vector3 up = instance.transform.rotation * Vector3.up;
                    instance.transform.Rotate(new Vector3(0,(mousepos.x - Event.current.mousePosition.x),0));
                    instance.transform.position = instance.transform.position + up * (mousepos.y - Event.current.mousePosition.y)/50;
                    
                    UnityEditor.Handles.color = Color.blue;
                    UnityEditor.Handles.DrawWireDisc(instance.transform.position, up, 2);
                    UnityEditor.Handles.color = Color.red;
                    UnityEditor.Handles.DrawLine(instance.transform.position, instance.transform.position + up*2);
                    
                    SetCursorPos(mouseValuePos.x, mouseValuePos.y);
                    
                    
                }

            }
            
            // Define raycast target
            else if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("Sand") + LayerMask.GetMask("Default")))
            {
                // Spawn Prefab
                if (click && !alt && !shift && prefab)
                {
                    GameObject searchParent = hit.collider.gameObject;
                    SearchParent();
                    
                    mousepos = Event.current.mousePosition;
                    instance =  (GameObject)PrefabUtility.InstantiatePrefab(prefab,parent.transform);
                    Undo.RegisterCreatedObjectUndo(instance, "Created instance");
                    normalInstance = hit.normal;
                    instance.transform.position = hit.point;
                    instance.transform.localScale = scale * instance.transform.localScale;
                    
                    _instances.Add(instance);
                    hasInstance = true;
                    mouseValuePos = new IntPoint();
                    GetCursorPos(out mouseValuePos);
                    SetCursorPos(mouseValuePos.x, mouseValuePos.y);
                    mousepos = Event.current.mousePosition;

                    

                }
                // Delete Prefab spawn with raycast
                else if (click && alt)
                {
                    if (_instances.Find(ctx => hit.collider.gameObject == ctx))
                    {
                        _instances.Remove(hit.collider.gameObject);
                        Undo.DestroyObjectImmediate(hit.collider.gameObject);
                    }
                    else
                    {
                        GameObject searchInstance = hit.collider.gameObject;
                        while (searchInstance.name != "archi" && searchInstance.name != "Archi" && searchInstance.transform.parent != null)
                        {
                            if (_instances.Find(ctx => searchInstance == ctx))
                            {   
                                
                                _instances.Remove(searchInstance);
                                Undo.DestroyObjectImmediate(searchInstance);
                                break;
                            }
                            else searchInstance = searchInstance.transform.parent.gameObject;
                        }
                    }
                }
                else if (click && shift)
                {
                    if (_instances.Find(ctx => hit.collider.gameObject == ctx))
                    {
                        hasInstance = true;
                        instance = hit.collider.gameObject;
                        Undo.RegisterCompleteObjectUndo(instance.transform, "Transform Position "+instance.name);
                        instance.transform.position = instance.transform.position;
                    }
                    else
                    {
                        GameObject searchInstance = hit.collider.gameObject;
                        while (searchInstance.name != "archi" && searchInstance.name != "Archi" && searchInstance.transform.parent != null)
                        {
                            if (_instances.Find(ctx => searchInstance == ctx))
                            {
                                hasInstance = true;
                                instance = searchInstance;
                                
                                break;
                            }
                            else searchInstance = searchInstance.transform.parent.gameObject;
                        }
                    }
                }
                // Draw Debug
                else
                {
                    Transform objectHit = hit.transform;
                    Gizmos.color = new Color(0, 0, 1, 0.5f);

                    if (prefab == null) draw = new List<MeshRenderInfo>();
                    if ((last != prefab || change) && (prefab != null || prefabs.Count>0))
                    {
                        if (prefab == null || change) prefab = prefabs[indicePrefab];
                        scale = 1;
                        change = false;
                        draw = new List<MeshRenderInfo>();
                        GetMesh(prefab);
                        last = prefab;
                    }

                    foreach (MeshRenderInfo mesh in draw)
                        Gizmos.DrawMesh(mesh.mesh, mesh.position * scale + hit.point, mesh.rotation, mesh.scale * scale);
                }
            }

            click = false;
        }
    }

    private void SearchParent()
    {
        Vector2 mouse = Event.current.mousePosition;
        mouse.y = Screen.height - margey - mouse.y;
        RaycastHit hit;
        Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(mouse);
        if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("Default")))
        {
            GameObject searchParent = hit.collider.gameObject;
            while (searchParent.transform.parent != null && (searchParent.name != "Archi" && searchParent.name != "archi"))
            {
                searchParent = searchParent.transform.parent.gameObject;
            }

            parent = searchParent;
                    
        }
        

    }

    void GetMesh(GameObject o)
    {
        if (o.TryGetComponent(out MeshFilter mesh)) draw.Add(new MeshRenderInfo(mesh.sharedMesh, o.transform.position, o.transform.rotation, o.transform.localScale));
        for(int i = 0; i < o.transform.childCount; ++i) GetMesh(o.transform.GetChild(i).gameObject);
    }

}
#endif
#if UNITY_EDITOR
[CustomEditor(typeof(Instantiator))]
public class InstantiatorEditor: Editor
{
    private bool active;
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button((active) ? "Disable" : "Enable"))
        {
            active = !active;
            ((Instantiator) target).Active = active;
        }
        base.OnInspectorGUI();
        
    }
}
#endif