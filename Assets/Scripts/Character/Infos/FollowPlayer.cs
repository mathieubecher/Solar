using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script permetant à un gameObject de suivre le personnage
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    private CameraTarget c;
    void Start()
    {
        c = FindObjectOfType<CameraTarget>();
    }

    void Update()
    {
        transform.position = c.transform.position;
    }
}
