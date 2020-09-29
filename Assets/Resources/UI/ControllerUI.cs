using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerUI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 19)
        {
            Debug.Log("Open TUTO");
            other.gameObject.GetComponent<UITuto>().tuto.SetBool("Play", true);
        }
    }
}
