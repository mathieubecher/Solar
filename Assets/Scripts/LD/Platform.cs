using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

/// <summary>
/// Plateforme à trajectoire circulaire
/// </summary>
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
        
        // Récupération de la liste des plateformes
        childs = new List<Transform>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            childs.Add(transform.GetChild(i));
        }
        progress = 0;
    }

    /// <summary>
    /// Met à jour la vélocité
    /// </summary>
    /// <param name="velocity"></param>
    public virtual void SetProgress(float velocity)
    {
        _velocity = velocity;
    }
    /// <summary>
    /// Récipère la vélocité
    /// </summary>
    /// <returns></returns>
    protected float GetLocalProgress()
    {
        return progress + begin_progress;
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        InputVelocity();
        transform.rotation = Quaternion.Euler(new Vector3(0,GetLocalProgress() * 360,0));
    }

    /// <summary>
    /// Récupere la valeur de progress en fonction du type de la partie.
    /// </summary>
    protected void InputVelocity()
    {
        float lastProgress = progress;
        // S'il s'agit d'un serveur
        if ((StaticClass.gameType == GameManager.GameType.SERVER ||
             StaticClass.gameType == GameManager.GameType.CLIENT))
        {
            // et que l'instance du joueur a le control du soleil
            if((StaticClass.serverType == StaticClass.ServerType.SUN &&
                StaticClass.gameType == GameManager.GameType.SERVER) ||
               (StaticClass.serverType == StaticClass.ServerType.PLAYER &&
                StaticClass.gameType == GameManager.GameType.CLIENT))
            {
                progress += _velocity * speed * Time.deltaTime;
                _server.CallSetProgress(progress);
            }
            // et que l'instance du joueur n'a pas le control du soleil
            else
            {
                progress = _server.GetProgress();
            }
        }
        // Sinon
        else
        {
            progress += _velocity * speed * Time.deltaTime;
        }
        Debug.Log(Mathf.Abs(progress - lastProgress)/(Time.deltaTime * speed));
        AkSoundEngine.SetRTPCValue("RTPC_Plateform_Velocity", Mathf.Abs(progress - lastProgress)/(Time.deltaTime * speed) *100);
    }
    
#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        // Met à jour la position de la plateforme avec progress
        if (!Application.isPlaying) progress += Time.deltaTime * speed;
        
        // Dessin de la trajectoire
        childs = new List<Transform>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            childs.Add(transform.GetChild(i));
        }
        
        // Centre du cercle
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,1);
        
        // Dessin de la position de chaque plateforme
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
