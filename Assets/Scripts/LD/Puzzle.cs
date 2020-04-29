using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject respawn;

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
