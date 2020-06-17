using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimVCam : MonoBehaviour
{
    public Animator door;

    public void OpenDoor()
    {
        door.SetBool("active",true);
    }

    public void CloseDoor()
    {
        door.SetBool("active",false);
        
    }
}
