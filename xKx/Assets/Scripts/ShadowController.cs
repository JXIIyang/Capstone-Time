using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{

    public Transform baseTransform;

    public Transform baseModel;
    public Transform shadowModel;
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(baseTransform.position.x, -baseTransform.position.y, baseTransform.position.z);
        transform.localScale = new Vector3(baseTransform.localScale.x, -baseTransform.localScale.y, baseTransform.localScale.z);
        shadowModel.transform.localEulerAngles = baseModel.transform.localEulerAngles;

    }
}
