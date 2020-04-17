using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameType
    {
        SOLO, LOCAL, CLIENT, SERVER
    }
    [HideInInspector]
    public bool debug;

    public GameType gameType = GameType.SOLO;
    // Start is called before the first frame update
    void Awake()
    {
        gameType = StaticClass.gameType;
        Debug.Log(gameType);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            debug = !debug;
        }

        if (Input.GetKeyDown(KeyCode.R) && (gameType == GameType.SOLO || gameType == GameType.LOCAL))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
