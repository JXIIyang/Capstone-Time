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
        Instance = this;
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

        if (Input.GetKeyUp(KeyCode.P)) Player.Singleton.PlayerState = Player.State.Idle;
        if (Input.GetKeyUp(KeyCode.O)) Player.Singleton.PlayerState = Player.State.Combat;


        if (CamState != Player.Singleton.PlayerState)
        {
            switch (Player.Singleton.PlayerState)
            {
                case Player.State.Combat:
                    y = Mathf.Lerp(y, CamPresetPosition[1].y, 0.05f);
                    z = Mathf.Lerp(z, CamPresetPosition[1].z, 0.1f);
                    x = Mathf.Lerp(x, Player.Singleton.transform.position.x + CamPresetPosition[1].x, 0.05f);
                    transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, CamPresetRotation[1], 0.05f);
                    CamState = Player.State.Transit;
                    if (Vector3.Distance(transform.position, Player.Singleton.transform.position + CamPresetPosition[1]) < 1f &&
                        Vector3.Distance(transform.localEulerAngles, CamPresetRotation[1]) < 0.5f)
                    {
                        CamState = Player.State.Combat;
                    }
                    break;
                case Player.State.Idle:
                    y = Mathf.Lerp(y, CamPresetPosition[0].y, 0.1f);
                    z = Mathf.Lerp(z, CamPresetPosition[0].z, 0.2f);
                    x = Mathf.Lerp(x, Player.Singleton.transform.position.x + CamPresetPosition[0].x, 0.1f);
                    transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, CamPresetRotation[0], 0.15f);
                    var pos = new Vector3(Player.Singleton.transform.position.x + CamPresetPosition[0].x,CamPresetPosition[0].y, CamPresetPosition[0].z);
                    CamState = Player.State.Transit;
                    BackgroundController.Background.CamStop = false;
                    if (Vector3.Distance(transform.position, pos) < 0.1f &&
                        Vector3.Distance(transform.localEulerAngles, CamPresetRotation[0]) < 0.1f)
                    {
                        CamState = Player.State.Idle;
                        x = pos.x;
                        y = pos.y;
                        z = pos.z;
                        transform.localEulerAngles = Vector3.zero;
                    }
                    break;
            }
        }
        
        switch (CamState)
        {
            case Player.State.Combat:
                if (Mathf.Abs(z - Player.Singleton.transform.position.z) > 3f)
                {
                    z = Mathf.Lerp(z, Player.Singleton.transform.position.z, 0.05f);
                } 
                if (y - Player.Singleton.transform.position.y < 0.7f)
                {
                    y = Mathf.Lerp(y, Player.Singleton.transform.position.y + 0.7f, 0.05f);
                }
                if (Player.Singleton.transform.position.x - x > 2.4f || Player.Singleton.transform.position.x - x < 2.6f )
                {
                    x = Mathf.Lerp(x, Player.Singleton.transform.position.x - 2.5f, 0.05f);
                }

                var num = Player.Singleton.transform.position.x - x;
                Debug.Log("X Dis: " + num);
                float h = Input.GetAxis("Mouse X");
                float v = Input.GetAxis("Mouse Y");
                
                break;
            case Player.State.Idle:
                BackgroundController.Background.CamStop = true;
                if (x - Player.Singleton.transform.position.x < 0f)
                {
                    x = Mathf.Lerp(x, Player.Singleton.transform.position.x, 0.1f);
                    BackgroundController.Background.CamStop = false;
                } 
                if (x - Player.Singleton.transform.position.x > 3f)
                {
                    x = Mathf.Lerp(x, Player.Singleton.transform.position.x + 3f, 0.1f);
                    BackgroundController.Background.CamStop = false;
                }   
                break;
        }
        
        
        
        
        transform.position = new Vector3(x, y, z);

    }

}
