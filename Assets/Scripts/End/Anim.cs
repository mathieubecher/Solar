using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{

    public bool active;
    public Watcher watcher;
    public Door door;
    private bool open;
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        active = true;
        Debug.Log("begin");
        watcher.SetActive();
    }

    public void OpenDoor()
    {
        if (!open)
        {
            door.Open();
            open = true;
            
        }
    }
}
