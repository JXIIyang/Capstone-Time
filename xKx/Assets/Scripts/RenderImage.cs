using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderImage : MonoBehaviour
{
    public Material rTMat;
    //public RenderTexture RT;
    
    


    private void Awake()
    {
        var cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
    }
    
    
    [ExecuteInEditMode]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, rTMat);
        Debug.Log(destination);
    }
}
