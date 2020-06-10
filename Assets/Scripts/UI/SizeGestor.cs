using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeGestor : MonoBehaviour
{
    public GameObject gestor;

    public GameObject full;

    public GameObject half;
    
    public void Half()
    {
        /*
        gestor.transform.parent = half.transform;
        gestor.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,0);
        */
    }

    public void Full()
    {
        /*
        gestor.transform.parent = full.transform;
        */
    }
}
