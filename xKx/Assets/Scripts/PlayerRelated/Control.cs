using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public float Speed;
    public float JumpSpeed;
    
    public Vector3 JumpMovement = Vector3.zero;
    public float Gravity;
    public bool Grounded;
    public float ForwardInput;
    private float lastForwardInput;
    public float InwardInput;

    public Animator PlayerAnimator;
    public Animator ShadowAnimator;

    public Transform Model;
    
    public void Awake()
    {
        if (Player.Controller == null)
        {
            Player.Controller = GetComponent<Control>();
            //DontDestroyOnLoad(Player.Controller);
        }
        else
        {
            Debug.Log("Multiple Instances of Controller");
            Destroy(this);
        }
    }

    public void Start()
    {
        Player.Singleton.ControlEvent += HorizontalMovement;
        Player.Singleton.ControlEvent += Jump;
    }

    public void Update()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            PlayerAnimator.Play("Run");
            ShadowAnimator.Play("Run");
        }
        else
        {
            PlayerAnimator.Play("Idle");
            ShadowAnimator.Play("Idle");
        }
        if (Player.Singleton.PlayerState == Player.State.Combat)
        {
            DepthMovement();
            Debug.Log("zsubscribe");
        }
    }
    
    
    public void HorizontalMovement()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            ForwardInput = 1;
            Model.transform.localEulerAngles = new Vector3(0, 0, 0); 
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            ForwardInput = -1;
            Model.transform.localEulerAngles = new Vector3(0, 180, 0); 
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) ForwardInput = 0;
//        Model.transform.localEulerAngles = new Vector3(0, ForwardInput > 0 ? 0 : 180, 0);       
        transform.position += ForwardInput * transform.forward * Speed * Time.deltaTime;
        Debug.Log(transform.forward);
    }
    
    public void DepthMovement()
    {
        InwardInput = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A)? -1 : 0);
        //transform.localEulerAngles = new Vector3(0, 90 * Mathf.Sign(ForwardInput), 0);
        transform.position += InwardInput * transform.right * Speed * Time.deltaTime;
        Debug.Log("zinput  " + InwardInput);
    }
    
    public void Jump()
    {
        var ty = transform.position.y;
        var my = JumpMovement.y;
        if (ty < 0.1f)
        {
            Grounded = true;
            ty = 0;
            my = 0;
        }
        if(!Grounded) {
            my -= Gravity * Time.deltaTime;
        } else {
            if(Input.GetKeyDown(KeyCode.Space)) {
                my = JumpSpeed;
                Grounded = false;
            }
        }

        JumpMovement = new Vector3(JumpMovement.x, my, JumpMovement.z);
        transform.position = new Vector3(transform.position.x, ty + my, transform.position.z);
    }
}
