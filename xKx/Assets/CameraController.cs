using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditorInternal;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CameraController : MonoBehaviour
{
    public float Xoffset;
    public List<Vector3> CamPresetPosition = new List<Vector3>();
    public List<Vector3> CamPresetRotation = new List<Vector3>();
    
    //CamState
    public Player.State CamState;

    public static CameraController Instance;


    void Awake()
    {
        CameraController.Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Xoffset = transform.position.x - Player.Singleton.transform.position.x;
        CamState = Player.State.Idle;

    }

    // Update is called once per frame
    void Update()
    {
        var x = transform.position.x;
        var y= transform.position.y;
        var z = transform.position.z;



        if (CamState != Player.Singleton.PlayerState)
        {
            switch (Player.Singleton.PlayerState)
            {
                case Player.State.Combat:
                    y = Mathf.Lerp(y, CamPresetPosition[1].y, 0.05f);
                    z = Mathf.Lerp(z, CamPresetPosition[1].z, 0.05f);
                    x = Mathf.Lerp(x, Player.Singleton.transform.position.x + CamPresetPosition[1].x, 0.05f);
                    transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, CamPresetRotation[1], 0.05f);
                    //Debug.Log("Dis: " + Vector3.Distance(transform.position, Player.Singleton.transform.position + CamPresetPosition[1]));
                    //Debug.Log("Rot: " + Vector3.Distance(transform.localEulerAngles, CamPresetRotation[1]));
                    if (Vector3.Distance(transform.position, Player.Singleton.transform.position + CamPresetPosition[1]) < 4.3f &&
                        Vector3.Distance(transform.localEulerAngles, CamPresetRotation[1]) < 0.5f)
                    {
                        CamState = Player.State.Combat;
                    }
                    break;
                case Player.State.Idle:
                    CamState = Player.State.Idle;
                    break;
            }
        }
        
        switch (CamState)
        {
            case Player.State.Combat:
                if (Mathf.Abs(z - Player.Singleton.transform.position.z) > 3f)
                {
                    z = Mathf.Lerp(z, Player.Singleton.transform.position.z, 0.1f);
                } 
                if (y - Player.Singleton.transform.position.y < 0.7f)
                {
                    y = Mathf.Lerp(y, Player.Singleton.transform.position.y + 0.7f, 0.1f);
                }
                if (Player.Singleton.transform.position.x - x > 2.4f || Player.Singleton.transform.position.x - x < 2.6f )
                {
                    x = Mathf.Lerp(x, Player.Singleton.transform.position.x - 2.5f, 0.1f);
                }

                var num = Player.Singleton.transform.position.x - x;
                Debug.Log("X Dis: " + num);
                float h = Input.GetAxis("Mouse X");
                float v = Input.GetAxis("Mouse Y");
                
                break;
            case Player.State.Idle:
                if (x - Player.Singleton.transform.position.x < -0.5f)
                {
                    x = Mathf.Lerp(x, Player.Singleton.transform.position.x - 0.5f, 0.1f);
                } 
                if (x - Player.Singleton.transform.position.x > 3.5f)
                {
                    x = Mathf.Lerp(x, Player.Singleton.transform.position.x + 3.5f, 0.1f);
                }
                break;
        }
        
        
        
        
        transform.position = new Vector3(x, y, z);

    }

}
