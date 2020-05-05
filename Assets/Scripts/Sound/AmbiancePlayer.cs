using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiancePlayer : MonoBehaviour
{
    public int nbVolume = 0;

    void Update()
    {
        
        if (nbVolume > 0)
        {
            AkSoundEngine.PostEvent("Amb_Int_Lvl01",gameObject);
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
            ++nbVolume;
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
