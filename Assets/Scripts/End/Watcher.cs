using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    public Anim global;
    public GameObject Goto;
    private Vector3 objectif;
    public bool active;
    [SerializeField] private ParticleSystem particle;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        objectif = Goto.transform.position;
        _animator = GetComponent<Animator>();
        //Debug.Log((objectif - transform.position).magnitude);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active && _animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            if ((objectif - transform.position).magnitude < 25) global.OpenDoor();
            if((objectif - transform.position).magnitude > 1)
                transform.position += new Vector3(objectif.x - transform.position.x,objectif.y - transform.position.y,objectif.z - transform.position.z).normalized * 5 * Time.deltaTime;
            /*
            else
            {
                _animator.SetBool("dead", true);
            }
            */
        }
        
    }

    public void SetActive()
    {
        active = true;
        _animator.SetBool("active", true);
    }
    
    

    public void LaunchParticle()
    {
        particle.Play();
    }

}
