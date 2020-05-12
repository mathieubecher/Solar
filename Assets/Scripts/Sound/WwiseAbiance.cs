using UnityEngine;

public class WwiseAbiance : MonoBehaviour
{
    public enum TypeAmbiance
    {
        AMB_GLOBAL, AMB_INT, AMB_VIEW, AMB_KIOSK
    }

    public TypeAmbiance type;
    void Start()
    {
        
#if UNITY_EDITOR
#else
        gameObject.GetComponent<MeshRenderer>().enabled = false;


#endif
    }
    
}
