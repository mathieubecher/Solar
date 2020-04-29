using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject respawn;

    public CinemachineVirtualCamera cam;

    public float beginRotate;
    // Start is called before the first frame update
    public Vector3 GetRespawnPoint()
    {
        return respawn.transform.position;
    }

    public void Enter(float sunGotoAngle)
    {
        //TODO Nouveau spawn
        beginRotate = sunGotoAngle;
    }
}
