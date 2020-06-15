using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using UnityEngine.VFX.Utility;

public class UIInterface : MonoBehaviour
{
    public Controller controller;

    public Player1Settings player1Settings;
    public Player2Settings player2Settings;
    [Header("Buttons")]
    public SoundList sounds;
    public GraphicsList graphics;
    public Player1List player1;
    public Player2List player2;
    public bool inGame = false;

    
    
    // Start is called before the first frame update
    void Start()
    {
        SwitchFullscreen(graphics.FullScreen.isOn);
        SwitchScreenResolution(graphics.ScreenResolution.value);
        
       
    }

    public void ApplyAllChange()
    {
        inGame = true;
        
        OnChangeMainVolume(sounds.MainVolume.value);
        OnChangeMusic(sounds.Music.value);
        OnChangeSoundEffect(sounds.SoundEffect.value);
        
        ChangeBrightness(graphics.Brightness.value);
        ChangeContrast(graphics.Contrast.value);
        SwitchFullscreen(graphics.FullScreen.isOn);
        SwitchScreenResolution(graphics.ScreenResolution.value);
        SwitchVSync(graphics.VSync.isOn);
        SwitchQualitySettings(graphics.Quality.value);

        InvertVerticalAxis(player1.InvertVerticalAxis.isOn);
        OnChangeXAsisSensitivity(player1.XSensitivity.value);
        OnChangeYAxisSensitivity(player1.YSensitivity.value);
        Player1ActiveVibration(player1.Vibration.isOn);

        player2Settings.sun = Bind.L2R2;
        BindMoveSun(player2.SunBinding.value);
        player2Settings.platform = Bind.L1R1;
        BindPlatform(player2.PlatformBinding.value);
        OnChangeSunSensitivity(player2.SunSensitivity.value);
        OnActiveOutline(player2.Outline.isOn);
        
        controller = FindObjectOfType<Controller>();
    }
    
    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AkSoundEngine.SetState("Menu_State", "inMenu");   //While inGame
    }
    void OnDisable()
    {
#if UNITY_EDITOR
#else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
        GoToInterfance("Pause");
        AkSoundEngine.SetState("Menu_State", "inGame");   //While inGame
    }
    
    #region Sound
    public void OnChangeMainVolume(float value)
    {
        sounds.MainVolume.value = value;
        if (!inGame) return;
        AkSoundEngine.SetRTPCValue("RTPC_Main_Volume", value);
    }

    public void OnChangeMusic(float value)
    {
        sounds.Music.value = value;
        if (!inGame) return;
        AkSoundEngine.SetRTPCValue("RTPC_Music_Volume", value);
    }

    public void OnChangeSoundEffect(float value)
    {
        sounds.SoundEffect.value = value;
        if (!inGame) return;
        AkSoundEngine.SetRTPCValue("RTPC_SFX_Volume", value);
    }

    public void OnClickDefaultSound()
    {
        OnChangeMainVolume(1);
        OnChangeMusic(1);
        OnChangeSoundEffect(1);
    }
    #endregion
    
    #region Graphics
    public void ChangeBrightness(float value)
    {
        graphics.Brightness.value = value;
        if (!inGame) return;
        VolumeProfile p = FindObjectOfType<GameManager>().postProcess.profile;
        if (p.TryGet(out ColorAdjustments color))
        {
            color.postExposure.value = value * 2;
        }
    }
    public void ChangeContrast(float value)
    {
        graphics.Contrast.value = value;
        if (!inGame) return;
        VolumeProfile p = FindObjectOfType<GameManager>().postProcess.profile;
        if (p.TryGet(out ColorAdjustments color))
        {
            color.contrast.value = value*50 + 9;
        }
    }
    public void SwitchFullscreen(bool value)
    {
        graphics.FullScreen.isOn = value;
        #if !UNITY_EDITOR
        if (!inGame){
            FindObjectOfType<UiScreenGestor>().SetFullScreen(value);
        }
        else{
            FindObjectOfType<MultiMonitor>().SetFullScreen(value);
        }
        #endif
    }
    public void SwitchScreenResolution(Int32 value)
    {
        graphics.ScreenResolution.value = value;
        
        #if !UNITY_EDITOR
        if (!inGame){
            FindObjectOfType<UiScreenGestor>().SetResolution(value);
        }
        else
        {
            FindObjectOfType<MultiMonitor>().SetResolution(value);
        }
        #endif
    }


    public void SwitchQualitySettings(Int32 value)
    {
        graphics.Quality.value = value;
        if (!inGame) return;
        switch (value)
        {
            case 0 :
                SwitchAntiAliasing(0);
                SwitchMotionBlur(false);
                SwitchVolumetricLights(false);
                SwitchAnisotropic(false);
                SwitchTextureRes(2);
                break;
            case 1 :
                SwitchMotionBlur(true);
                SwitchVolumetricLights(true);
                SwitchAnisotropic(false);
                SwitchTextureRes(1);
                break;
            default :
                SwitchMotionBlur(true);
                SwitchVolumetricLights(true);
                SwitchAnisotropic(true);
                SwitchTextureRes(0);
                break;
        }
    }
    
    // TODO
    public void SwitchAntiAliasing(Int32 value)
    {
        graphics.AntiAliasing.value = value;
        if (!inGame) return;
        foreach (HDAdditionalCameraData cameraData in FindObjectsOfType<HDAdditionalCameraData>())
        {
            cameraData.antialiasing = (HDAdditionalCameraData.AntialiasingMode) value;
        }
    }
    
    public void SwitchMotionBlur(bool value)
    {
        graphics.MotionBlur.isOn = value;
        if (!inGame) return;
        VolumeProfile p = FindObjectOfType<GameManager>().postProcess.profile;
        if (p.TryGet(out MotionBlur blur))
        {
            blur.active = value;
        }
    }

    public void SwitchVolumetricLights(bool value)
    {
        graphics.VolumetricLight.isOn = value;
        if (!inGame) return;
        VolumeProfile p = FindObjectOfType<GameManager>().sky.profile;
        if (p.TryGet(out Fog fog))
        {
            fog.active = value;
        }
    }

    public void SwitchAnisotropic(bool value)
    {
        if (!inGame) return;
        //QualitySettings.anisotropicFiltering = (value)? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable;
        //graphics.Anisotropic.isOn = value;
    }

    public void SwitchTextureRes(Int32 value)
    {
        graphics.TextureRes.value = value;
        if (!inGame) return;
        QualitySettings.masterTextureLimit = value;
    }
    
    public void SwitchVSync(bool value)
    {
        graphics.VSync.isOn = value;
        if (!inGame) return;
        QualitySettings.vSyncCount = (value) ? 1 : 0;
    }

    public void OnClickDefaultGraphics()
    {
        ChangeBrightness(0);
        ChangeContrast(0.5f);
        SwitchFullscreen(true);
        SwitchVSync(true);
        SwitchQualitySettings(graphics.Quality.value);
    }


    
    
    
    #endregion
    
    #region Player1
    
    [Serializable] public struct Player1Settings
    {
        public float xAxisSensitivity;
        public float yAxisSensitivity;
        public bool invertVertical;
        public bool vibration;

        public Player1Settings(float _xAxisSensitivity = 1, float _yAxisSensitivity = 1, bool _invertVertical = false, bool _vibration = true)
        {
            xAxisSensitivity = _xAxisSensitivity;
            yAxisSensitivity = _yAxisSensitivity;
            invertVertical = _invertVertical;
            vibration = _vibration;
        }
    }
    public void InvertVerticalAxis(bool value)
    {
        player1.InvertVerticalAxis.isOn = value;
        if (!inGame) return;
        player1Settings.invertVertical = value;
    }

    public void OnChangeXAsisSensitivity(float value)
    {
        player1.XSensitivity.value = value;
        if (!inGame) return;
        player1Settings.xAxisSensitivity = value;
    }

    public void OnChangeYAxisSensitivity(float value)
    {
        
        player1.YSensitivity.value = value;
        if (!inGame) return;
        player1Settings.yAxisSensitivity = value;
    }

    public void Player1ActiveVibration(bool value)
    {
        
        player1.Vibration.isOn = value;
        if (!inGame) return;
        player1Settings.vibration = value;
    }

    public void OnClickDefaultPlayer1()
    {
        InvertVerticalAxis(false);
        OnChangeXAsisSensitivity(1);
        OnChangeYAxisSensitivity(1);
        Player1ActiveVibration(true);
    }
    #endregion
    
    #region Player2
    [Serializable]  public struct Player2Settings
    {
        public Bind sun;
        public Bind platform;
        
        public float sunSensitivity;
        public float platformSensitivity;
        public bool invertSun;

        public Player2Settings(float _sunSensitivity = 1, float _platformSensitivity = 1, bool _invertSun = false)
        {
            sun = Bind.L2R2;
            platform = Bind.L1R1;
            sunSensitivity = _sunSensitivity;
            platformSensitivity = _platformSensitivity;
            invertSun = _invertSun;
        }
    }

    public enum Bind
    {
        L1R1 = 0,L2R2 = 1,LeftStick = 2,RightStick = 3
    }
    
    public void BindMoveSun(int value)
    {
        player2.SunBinding.value = value;
        Bind last= player2Settings.sun;
        player2Settings.sun = (Bind)value;
        if(player2Settings.platform == (Bind)value) BindPlatform((int)last);
        
        if (!inGame) return;
        
        if(controller == null) controller = FindObjectOfType<Controller>();
        controller.inputs.BindSun((Bind)value);
        
    }
    
    public void BindPlatform(int value)
    {
        player2.PlatformBinding.value = value;
        Bind last = player2Settings.platform;
        player2Settings.platform = (Bind)value;
        if(player2Settings.sun == (Bind)value) BindMoveSun((int)last);
        
        if (!inGame) return;
        
        if(controller == null) controller = FindObjectOfType<Controller>();
        controller.inputs.BindPlatform((Bind)value);
    }
    
    public void OnChangeSunSensitivity(float value)
    {
        player2.SunSensitivity.value = value;
        if (!inGame) return;
        player2Settings.sunSensitivity = value;
        
        if (FindObjectOfType<Controller>().inputs.GetType() == typeof(OnlineSun))
        {
            ((OnlineSun)FindObjectOfType<Controller>().inputs).UpdateSunSensitivity(value);
        }
    }

    public void OnActiveOutline(bool value)
    {
        player2.Outline.isOn = value;
        controller.outline.SetActive(value);
    }

    public void OnChangePlatformSensitivity(float value)
    {
        player2.PlatformSensitivity.value = value;
        if (!inGame) return;
        player2Settings.platformSensitivity = value;
    }

    public void InvertSunRotation(bool value)
    {
        player2.InvertSunRotation.isOn = value;
        if (!inGame) return;
        player2Settings.invertSun = value;
    }



    public void OnClickDefaultPlayer2()
    {
        BindMoveSun((int)Bind.L2R2);
        BindPlatform((int)Bind.L1R1);
        OnChangeSunSensitivity(1);
        OnChangePlatformSensitivity(1);
        InvertSunRotation(false);
        OnActiveOutline(true);
    }
    
    #endregion

    
    #region Navigation
    public List<Menu> _menus;
    
    public void GoTo(string name)
    {
        AkSoundEngine.PostEvent("UI_Clicked", gameObject);
        GoToInterfance(name);
        
    }
    #endregion

    public void GoToInterfance(string name)
    {
        if (name == "Exit")
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else if (name == "Resume")
        {
            
            gameObject.SetActive(false);
        }
        else
        {
            if (name == "StartMenu" && inGame) name = "Pause";
            foreach (Menu m in _menus)
            {
                if(m.name == name) m.gameObject.SetActive(true);
                else m.gameObject.SetActive(false);
            }
        }
    }
}


public class Menu : MonoBehaviour
{
    public string name;
    public string exit;
    
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button9)) FindObjectOfType<UIInterface>().GoToInterfance(exit);
    }

    public void Bip()
    {
        AkSoundEngine.PostEvent("UI_Clicked", gameObject);
    }
}