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
    public Vector3 F_Rotate = Vector3.zero;
    public Vector3 R_Rotate = Vector3.zero;

    public Animator PlayerAnimator;
    public Animator ShadowAnimator;

    public Transform Model;
    public Transform Shadow;
    
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
        //Player.Singleton.ControlEvent += Jump;
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

        if (Player.Singleton.PlayerState == Player.State.Idle)
        {
            R_Rotate = Vector3.zero;
        }
        if (Player.Singleton.PlayerState == Player.State.Combat && CameraController.Instance.CamState == Player.State.Combat)
        {
            DepthMovement();
            Debug.Log("zsubscribe");
        }
        
        Shadow.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
        Shadow.localEulerAngles = Model.transform.localEulerAngles + transform.localEulerAngles;
    }
    
    
    public void HorizontalMovement()
    {
        Model.transform.LookAt(Model.transform.position + new Vector3(ForwardInput, 0, -InwardInput));
        //Model.transform.localEulerAngles = Vector3.Lerp(Model.transform.localEulerAngles, F_Rotate + R_Rotate, 0.1f);
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            ForwardInput = 1;
//            F_Rotate = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            ForwardInput = -1;
//            F_Rotate = Vector3.up * 180;
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) ForwardInput = 0;
//        Model.transform.localEulerAngles = new Vector3(0, ForwardInput > 0 ? 0 : 180, 0);       
        transform.position += ForwardInput * transform.forward * Speed * Time.deltaTime;
        Debug.Log(transform.forward);
    }
    
    public void DepthMovement()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            InwardInput = -1;
//            R_Rotate = Vector3.up * -90;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            InwardInput = 1;
//            R_Rotate = Vector3.up * 90; 
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) 
            InwardInput = 0;
        if (Mathf.Abs(ForwardInput) > 0.01f && Mathf.Abs(InwardInput) < 0.01f) R_Rotate = Vector3.zero;
        transform.position += InwardInput * transform.right * Speed * Time.deltaTime;
        Debug.Log("zinput  " + InwardInput);
    }
    
    public void Jump()
    {
        var ty = transform.position.y;
        var my = JumpMovement.y;
        if (ty < 0.001f)
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
