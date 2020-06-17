using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiancePlayer : MonoBehaviour
{
    public int nbVolume = 0;
    public WwiseAbiance.TypeAmbiance type;
    
    void Update()
    {
        
        if (nbVolume > 0)
        {
            switch (type)
            {
                case WwiseAbiance.TypeAmbiance.AMB_INT:
                    AkSoundEngine.PostEvent("Amb_Int_Lvl01",gameObject);
                    break;
                case WwiseAbiance.TypeAmbiance.AMB_VIEW:
                    AkSoundEngine.PostEvent("Amb_Ext_Lvl01_Desert_View",gameObject);
                    break;
                case WwiseAbiance.TypeAmbiance.AMB_KIOSK:
                    AkSoundEngine.PostEvent("Amb_Ext_Lvl01_Desert_Kiosk",gameObject);
                    break;
                case WwiseAbiance.TypeAmbiance.AMB_PLACE:
                    AkSoundEngine.PostEvent("Amb_Ext_Lvl02_Place",gameObject);
                    break;
                case WwiseAbiance.TypeAmbiance.AMB_ENTRANCE:
                    AkSoundEngine.PostEvent("Amb_Ext_Lvl02_Entrance",gameObject);
                    break;
                case WwiseAbiance.TypeAmbiance.AMB_TUNNEL:
                    AkSoundEngine.PostEvent("Amb_Int_Tunnel_End", gameObject);
                    break;
                case WwiseAbiance.TypeAmbiance.AMB_TEMPLE_END:
                    AkSoundEngine.PostEvent("Amb_Int_Temple_End", gameObject);
                    break;
            }
        }
        else
        {
            AkSoundEngine.PostEvent("Amb_Ext_Lvl01_Desert_Global",gameObject);
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            type = other.gameObject.GetComponent<WwiseAbiance>().type;
            ++nbVolume;
        }
        else if (other.gameObject.layer == 15)
        {
            AkSoundEngine.PostEvent(other.gameObject.GetComponent<WwiseMusic>().type,gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            --nbVolume;
        }
    }
}
