using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Light sun;
    private List<Point> points;
    // Start is called before the first frame update
    void Awake()
    {
        points = new List<Point>();
        GetPoints(this.gameObject);
        
    }


    void Start()
    {
        sun = FindObjectOfType<Light>();
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Point p in points)
        {
            p.TestLight(sun);
        }
    }
    
    
    private void GetPoints(GameObject obj)
    {
        if(obj.TryGetComponent<Point>(out var p)) points.Add(p);
        for (int i = 0; i < obj.transform.childCount; ++i)
        {
            GetPoints(obj.transform.GetChild(i).gameObject);
        }
    }


}
