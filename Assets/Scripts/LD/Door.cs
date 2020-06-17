using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator door;
    

    private float timer;
    private bool active;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        Open();
    }

    public void Open()
    {
        door.SetBool("active",true);
        AkSoundEngine.PostEvent("TempleDoor_Open", gameObject);
        active = true;
    }
}
