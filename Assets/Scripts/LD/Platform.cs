using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected List<Transform> childs;
    public float begin_progress;
    protected float progress;
    protected float _velocity;
    [SerializeField] public float speed = 0.2f;
    protected PlatformServer _server;
    
    protected virtual void Awake()
    {
        _server = GetComponent<PlatformServer>();
        progress = 0;
        childs = new List<Transform>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            childs.Add(transform.GetChild(i));
        }
    }

    public virtual void SetProgress(float velocity)
    {
        _velocity = velocity;
    }
    protected float GetLocalProgress()
    {
        
        return progress + begin_progress;
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        if ((StaticClass.gameType == GameManager.GameType.SERVER ||
             StaticClass.gameType == GameManager.GameType.CLIENT))
        {
            if((StaticClass.serverType == StaticClass.ServerType.SUN &&
                StaticClass.gameType == GameManager.GameType.SERVER) ||
               (StaticClass.serverType == StaticClass.ServerType.PLAYER &&
               StaticClass.gameType == GameManager.GameType.CLIENT))
            {
                progress += _velocity * speed * Time.deltaTime;
                _server.CallSetProgress(progress);
            }
            else
            {
                progress = _server.GetProgress();
            }
        }
        else
        {
            progress += _velocity * speed * Time.deltaTime;
        }
        transform.rotation = Quaternion.Euler(new Vector3(0,GetLocalProgress() * 360,0));
    }
#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying) progress += Time.deltaTime * speed;
        childs = new List<Transform>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            childs.Add(transform.GetChild(i));
        }
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,1);
        
        foreach (Transform child in childs)
        {
            UnityEditor.Handles.color = Color.white;
            Vector3 distance = (child.localPosition);
            distance.y = 0;
            UnityEditor.Handles.DrawWireDisc(transform.position,transform.rotation * Vector3.up, distance.magnitude);
            Gizmos.color = Color.black;
            if (!Application.isPlaying) Gizmos.DrawSphere(transform.position + transform.rotation * new Vector3(Mathf.Sin(GetLocalProgress() * 2 * Mathf.PI),0,Mathf.Cos(GetLocalProgress() * 2 * Mathf.PI)) * distance.magnitude ,1);
        }
        
    }
#endif
    public void ResetProgress()
    {
        progress = 0;
    }
}
