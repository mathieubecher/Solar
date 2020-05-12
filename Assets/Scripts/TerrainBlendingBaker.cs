using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBlendingBaker : MonoBehaviour
{
    //Shader that renders object based on distance to camera
    public Shader depthShader;
    //The render texture which will store the depth of our terrain
    public RenderTexture depthTexture;
    //The camera this script is attached to
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateBakingCamera()
    {
        //if the camera hasn't been assigned then assign it
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
        //the total width of the bounding box of our cameras view
        Shader.SetGlobalFloat("TB_SCALE", GetComponent<Camera>().orthographicSize * 2);
        //find the bottom corner of the texture in world scale by subtracting the size of the camera from its x and z position
        Shader.SetGlobalFloat("TB_OFFSET_X", cam.transform.position.x - cam.orthographicSize);
        Shader.SetGlobalFloat("TB_OFFSET_Z", cam.transform.position.z - cam.orthographicSize);
        //we'll also need the relative y position of the camera, lets get this by subtracting the far clip plane from the camera y position
        Shader.SetGlobalFloat("TB_OFFSET_Y", cam.transform.position.y - cam.farClipPlane);
        //we'll also need the far clip plane itself to know the range of y values in the depth texture
        Shader.SetGlobalFloat("TB_FARCLIP", cam.farClipPlane);
 
        //NOTE: some of the arithmatic here could be moved to the shader but keeping it here makes the shader cleaner so ¯\_(ツ)_/¯
    }
    // The context menu tag allows us to run methods from the inspector (https://docs.unity3d.com/ScriptReference/ContextMenu.html)
    [ContextMenu("Bake Depth Texture")]
    public void BakeTerrainDepth()
    {
        //call our update camera method 
        UpdateBakingCamera();
 
        //Make sure the shader and texture are assigned in the inspector
        if (depthShader != null && depthTexture != null)
        {
            //Set the camera replacment shader to the depth shader that we will assign in the inspector 
            cam.SetReplacementShader(depthShader, "RenderType");
            //set the target render texture of the camera to the depth texture 
            cam.targetTexture = depthTexture;
            //set the render texture we just created as a global shader texture variable
            Shader.SetGlobalTexture("TB_DEPTH", depthTexture);
        }
        else
        {
            Debug.Log("You need to assign the depth shader and depth texture in the inspector");
        }
    }
}
