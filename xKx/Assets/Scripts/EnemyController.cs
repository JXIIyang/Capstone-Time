using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float sightRange;
    public float hearRange;
    public CubicPlayerController.Orient enemyFaceOrient;
    public bool awake;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInSight(awake))
        {
            awake = true;
            ChasePlayer();
        }
    }

    bool PlayerInSight(bool a)
    {
        var dis = CubicPlayerController.Singleton.transform.position.x - transform.position.x;
        if (a) return true;
        if (!a && dis <= hearRange && dis >= -sightRange && enemyFaceOrient == CubicPlayerController.Orient.Left) return true;
        if (!a && dis >= -hearRange && dis <= sightRange && enemyFaceOrient == CubicPlayerController.Orient.Right) return true;
        return false;

    }


    private void ChasePlayer()
    {
        
    }
}
