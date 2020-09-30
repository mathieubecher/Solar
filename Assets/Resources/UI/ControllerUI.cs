using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerUI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 19 && (FindObjectOfType<GameManager>().gameType == GameManager.GameType.SOLO || StaticClass.serverType == StaticClass.ServerType.SUN))
        {
            Debug.Log("Open TUTO");
            UITuto tuto = other.gameObject.GetComponent<UITuto>();
            tuto.tuto.gameObject.SetActive(true);
            tuto.tuto.SetBool("Play", true);
            tuto.active = tuto.first;
            tuto.first = false;
            tuto.timer = 20;
        }
    }
}
