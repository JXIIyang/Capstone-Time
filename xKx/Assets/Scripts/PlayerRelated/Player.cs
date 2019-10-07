using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Singleton;
    public static Control Controller;
    public delegate void RegularControl();
    public event RegularControl ControlEvent;
    // Start is called before the first frame update
    public virtual void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton);
        }
        else
        {
            Debug.Log("Multiple Instances of Player");
            Destroy(this);
        }
    }
    
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        ControlEvent?.Invoke();
    }
    
    
}
