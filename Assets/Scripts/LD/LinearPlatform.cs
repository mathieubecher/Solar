using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearPlatform : Platform
{
    [SerializeField] private Transform path;
    private float maxDist;

    private List<Vector3> pathPoints;

    // Start is called before the first frame update
    void Awake()
    {
        pathPoints = new List<Vector3>();
        pathPoints.Add(path.GetChild(0).position);
        for (int i = 1; i < path.childCount; ++i)
        {
            pathPoints.Add(path.GetChild(i).position);
            maxDist += (pathPoints[i] - pathPoints[i - 1]).magnitude;
        }

        childs = new List<Transform>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (path != transform.GetChild(i))
            {
                childs.Add(transform.GetChild(i));
                transform.GetChild(i).position = progressPos();
            }
        }

        progress = 0;
    }


    // Update is called once per frame
    protected override void Update()
    {
        if ((StaticClass.gameType == GameManager.GameType.SERVER ||
             StaticClass.gameType == GameManager.GameType.CLIENT))
        {
            if(StaticClass.serverType == StaticClass.ServerType.SUN)
            {
                _server.CallSetProgress(progress);
            }
            else progress = _server.GetProgress();
        }
        else
        {
            progress += _velocity * speed * Time.deltaTime;
            progress = Mathf.Max(0, Mathf.Min(0.9999f, progress));
        }
        
        for (int i = 0; i < childs.Count; ++i)
        {
            childs[i].position = progressPos();
        }
    }
    Vector3 progressPos()
    {
        float total = (GetLocalProgress()+begin_progress)%1 * maxDist;
        for (int i = 1; i < pathPoints.Count; ++i)
        {
            float actualDist = (pathPoints[i] - pathPoints[i - 1]).magnitude;
            if (total <= actualDist) return Vector3.Lerp(pathPoints[i - 1], pathPoints[i], total/actualDist);
            else total -= actualDist;
        }
        return Vector3.zero;
    }
    
    
    
#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        if (!Application.isPlaying) progress += Time.deltaTime * speed;
        Gizmos.color = Color.white;
        pathPoints = new List<Vector3>();
        pathPoints.Add(path.GetChild(0).position);
        maxDist = 0;
        for (int i = 1; i < path.childCount; ++i)
        {
            pathPoints.Add(path.GetChild(i).position);
            maxDist += (pathPoints[i] - pathPoints[i - 1]).magnitude;
            Gizmos.DrawLine(pathPoints[i - 1], pathPoints[i]);
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (path != transform.GetChild(i) && !Application.isPlaying) Gizmos.DrawSphere(progressPos(), 1f);
        }
    }
#endif

   
}
