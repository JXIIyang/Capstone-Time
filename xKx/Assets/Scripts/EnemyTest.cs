using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{

    public bool shadow;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * 5 * Time.deltaTime;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && CubicPlayerController.Singleton.ShadowMode == shadow)
        {
            Destroy(col.gameObject);
        }
    }
    
        
    private void OnCollisionStay(Collision col )
    {
        if (col.gameObject.CompareTag("Player")  && CubicPlayerController.Singleton.ShadowMode == shadow)
        {
            Destroy(col.gameObject);
        }
    }
    
    
}
