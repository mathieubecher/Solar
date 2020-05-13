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

    public float platformProgress;

    // Start is called before the first frame update
    void Awake()
    {
        gameType = StaticClass.gameType;
        Debug.Log(gameType);
        mask = LayerMask.GetMask("Sand")+LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        platformProgress += Time.deltaTime;
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

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) platformProgress += Time.deltaTime;
    }
}
