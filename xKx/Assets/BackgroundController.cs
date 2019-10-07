using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{

    public Transform Tier1;
    public Transform Tier2;
    public Transform Tier3;

    public Vector3 CamPos;
    public Camera MainCam;
    public float RotSpeed;



// Start is called before the first frame update
    void Start()
    {
        CamPos = MainCam.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (CameraController.Instance.CamState == Player.State.Idle && MainCam.transform.localPosition != CamPos)
        {
            transform.position += Player.Controller.ForwardInput  * Player.Singleton.transform.forward * Player.Controller.Speed * Time.deltaTime;
            RotSpeed= Player.Controller.ForwardInput;
            Tier1.Rotate(Vector3.up, RotSpeed * 0.15f);
            Tier2.Rotate(Vector3.up, RotSpeed * 0.1f);
            Tier3.Rotate(Vector3.up, RotSpeed * 0.05f);
        }

        if (CameraController.Instance.CamState == Player.State.Combat)
        {
            RotSpeed = Mathf.Lerp(RotSpeed, 0, 0.05f);
            Tier1.Rotate(Vector3.up, RotSpeed * 0.15f);
            Tier2.Rotate(Vector3.up, RotSpeed * 0.1f);
            Tier3.Rotate(Vector3.up, RotSpeed * 0.05f);
        }


        CamPos = MainCam.transform.localPosition;

    }
}
