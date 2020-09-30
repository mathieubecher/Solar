using System;
using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int mask;
    public enum GameType
    {
        SOLO, LOCAL, CLIENT, SERVER
    }
    [HideInInspector]
    public bool debug;

    public GameType gameType = GameType.SOLO;
    
    
    public UIInterface UiInterface;
    
    public Volume sky;
    public Volume postProcess;
    
    [Header("Vitesse en jeu")]
    [Range(0,1)]
    public float timeScale = 1;

    public Controller controller;

    public GameObject wrapperPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        UIWrapper wrapper = FindObjectOfType<UIWrapper>();
        if (wrapper == null)
        {
            GameObject o = Instantiate(wrapperPrefab);
            wrapper = o.GetComponent<UIWrapper>();
        }
        UiInterface = wrapper.uiInterface;
        UiInterface.gameObject.SetActive(false);
        controller = FindObjectOfType<Controller>();
        gameType = StaticClass.gameType;
        //Debug.Log(gameType);
        mask = LayerMask.GetMask("Sand")+LayerMask.GetMask("Default");
        
    }

    void Start()
    {
        controller.UiInterface = UiInterface;
        UiInterface.ApplyAllChange();    
    }
    
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) /*|| Input.GetKeyDown(KeyCode.JoystickButton0)*/) && !UiInterface.gameObject.activeSelf)
        {
            UiInterface.gameObject.SetActive(true);

        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
        {
            debug = !debug;
        }

        
        if (Input.GetKeyDown(KeyCode.G)) controller.activeDead = !controller.activeDead;
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
        {
            if (!speed)
            {
                speed = true;
                controller.speed = 20;
            }
            else
            {
                speed = false;
                controller.speed = 5;
            }
        }
        
        Time.timeScale = timeScale;
    }

    private bool speed;
    public void StartMusic()
    {
        AkSoundEngine.PostEvent("Play_Intro",gameObject);
    }

    public void Restart()
    {
        Destroy(FindObjectOfType<UIWrapper>().gameObject);
        try
        {
            NetWorker.EndSession();
            NetworkManager.Instance.Disconnect();
        }
        catch (Exception e)
        {
            // ignored
        }
        AkBankManager.UnloadBank("ALL");

        //Destroy(FindObjectOfType<AkBank>().gameObject);
        SceneManager.LoadScene(0);
    }
}
