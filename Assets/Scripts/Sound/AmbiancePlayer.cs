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
            AkSoundEngine.PostEvent("Amb_Int",gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Amb_Ext",gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Debug.Log("J'entre");
            ++nbVolume;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Debug.Log("Je sors");
            --nbVolume;
        }
    }
}
