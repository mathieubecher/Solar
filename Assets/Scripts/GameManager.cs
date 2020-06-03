using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;
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

    public Options options;
    
    [Header("Vitesse en jeu")]
    [Range(0,1)]
    public float timeScale = 1;

    public Controller controller;

    // Start is called before the first frame update
    void Awake()
    {
        controller = FindObjectOfType<Controller>();
        gameType = StaticClass.gameType;
        Debug.Log(gameType);
        mask = LayerMask.GetMask("Sand")+LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
        #if UNITY_EDITOR
            //UnityEditor.EditorApplication.isPlaying = false;
        #else
            //Application.Quit();
        #endif

            options.gameObject.SetActive(!options.gameObject.active);

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            debug = !debug;
        }

        Time.timeScale = timeScale;
    }
}
