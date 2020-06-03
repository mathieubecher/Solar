using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Controller controller;

    public Player1Settings player1Settings;
    public Player2Settings player2Settings;
    [Header("Buttons")]
    public SoundList sounds;
    public GraphicsList graphics;
    public Player1List player1;
    public Player2List player2;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (Options options in FindObjectsOfType<Options>())
        {
            if (options != this)
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void OnDisable()
    {
#if UNITY_EDITOR
#else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }
    
    #region Sound
    public void OnChangeMainVolume(float value)
    {
        //TODO Charge RTPC
        sounds.MainVolume.value = value;
    }

    public void OnChangeMusic(float value)
    {
        //TODO Charge RTPC
        sounds.Music.value = value;
    }

    public void OnChangeSoundEffect(float value)
    {
        //TODO Charge RTPC
        sounds.SoundEffect.value = value;
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
    }

    public void ChangeContrast(float value)
    {
        graphics.Contrast.value = value;
    }

    public void SwitchFullscreen(bool value)
    {
        graphics.FullScreen.isOn = value;
    }

    public void SwitchScreenResolution(Int32 value)
    {
        graphics.ScreenResolution.value = value;
    }

    public void SwitchAntiAliasing(Int32 value)
    {
        graphics.AntiAliasing.value = value;
    }

    public void SwitchMotionBlur(bool value)
    {
        graphics.MotionBlur.isOn = value;
    }

    public void SwtichVolumetricLights(bool value)
    {
        graphics.VolumetricLight.isOn = value;
    }

    public void OnClickDefaultGraphics()
    {
        ChangeBrightness(0.5f);
        ChangeContrast(0.5f);
        SwitchFullscreen(true);
        SwitchAntiAliasing(2);
        SwitchMotionBlur(true);
        SwtichVolumetricLights(true);
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
        player1Settings.invertVertical = value;
        player1.InvertVerticalAxis.isOn = value;
    }

    public void OnChangeXAsisSensitivity(float value)
    {
        player1Settings.xAxisSensitivity = value;
        player1.XSensitivity.value = value;
    }

    public void OnChangeYAxisSensitivity(float value)
    {
        
        player1Settings.yAxisSensitivity = value;
        player1.YSensitivity.value = value;
    }

    public void Player1ActiveVibration(bool value)
    {
        
        player1Settings.vibration = value;
        player1.Vibration.isOn = value;
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
        public bool vibration;

        public Player2Settings(float _sunSensitivity = 1, float _platformSensitivity = 1, bool _invertSun = false, bool _vibration = true)
        {
            sun = Bind.L2R2;
            platform = Bind.L1R1;
            sunSensitivity = _sunSensitivity;
            platformSensitivity = _platformSensitivity;
            invertSun = _invertSun;
            vibration = _vibration;
        }
    }

    public enum Bind
    {
        L1R1 = 0,L2R2 = 1,LeftStick = 2,RightStick = 3
    }
    
    public void BindMoveSun(int value)
    {
        if(controller == null) controller = FindObjectOfType<Controller>();
        Bind last = player2Settings.sun;
        
        player2Settings.sun = (Bind)value;
        if(player2Settings.platform == (Bind)value) BindPlatform((int)last);
        
        controller.inputs.BindSun((Bind)value);
        player2.SunBinding.value = value;
        
    }

    
    public void BindPlatform(int value)
    {
        if(controller == null) controller = FindObjectOfType<Controller>();
        Bind last = player2Settings.platform;
        
        player2Settings.platform = (Bind)value;
        if(player2Settings.sun == (Bind)value) BindMoveSun((int)last);
        
        controller.inputs.BindPlatform((Bind)value);
        player2.PlatformBinding.value = value;
    }
    public void OnChangeSunSensitivity(float value)
    {
        player2Settings.sunSensitivity = value;
        
        if (FindObjectOfType<Controller>().inputs.GetType() == typeof(OnlineSun))
        {
            ((OnlineSun)FindObjectOfType<Controller>().inputs).UpdateSunSensitivity(value);
        }
        player2.SunSensitivity.value = value;
    }

    public void OnChangePlatformSensitivity(float value)
    {
        player2Settings.platformSensitivity = value;
        player2.PlatformSensitivity.value = value;
    }

    public void InvertSunRotation(bool value)
    {
        player2Settings.invertSun = value;
        player2.InvertSunRotation.isOn = value;
    }

    public void Player2ActiveVibration(bool value)
    {
        player2Settings.vibration = value;
        player2.Vibration.isOn = value;
    }

    public void OnClickDefaultPlayer2()
    {
        BindMoveSun((int)Bind.L2R2);
        BindPlatform((int)Bind.L1R1);
        OnChangeSunSensitivity(1);
        OnChangePlatformSensitivity(1);
        InvertSunRotation(false);
        Player2ActiveVibration(true);
    }
    
    #endregion
}
