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

    }

    public void SetActive()
    {
    }
    
    

    public void LaunchParticle()
    {
        particle.Play();
    }

    public void Rise()
    {
            _animator.SetBool("active",true);
    }
    
    public void Walk()
    {
        _animator.SetBool("walk",true);
    }
    
    public void Dead()
    {
        _animator.SetBool("dead",true);
        LaunchParticle();
    }
    
}
