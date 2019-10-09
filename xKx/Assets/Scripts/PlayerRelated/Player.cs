using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Singleton;
    public static Control Controller;
    public delegate void RegularControl();
    public event RegularControl ControlEvent;

    public Transform Shadow;
    
    public BoxCollider CombatTrigger;

    public enum State
    {
        Idle,
        Combat,
        Dialogue,
        Transit
    }

    public State PlayerState;

    public Transform Model;
    
    // Start is called before the first frame update
    public virtual void Awake()
    {
        
        if (Singleton == null)
        {
            Singleton = this;
//            DontDestroyOnLoad(Singleton);
        }
        else
        {
            Debug.Log("Multiple Instances of Player");
            Destroy(this);
        }
    }
    
    public virtual void Start()
    {
        PlayerState = State.Idle;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        ControlEvent?.Invoke();
        if (PlayerState == State.Idle)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0); 
            Model.transform.localEulerAngles = Vector3.zero;
        }
        
        Shadow.position = new Vector3(transform.position.x, -transform.position.y, 0);
        Shadow.localEulerAngles = transform.localEulerAngles;
        
    }
    
    
    private void OnTriggerEnter(Collider col)
    {
        if (col == CombatTrigger)
        {
            PlayerState = State.Combat;
        }
        
    }
    
    
}
