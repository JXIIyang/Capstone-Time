using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public CubicPlayerController playerController;

    public Transform playerTransform;

    public Transform shadowModel;
    public Transform playerModel;
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, -playerTransform.position.y, playerTransform.position.z);
        transform.localScale = new Vector3(playerTransform.localScale.x, -playerTransform.localScale.y, playerTransform.localScale.z);
        shadowModel.transform.localEulerAngles = playerModel.transform.localEulerAngles;
    }
}
