using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MultiMonitor : MonoBehaviour
{
    [SerializeField] private Camera main;
    [SerializeField] private Camera player2;
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
        
        //Display.displays[0].Activate(1920, 1080, 60);
        //Display.displays[1].Activate(1920, 1080, 30);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(!multi){
                multi = true;
                Screen.fullScreenMode = FullScreenMode.Windowed;   
                Screen.SetResolution (3840,1080+100,false);
                main.rect = new Rect(0,0,0.5f,1);
                player2.rect = new Rect(0.5f,0,0.5f,1);
                #if UNITY_STANDALONE_WIN
                StartCoroutine(SetWindowPosition(-8, -50));
                #endif
            }
            else
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
        }
        
    }
}