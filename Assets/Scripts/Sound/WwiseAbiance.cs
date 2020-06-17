using UnityEngine;

public class WwiseAbiance : MonoBehaviour
{
    public enum TypeAmbiance
    {
        AMB_GLOBAL, AMB_INT, AMB_VIEW, AMB_KIOSK, AMB_PLACE, AMB_ENTRANCE, AMB_TUNNEL, AMB_TEMPLE_END
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
