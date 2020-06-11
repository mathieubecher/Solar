using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : Menu
{

    void Awake()
    {
        foreach (OptionsMenu m in _menus)
        {
            m.Awake();
        } 
    }
    
    public List<OptionsMenu> _menus;
    public void GoTo(string name)
    {
        AkSoundEngine.PostEvent("UI_Clicked", gameObject);
        foreach (OptionsMenu m in _menus)
        {
            if(m.name == name) m.Enable();
            else m.Disable();
        }
        
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        GoTo("");
        foreach (OptionsMenu m in _menus)
        {
            m.Reset();
        }
        
    }
    
}

public class OptionsMenu : Menu
{
    private float timer = 0;
    private bool enable = false;
    private Vector3 originPos;
    private AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1);
    private CanvasGroup _canvas;
    public void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
        originPos = transform.position;
    }
    public void Enable()
    {
        gameObject.SetActive(true);
        enable = true;
        EstimateRender();
    }

    public void Disable()
    {
        enable = false;
    }

    void Update()
    {
        if (enable && timer < 1)
        {
            timer += Time.deltaTime * 3;
            EstimateRender();
        }
        else if (!enable && timer > 0)
        {
            timer -= Time.deltaTime * 3;
            EstimateRender();
            if (timer <= 0)
            {
                timer = 0;
                gameObject.SetActive(false);
            }
        }
        
    }

    private void EstimateRender()
    {
        _canvas.alpha = curve.Evaluate(timer);
        transform.position = Vector3.Lerp(new Vector3(originPos.x + 100, originPos.y, originPos.z), originPos,curve.Evaluate(timer));
    }

    public void Reset()
    {
        timer = 0;
        enable = false;
        EstimateRender();
        gameObject.SetActive(false);
    }
}