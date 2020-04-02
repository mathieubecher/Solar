using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject leftFoot;
    [SerializeField] private GameObject rightFoot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeftFootStep()
    {
        AkSoundEngine.PostEvent("Cha_Footsteps_Play", leftFoot);
    }

    public void RightFootStep()
    {
        AkSoundEngine.PostEvent("Cha_Footsteps_Play", rightFoot);
    }
}
