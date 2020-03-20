using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Light[] lights;
    // Start is called before the first frame update
    void Start()
    {
        lights = FindObjectsOfType<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
