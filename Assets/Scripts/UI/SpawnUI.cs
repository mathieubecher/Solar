using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUI : MonoBehaviour
{
    public GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        GameObject UIInstance = Instantiate(UI);
        DontDestroyOnLoad(UIInstance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
