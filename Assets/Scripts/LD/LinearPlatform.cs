using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plateforme à trajectoire linéaire
/// </summary>
public class LinearPlatform : Platform
{
    [SerializeField] private Transform path;
    private float maxDist;

    private List<Vector3> pathPoints;

    // Start is called before the first frame update
    void Awake()
    {
        _server = GetComponent<PlatformServer>();
        // Récupération de la liste des points de la trajectoire
        pathPoints = new List<Vector3>();
        pathPoints.Add(path.GetChild(0).position);
        for (int i = 1; i < path.childCount; ++i)
        {
            pathPoints.Add(path.GetChild(i).position);
            maxDist += (pathPoints[i] - pathPoints[i - 1]).magnitude;
        }

        // Récupération de la liste des plateformes
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
        // Récupere la valeur de progress en fonction du type de la partie
        InputVelocity();
        
        // Met à jour la position de la plateforme avec progress
        for (int i = 0; i < childs.Count; ++i)
        {
            childs[i].position = progressPos();
        }
    }
    
    /// <summary>
    /// Cherche la position de la plateforme en fonction de progress.
    /// </summary>
    /// <returns>Position actuel de la plateforme</returns>
    Vector3 progressPos()
    {
        float total = ((GetLocalProgress()) * maxDist)%maxDist;
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
        // Progression de la plateforme dans le temps pour tester
        if (!Application.isPlaying) progress += Time.deltaTime * speed;
        
        // Dessin de la trajectoire
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

        // Dessin de la position de chaque plateforme
        Gizmos.color = Color.black;
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (path != transform.GetChild(i) && !Application.isPlaying) Gizmos.DrawSphere(progressPos(), 1f);
        }
    }
#endif

   
}
