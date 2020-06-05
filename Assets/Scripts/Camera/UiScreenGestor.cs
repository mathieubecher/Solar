using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class UiScreenGestor : MonoBehaviour
{
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
    
    public void SetFullScreen(bool value)
    {
        if(value) Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else SetResolution(res, true);
        
    }

    private Int32 res;
    public void SetResolution(Int32 value, bool change = false)
    {
        res = value;
        if ((Screen.fullScreenMode != FullScreenMode.FullScreenWindow && Screen.fullScreenMode != FullScreenMode.MaximizedWindow) || change)
        {
            switch (res)
            {
                case 0 : 
                    Screen.SetResolution (1366,768,false);
                    break;
                case 1 : 
                    Screen.SetResolution (1600,900,false);
                    break;
                case 2 : 
                    Screen.SetResolution (1920,1080,false);
                    break;
                default: 
                    Screen.SetResolution (2560,1440,false);
                    break;
            
            }

            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
}