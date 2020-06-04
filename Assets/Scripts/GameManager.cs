using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Awake()
    {
        UiInterface = FindObjectOfType<UIWrapper>().uiInterface; 
        UiInterface.gameObject.SetActive(false);
        controller = FindObjectOfType<Controller>();
        gameType = StaticClass.gameType;
        Debug.Log(gameType);
        mask = LayerMask.GetMask("Sand")+LayerMask.GetMask("Default");
        
    }

    void Start()
    {
        UiInterface.ApplyAllChange();    
    }
    
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton0)) && !UiInterface.gameObject.activeSelf)
        {
            UiInterface.gameObject.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            debug = !debug;
        }

        Time.timeScale = timeScale;
    }
}
