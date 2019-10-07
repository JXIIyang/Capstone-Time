using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Xoffset;
    public List<Vector3> CamPresetPosition = new List<Vector3>();
    public List<Vector3> CamPresetRotation = new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
        Xoffset = transform.position.x - Player.Singleton.transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        var x = transform.position.x;
        var y= transform.position.y;
        var z = transform.position.z;

        if (x - Player.Singleton.transform.position.x < -0.5f)
        {
            x = Mathf.Lerp(x, Player.Singleton.transform.position.x - 0.5f, 0.1f);
        } 
        if (x - Player.Singleton.transform.position.x > 3.5f)
        {
            x = Mathf.Lerp(x, Player.Singleton.transform.position.x + 3.5f, 0.1f);
        }

        transform.position = new Vector3(x, y, z);

    }
}
