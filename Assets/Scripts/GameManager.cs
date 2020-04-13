using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameType
    {
        LOCAL, CLIENT, SERVER
    }
    [HideInInspector]
    public bool debug;

    public GameType _gameType;
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<Controller>().networkObject.IsServer) _gameType = GameType.SERVER;
        else _gameType = GameType.CLIENT;
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
