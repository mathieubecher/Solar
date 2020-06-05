using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWrapper : MonoBehaviour
{
    public UIInterface uiInterface;

    //TODO destroy if already exist
    void Awake()
    {
        /*
        foreach (UIWrapper options in FindObjectsOfType<UIWrapper>())
        {
            if (options != this)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        */
    }

}
