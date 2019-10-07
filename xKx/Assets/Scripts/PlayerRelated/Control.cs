using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : Player
{
    public float Speed;
    public float ForwardInput;

    public override void Awake()
    {
        base.Awake();
        if (Controller == null)
        {
            Controller = GetComponent<Control>();
            DontDestroyOnLoad(Controller);
        }
        else
        {
            Debug.Log("Multiple Instances of Controller");
            Destroy(this);
        }
    }

    public override void Start()
    {
        base.Start();
        ControlEvent += HorizontalMovement;
    }
    
    public void HorizontalMovement()
    {
        ForwardInput = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S)? -1 : 0);
        transform.position += ForwardInput * transform.forward * Speed * Time.deltaTime;
    }
}
