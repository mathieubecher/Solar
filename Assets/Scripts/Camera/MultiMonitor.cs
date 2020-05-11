using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MultiMonitor : MonoBehaviour
{
    [SerializeField] private Camera main;
    [SerializeField] private Camera player2;
    private GameManager _manager;
    private bool multi = false;
    
    #if UNITY_STANDALONE_WIN
       
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
       
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string className, string windowName);
       
        public static IEnumerator SetWindowPosition(int x, int y) {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            SetWindowPos(FindWindow(null, Application.productName), 0, x, y, 0, 0, 5);
        }
       
    #endif

    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<GameManager>();
        if(_manager.gameType == GameManager.GameType.SOLO) Mono();
        else if(_manager.gameType == GameManager.GameType.LOCAL) Dual();
        else if (StaticClass.serverType == StaticClass.ServerType.PLAYER) OnlinePlayer();
        else if (StaticClass.serverType == StaticClass.ServerType.SUN) OnlineSun();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)  && (_manager.gameType == GameManager.GameType.SOLO || _manager.gameType == GameManager.GameType.LOCAL))
        {
            if(!multi)
            {
                Dual();
            }
            else
            {
                Mono();
            }
        }
        
    }

    public void Mono()
    {
        multi = false;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution (1920,1080,true);
        main.rect = new Rect(0,0,1,1);
        player2.rect = new Rect(0.7f,0.05f,0.25f,0.25f);
        #if UNITY_STANDALONE_WIN
        StartCoroutine(SetWindowPosition(0,0));
        #endif
    }

    public void Dual()
    {
        Debug.Log("dual");
        multi = true;
        
        /*
        // MULTI GPU 
        Display.displays[0].Activate(1920, 1080, 60);
        Display.displays[1].Activate(1920, 1080, 30);
         */
        
        
        Screen.fullScreenMode = FullScreenMode.Windowed;   
        Screen.SetResolution (3840,1080+100,false);
        main.rect = new Rect(0,0,0.5f,1);
        player2.rect = new Rect(0.5f,0,0.5f,1);
        #if UNITY_STANDALONE_WIN
        StartCoroutine(SetWindowPosition(-8, -50));
        #endif
    }

    public void OnlineSun()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution (1920,1080,true);
        player2.rect = new Rect(0,0,1,1);
        main.enabled = false;
        player2.enabled = true;
    }
    public void OnlinePlayer()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution (1920,1080,true);
        main.rect = new Rect(0,0,1,1);
        player2.enabled = false;
        main.enabled = true;
    }
}