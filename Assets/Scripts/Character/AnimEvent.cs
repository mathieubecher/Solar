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
    [SerializeField] private ParticleSystem particle;

    private bool isBurning;
    private float burnTimer;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isBurning && burnTimer < 1.1f)
        {
            burnTimer += Time.deltaTime/3f;
            Shader.SetGlobalFloat("timerburn", burnTimer);
        }
    }
    
    /// <summary>
    /// Fonction appelée quand le personnage pose le pied gauche sur le sol
    /// </summary>
    public void LeftFootStep()
    {
        
        Vector3 position = leftFoot.transform.position;
        if (Physics.Raycast(origin: leftFoot.transform.position, direction: Vector3.down,
                hitInfo: out RaycastHit hit, maxDistance: 5, layerMask: GameManager.mask) && hit.collider.gameObject.layer == 11)
        {
            AkSoundEngine.SetSwitch("FootStep_Floor","Sand",leftFoot);
            position = hit.point + Vector3.up * 0.1f;
            GameObject left = Instantiate(decalLeft, position, leftFoot.transform.rotation, footSteps.transform);
            left.transform.Rotate(Vector3.right, 90);
            //left.transform.Rotate(Vector3.forward, 180);
        }
        else AkSoundEngine.SetSwitch("FootStep_Floor","Stone",leftFoot);

        AkSoundEngine.PostEvent("Cha_Footsteps_Play", leftFoot);
    }

    /// <summary>
    /// Fonction appelée quand le personnage pose le pied droit sur le sol
    /// </summary>
    public void RightFootStep()
    {
        Vector3 position = rightFoot.transform.position;
        if (Physics.Raycast(origin: rightFoot.transform.position, direction: Vector3.down,
            hitInfo: out RaycastHit hit, maxDistance: 5, layerMask: GameManager.mask) && hit.collider.gameObject.layer == 11)
        {
            AkSoundEngine.SetSwitch("FootStep_Floor","Sand",rightFoot);
            position = hit.point + Vector3.up*0.1f;
            GameObject right = Instantiate(decalRight, position, rightFoot.transform.rotation,footSteps.transform);
            right.transform.Rotate(Vector3.right, 90);
            //right.transform.Rotate(Vector3.forward, 180);
        }
        else AkSoundEngine.SetSwitch("FootStep_Floor","Stone",rightFoot);
        
        AkSoundEngine.PostEvent("Cha_Footsteps_Play", rightFoot);
    }

    public void BeginBurn()
    {
        isBurning = true;
        //Debug.Log("burn");
        particle.Play();
    }

    public void ResetBurn()
    {
        isBurning = false;
        burnTimer = 0;
        Shader.SetGlobalFloat("timerburn", burnTimer);
    }

    public void Inspiration()
    {
        AkSoundEngine.PostEvent("Cha_Breath_Inspi", gameObject);
    }

    public void Expiration()
    {
        AkSoundEngine.PostEvent("Cha_Breath_Expi", gameObject);
    }
}
