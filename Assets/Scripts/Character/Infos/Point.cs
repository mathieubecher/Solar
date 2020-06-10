using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private bool _touch;
    public bool Touch => _touch;
    private float _speedDamage = 0.5f;

    private Material _parentMat;
    private float _damageValue = 0;
    private float dist = 200;

    [SerializeField] private float pointForce = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //_damageValue = Mathf.Max(0, Mathf.Min(1,_damageValue + ((_touch)?_speedDamage:-_speedHeal) * Time.deltaTime));
        //_parentMat.SetFloat("_sunTouch", _damageValue);
    }
    
    
    /// <summary>
    /// Regarde si le soleil atteint le point de contact.
    /// </summary>
    /// <param name="sun">Directional light</param>
    /// <returns></returns>
    public float TestLight(LightController sun)
    {
        
        if (Physics.Raycast(origin: transform.position, direction: sun.transform.rotation * Vector3.back, hitInfo: out RaycastHit hit, maxDistance:dist, layerMask: GameManager.mask))
        {
            _touch = false;
            _damageValue = 0;
        }
        else
        {
            _touch = true;
            _damageValue = pointForce;
        }

        return _damageValue;

    }
    /// <summary>
    /// Regarde si le soleil atteint le point de contact.
    /// </summary>
    /// <param name="sun">Directional light</param>
    /// <param name="test">Est ce qu'il faut revérifier</param>
    /// <returns></returns>
    public float TestLight(LightController sun, bool test)
    {
        if (test) return _damageValue;
        return TestLight(sun);
    }
    
    /// <summary>
    /// Réinitialise le point
    /// </summary>
    public void ResetPoint()
    {
        _damageValue = 0;
    }

    // Affiche une sphere rouge ou verte en fonction de la détection
    private void OnDrawGizmos()
    {
        Gizmos.color = (_touch)?Color.red:Color.green;
        Gizmos.DrawSphere(transform.position,0.1f);
    }

}
