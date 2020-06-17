using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject respawn;
    public float beginRotate;

    public bool isActiveFirst;
    public GameObject clip;
    public Puzzle unClip;
    
    [Header("Transition info")]
    [SerializeField] private CMCamera last;
    public CMCamera cam;

    public CinemachineBlendDefinition.Style type = CinemachineBlendDefinition.Style.EaseInOut;
    [Range(0,5)] public float transitionTime = 1;

    void Awake()
    {
        if(!isActiveFirst && clip != null) clip.SetActive(false);
    }
    // Start is called before the first frame update
    public Vector3 GetRespawnPoint()
    {
        return respawn.transform.position;
    }

    public CameraCurve curve;
    /// <summary>
    /// Fonction appelé quand le joueur arrive dans un nouveau spawn
    /// </summary>
    /// <param name="sunGotoAngle"></param>
    public void Enter(float sunGotoAngle)
    {
        //TODO Nouveau spawn
        beginRotate = sunGotoAngle;
        if(unClip!= null && unClip.clip != null) unClip.clip.SetActive(true);
    }

    #if UNITY_EDITOR
    
    private List<Vector3> bezier;
    void OnDrawGizmos()
    {
        if (last != null && cam != null)
        {
            if(curve == null || curve.Points.Length == 0) Gizmos.DrawLine(last.transform.position, cam.transform.position);
            else{

                if (bezier == null || bezier.Count == 0 || PointChange())
                {
                    UpdateBezier();
                }
                for (int i = 1; i < bezier.Count; ++i)
                {
                    Gizmos.DrawLine(bezier[i-1], bezier[i]);
                }
            }
        }
    }

    
    void UpdateBezier()
    {
        bezier = new List<Vector3>();
        
        List<Vector3> points = new List<Vector3>();
        points.Add(last.transform.position);
        foreach (GizmosPoint point in curve.Points)
        {
            points.Add(point.transform.position);
        }
        points.Add(cam.transform.position);

        bezier.Add(last.transform.position);
        for (float i = 0.1f; i <= 1; i += 0.1f)
        {
            bezier.Add(CameraCurve.Bezier(points,i));
        }
        bezier.Add(cam.transform.position);
    }

    bool PointChange()
    {
        bool change = false;
        int i = 0;
        while (!change && i < curve.Points.Length)
        {
            change |= curve.Points[i].change;
            ++i;
        }
        
        return change;
    }
#endif
}
