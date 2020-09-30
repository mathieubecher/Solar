using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UITuto : MonoBehaviour
{
    public bool active;
    public float timer;
    public bool first = true;
    public Animator tuto;

    public void Update()
    {
        if (active)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                tuto.gameObject.SetActive(false);
                active = false;
            }
        }
    }
}
