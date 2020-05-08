using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamage : MonoBehaviour
{

    public EnemyController myController;
    // Start is called before the first frame update
    void Start()
    {
        myController = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.tag);
        if (myController.isAttacking && myController.performDamage && other.CompareTag("PlayerCollider"))
        {
            Debug.Log("Hurt");
            CubicPlayerController.Singleton.PlayerAnimator.SetTrigger("Death");
            CubicPlayerController.Singleton.ShadowAnimator.SetTrigger("Death");
            CubicPlayerController.Singleton.Death = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {

        Debug.Log(other.tag);
        if (myController.isAttacking && myController.performDamage && other.CompareTag("PlayerCollider"))
        {
            Debug.Log("Hurt");
            CubicPlayerController.Singleton.PlayerAnimator.SetTrigger("Death");
            CubicPlayerController.Singleton.ShadowAnimator.SetTrigger("Death");
            CubicPlayerController.Singleton.Death = true;
        }
    }
}
