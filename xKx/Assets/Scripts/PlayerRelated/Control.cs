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

    public Animator PlayerAnimator;
    public Animator ShadowAnimator;

    
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

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            PlayerAnimator.Play("Run");
            ShadowAnimator.Play("Run");
            Debug.Log("run");
        }
        else
        {
            Debug.Log("idle");
            PlayerAnimator.Play("Idle");
            ShadowAnimator.Play("Idle");
        }
    }
    
    
    public void HorizontalMovement()
    {
        ForwardInput = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S)? -1 : 0);
        transform.localEulerAngles = new Vector3(0, 90 * Mathf.Sign(ForwardInput), 0);
        transform.position += Mathf.Abs(ForwardInput) * transform.forward * Speed * Time.deltaTime;
        Debug.Log(transform.forward);
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
