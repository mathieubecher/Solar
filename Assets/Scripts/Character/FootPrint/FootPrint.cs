using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FootPrint : MonoBehaviour
{
    private float _lifeTime = 5;
    private FootPrints foots;

    private Material m;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _lifeTime -= Time.deltaTime;
        if(_lifeTime<0) Destroy(this.gameObject);
    }
}
