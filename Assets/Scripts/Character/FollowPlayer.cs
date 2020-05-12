using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Controller c;
    // Start is called before the first frame update
    void Start()
    {
        c = FindObjectOfType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = c.Target;
    }
}
