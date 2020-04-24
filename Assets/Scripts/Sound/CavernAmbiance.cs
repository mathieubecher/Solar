using UnityEngine;

public class CavernAmbiance : MonoBehaviour
{
    void Start()
    {
        
#if UNITY_EDITOR
#else
        gameObject.GetComponent<MeshRenderer>().enabled = false;


#endif
    }
}
