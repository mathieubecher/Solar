using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Transform target;
    private Vector3 originalPos;
    
    [Header("Anim infos")]
    [SerializeField] private float Timer;
    [SerializeField] private AnimationCurve curve;

    private float timer;
    private bool active;
    // Start is called before the first frame update
    void Awake()
    {
        timer = 0;
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && timer <= Timer)
        {
            timer += Time.deltaTime;
            door.transform.position = Vector3.Lerp(originalPos, target.position, timer/Timer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        active = true;
    }
}
