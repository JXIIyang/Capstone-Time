using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CubicCameraController : MonoBehaviour
{


    public enum CubicCamState
    {
        Back,
        Top,
        Side
    }

    public CubicCamState camState = CubicCamState.Side;

    public Transform playerTrans;

    public float relativeX;
    public float relativeY;
    public float relativeZ;
    
    // Start is called before the first frame update
    void Start()
    {
        relativeX = transform.position.x - playerTrans.position.x;
        relativeY = transform.position.y - playerTrans.position.y;
        relativeZ = transform.position.z - playerTrans.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) camState = CubicCamState.Side;
        if (Input.GetKeyDown(KeyCode.Alpha2)) camState = CubicCamState.Top;
        if (Input.GetKeyDown(KeyCode.Alpha3)) camState = CubicCamState.Back;

        SetCameraPos(camState);

    }

    public void SetCameraPos(CubicCamState c)
    {
        var rx = transform.localEulerAngles.x;
        var ry = transform.localEulerAngles.y;
        var rz = transform.localEulerAngles.z;

        //var rate = Mathf.Clamp(Time.deltaTime * 10, 0.3f,1.2f);
        var rate = 0.2f;
        
        switch (c)
        {
            case CubicCamState.Side:
                relativeX = Mathf.SmoothStep(relativeX, 0, rate);
                relativeY = Mathf.SmoothStep(relativeY, 0, rate);
                relativeZ = Mathf.SmoothStep(relativeZ, -4, rate);
                rx = Mathf.SmoothStep(rx, 0, rate);
                ry = Mathf.SmoothStep(ry, 0, rate);
                rz = Mathf.SmoothStep(rz, 0, rate);
                
                break;
            case CubicCamState.Top:
                relativeX = Mathf.SmoothStep(relativeX, -1.5f,  rate);
                relativeY = Mathf.SmoothStep(relativeY, 2, rate);
                relativeZ = Mathf.SmoothStep(relativeZ, 0,  rate);
                rx = Mathf.SmoothStep(rx, 40, rate);
                ry = Mathf.SmoothStep(ry, 90, rate);
                rz = Mathf.SmoothStep(rz, 0, rate);
                break;
            case CubicCamState.Back:
                relativeX = Mathf.SmoothStep(relativeX, -3, rate);
                relativeY = Mathf.SmoothStep(relativeY, 0f, rate);
                relativeZ = Mathf.SmoothStep(relativeZ, 0, rate);
                rx = Mathf.SmoothStep(rx, 0, rate);
                ry = Mathf.SmoothStep(ry, 90, rate);
                rz = Mathf.SmoothStep(rz, 0, rate);
                break;
       
        }
        
        transform.position = new Vector3(playerTrans.position.x + relativeX, 0.4f * playerTrans.position.y + relativeY, playerTrans.position.z + relativeZ);
        transform.localEulerAngles = new Vector3(rx, ry, rz);
        
    }
}
