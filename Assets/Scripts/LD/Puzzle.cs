using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject respawn;

    public CMCamera cam;

    public float beginRotate;
    // Start is called before the first frame update
    public Vector3 GetRespawnPoint()
    {
        return respawn.transform.position;
    }

    /// <summary>
    /// Fonction appelé quand le joueur arrive dans un nouveau spawn
    /// </summary>
    /// <param name="sunGotoAngle"></param>
    public void Enter(float sunGotoAngle)
    {
        //TODO Nouveau spawn
        beginRotate = sunGotoAngle;
    }
}
