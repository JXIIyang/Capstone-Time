using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Start is called before the first frame update
    public CubicPlayerController MyPlayer;
    public Material BlackMat;
    public Material WhiteMat;

    public bool DamageVisualized;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DamageVisualized) Visualization();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!MyPlayer.Death && MyPlayer.performDamage && other.CompareTag("Enemy"))
        {
            var c = other.GetComponentInParent<EnemyController>();
            if (!c.Death)
            {
               c.Die();
            }
            //Destroy(other.transform.parent.gameObject);
            //Debug.Log("Attack Performed");
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (!MyPlayer.Death && MyPlayer.performDamage && other.CompareTag("Enemy"))
        {
            var c = other.GetComponentInParent<EnemyController>();
            if (!c.Death)
            {
               c.Die();
            }
            //Destroy(other.transform.parent.gameObject);
            //Debug.Log("Attack Performed");
        }
        
    }

    public void Visualization()
    {
        if (MyPlayer.performDamage)
        {
            GetComponent<MeshRenderer>().material = WhiteMat;
        }
        else
        {
            GetComponent<MeshRenderer>().material = BlackMat;
        }
    }
}
