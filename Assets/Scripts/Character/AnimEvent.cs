using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class AnimEvent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject leftFoot;
    [SerializeField] private GameObject rightFoot;
    [SerializeField] private GameObject decalLeft;
    [SerializeField] private GameObject decalRight;
    [SerializeField] private GameObject footSteps;
    private int _mask;
    void Start()
    {
        _mask =~ LayerMask.GetMask("Character");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeftFootStep()
    {
        
        Vector3 position = leftFoot.transform.position;
        if (Physics.Raycast(origin: leftFoot.transform.position, direction: Vector3.down,
            hitInfo: out RaycastHit hit, maxDistance: 5, layerMask: _mask) && hit.collider.gameObject.layer == 11)
        {
            
            position = hit.point + Vector3.up*0.1f;
            GameObject left = Instantiate(decalLeft, position, leftFoot.transform.rotation,footSteps.transform);
            left.transform.Rotate(Vector3.right,180);
            left.transform.Rotate(Vector3.forward, 180);
            AkSoundEngine.PostEvent("Cha_Footsteps_Play", leftFoot);
        }
    
        
    }

    public void RightFootStep()
    {
        Vector3 position = rightFoot.transform.position;
        if (Physics.Raycast(origin: rightFoot.transform.position, direction: Vector3.down,
            hitInfo: out RaycastHit hit, maxDistance: 5, layerMask: _mask) && hit.collider.gameObject.layer == 11)
        {
            position = hit.point + Vector3.up*0.1f;
            GameObject right = Instantiate(decalRight, position, rightFoot.transform.rotation,footSteps.transform);
        
            right.transform.Rotate(Vector3.right, 180);
            right.transform.Rotate(Vector3.forward, 180);
            AkSoundEngine.PostEvent("Cha_Footsteps_Play", rightFoot);
        }
    }
}
