using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anim : MonoBehaviour
{

    public bool active;
    public Watcher watcher;
    public Door door;
    private bool open;
    public Animation cmcam;
    
    public CanvasGroup ui;
    public CanvasGroup background;
    public float uiTimer;
    public AnimationCurve uiCurve;


    public float restart;
    void Update()
    {
        if (open)
        {

        }
    }
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
            //cmcam.Play();
            door.Open();
            open = true;
            uiTimer = 10;
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
