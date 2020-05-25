using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseMusic : MonoBehaviour
{
    public string type;
    
    void Start()
    {
        
#if UNITY_EDITOR
#else
        gameObject.GetComponent<MeshRenderer>().enabled = false;


#endif
    }

}
