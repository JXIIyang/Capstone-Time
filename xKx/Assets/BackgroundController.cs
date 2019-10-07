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

    public enum CamList
    {
        follow,
        behind
    }
    public CamList CurrentCam;



// Start is called before the first frame update
    void Start()
    {
        CamPos = MainCam.transform.position;
        CurrentCam = CamList.follow;

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentCam == CamList.follow && MainCam.transform.localPosition != CamPos)
        {
            transform.position += Player.Controller.ForwardInput  * Player.Singleton.transform.forward * Player.Controller.Speed * Time.deltaTime;
            Tier1.Rotate(Vector3.up, Player.Controller.ForwardInput * 0.15f);
            Tier2.Rotate(Vector3.up, Player.Controller.ForwardInput * 0.1f);
            Tier3.Rotate(Vector3.up, Player.Controller.ForwardInput * 0.05f);
        } 
        
        if (CurrentCam == CamList.behind)


        CamPos = MainCam.transform.localPosition;

    }
}
