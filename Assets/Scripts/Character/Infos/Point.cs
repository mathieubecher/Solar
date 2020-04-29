using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private bool _touch;
    private int _mask;
    private float _speedDamage = 0.5f;
    private float _speedHeal=1f;

    private Material _parentMat;
    private float _damageValue = 0;
    private float dist = 100;
    // Start is called before the first frame update
    void Start()
    {
        _mask =~ LayerMask.GetMask("Character");
        //_parentMat = GetComponentInParent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _damageValue = Mathf.Max(0, Mathf.Min(1,_damageValue + ((_touch)?_speedDamage:-_speedHeal) * Time.deltaTime));
        //_parentMat.SetFloat("_sunTouch", _damageValue);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = (_touch)?Color.red:Color.green;
        Gizmos.DrawSphere(transform.position,0.1f);
    }

    public float TestLight(LightController sun)
    {
        
        if (Physics.Raycast(origin: transform.position, direction: sun.transform.rotation * Vector3.back, hitInfo: out RaycastHit hit, maxDistance:dist, layerMask: _mask))
        {
            _touch = false;
        }
        else
        {
            _touch = true;
        }

        return _damageValue;

    }

    public void ResetPoint()
    {
        _damageValue = 0;
    }
}
