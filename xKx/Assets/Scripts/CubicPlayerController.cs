using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;

public class CubicPlayerController : MonoBehaviour
{
    public static CubicPlayerController Singleton;
    
    public KeyCode MoveRight;
    public KeyCode MoveLeft;
    public KeyCode MoveForward;
    public KeyCode MoveBackward;
    public KeyCode JumpKey;

    public float hinput; 
    public float vinput; 

    public float Speed;
    public float JumpSpeed;
    public float _jumpSpeed;
    public float Gravity;
    public float _gravity;
    public Animator PlayerAnimator;
    public Animator ShadowAnimator;

    public SkinnedMeshRenderer ModelRenderer;
    public MeshRenderer SwordRenderer;
    public MeshRenderer TrailRenderer;
    public SkinnedMeshRenderer shadowRenderer;
    public MeshRenderer shadowSwordRenderer;


    public Transform PlayerModel;

    public float damageDelay;
    private float _attackTimer;
    public bool performDamage = false;

    public GameObject reverseBackground;
    public GameObject normalBackground;
    public Camera mainCamera;
    public Color normalBackgroundColor;
    public Color reverseBackgroundColor;
    
    public Material blackCutOut;
    public Material shadowNormal;
    public Material shadowReverse;
    public Material whiteCutOut;


    public enum PlayerState
    {
        Grounded,
        Jump,
        Attack,
        Dash
    }
    
    public bool ShadowMode;
    public Vector3 ShadowPos;
    public Vector3 CurrentPos;
    public float dashTime;
    private float _dashTimer;
    public RectangularMeshTrail MeshTrail;

    private float _tempGravity;
    private float _tempJumpSpeed;

    public PlayerState MyState;
    [SerializeField]
    private PlayerState MyLastState;
    
    public enum Orient
    {
        Right,
        Left
    }

    public Orient FaceOrient;
    
    
    private void Awake()
    {
        Singleton = this;
    }
    
    
    void Start()
    {
        MyState = PlayerState.Grounded;
        MyLastState = PlayerState.Grounded;
        FaceOrient = Orient.Right;
    }

    void Update()
    {
        Movement();
        Jump();
        Attack();
        ShadowDash();
        
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    private void Movement()
    {
        hinput = Input.GetAxis("Horizontal");
        vinput = Input.GetAxis("Vertical");

        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) hinput = 0;
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) vinput = 0;
        
        if (MyState != PlayerState.Attack) transform.Translate(Vector3.right * hinput * Speed * Time.deltaTime);

        if (hinput > 0)  SetOrient(Orient.Right);
        if (hinput < 0)  SetOrient(Orient.Left);


        if (Mathf.Abs(hinput) > 0.1f)
        {
            PlayerAnimator.SetInteger("State", 1);
            ShadowAnimator.SetInteger("State", 1);
        }
        else
        {
            PlayerAnimator.SetInteger("State", 0);
            ShadowAnimator.SetInteger("State", 0);
        }

            

    }

    private void Jump()
    {
        var index = Mathf.Sign(Gravity);
        if (Input.GetKeyDown(JumpKey) && JumpAllowed())
        {
            MyLastState = MyState;
            MyState = PlayerState.Jump;
            PlayerAnimator.SetTrigger("Jump");
            ShadowAnimator.SetTrigger("Jump");
            _jumpSpeed = JumpSpeed;
            _gravity = Gravity * 0.7f;

        }
         
        /*if(MyLastState == PlayerState.Dash && Input.GetKeyDown(JumpKey))
        {
            MyLastState = MyState;
            MyState = PlayerState.Jump;
            PlayerAnimator.SetTrigger("Jump");
            ShadowAnimator.SetTrigger("Jump");
        }*/

        if (MyState == PlayerState.Jump)
        {
           
            _gravity += index * 0.2f * Time.deltaTime;
            if (MyLastState == PlayerState.Grounded) _jumpSpeed -=_gravity * Time.deltaTime;
            else if (MyLastState == PlayerState.Attack || MyLastState == PlayerState.Dash) _jumpSpeed -= 1.4f * _gravity * Time.deltaTime;
            transform.Translate(Vector3.up * _jumpSpeed);
            
            if ((Gravity > 0 && transform.position.y < -0.01f) || (Gravity < 0 && transform.position.y > 0.01f))
            {
                ResetHeight();
                MyLastState = MyState;
                MyState = PlayerState.Grounded;
                _gravity = 0;
                _jumpSpeed = 0;
            }
        }

        if (MyState == PlayerState.Attack && MyLastState == PlayerState.Jump)
        {
            _gravity += index * 0.2f * Time.deltaTime;
            _jumpSpeed -=  0.5f * _gravity * Time.deltaTime;
            transform.Translate(Vector3.up * _jumpSpeed);
            
            if ((Gravity > 0 && transform.position.y < -0.01f) || (Gravity < 0 && transform.position.y > 0.01f))
            {
                ResetHeight();
                _gravity = 0;
                _jumpSpeed = 0;
            }
        }
        
       
    }

    public void SetOrient(Orient o)
    {
        FaceOrient = o;
        switch (o)
        {
            case Orient.Left:
                PlayerModel.localEulerAngles = new Vector3(0, -90, 0);
                break;
            case Orient.Right:
                PlayerModel.localEulerAngles = new Vector3(0, 90, 0);
                break;        
               
        }
        
    }
    

    private bool JumpAllowed()
    {
        if (MyState == PlayerState.Grounded) return true;
        
        return false;
    }

    public void ResetHeight()
    {
        var x = transform.position.x;
        var z = transform.position.z;
        transform.position = new Vector3(x, 0, z);
    }


    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && MyState != PlayerState.Attack)
        {
            MyLastState = MyState;
            MyState = PlayerState.Attack;
            PlayerAnimator.SetTrigger("Attack");
            ShadowAnimator.SetTrigger("Attack");
            
        }

        if (MyState == PlayerState.Attack)
        {
            _attackTimer++;
            performDamage = _attackTimer > damageDelay;
            
            var o = FaceOrient == Orient.Right ? 1 : -1;
            transform.Translate(Vector3.right * o * Speed * 0.2f * Time.deltaTime);

            if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                MyState = MyLastState;
                MyLastState = PlayerState.Attack;
                performDamage = false;
                _attackTimer = 0;
                transform.Translate(Vector3.right * o * Speed * 0.5f * Time.deltaTime);
            }
        }
        
        
    }
    
    private void ShadowDash()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            MeshTrail.height = 0.3f;
            MyLastState = MyState;
            MeshTrail.dieFrame = 0;
            _tempGravity = -_gravity;
            _tempJumpSpeed = -_jumpSpeed;
            MyState = PlayerState.Dash;            
            ShadowMode = !ShadowMode;
            

            if (ShadowMode)
            {
                ModelRenderer.material = whiteCutOut;
                TrailRenderer.material = whiteCutOut;
                SwordRenderer.material = whiteCutOut;
            }
            else
            {
                ModelRenderer.material = blackCutOut;
                TrailRenderer.material = blackCutOut;
                SwordRenderer.material = blackCutOut;
            }
            
            
            _dashTimer = dashTime;
            CurrentPos = transform.position;
            ShadowPos = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
            transform.localScale = new Vector3(1, -transform.localScale.y, 1);
        }

        if (MyState == PlayerState.Dash && _dashTimer > 0)
        {
            
            _dashTimer--;
            transform.position = Vector3.Slerp(transform.position, ShadowPos + Vector3.right * 1f * (FaceOrient == Orient.Right ? 1 : -1), 0.1f);
        }
        else if (MyState == PlayerState.Dash && _dashTimer <= 0)
        {
            
            MeshTrail.height = 0.1f;
            MeshTrail.dieFrame = 60;
            //_jumpSpeed = _tempJumpSpeed;
            _jumpSpeed = 0;
            _gravity = _tempGravity;
            MyState = MyLastState == PlayerState.Dash ? PlayerState.Grounded : MyLastState;
            MyLastState = PlayerState.Dash;
            Gravity = transform.localScale.y * Mathf.Abs(Gravity);
            JumpSpeed = transform.localScale.y * Mathf.Abs(JumpSpeed);

            if (ShadowMode)
            {
                normalBackground.SetActive(false);
                reverseBackground.SetActive(true);
                shadowRenderer.material = shadowReverse;
                shadowSwordRenderer.material = shadowReverse;
                mainCamera.backgroundColor = reverseBackgroundColor;
            }
            else
            {
                normalBackground.SetActive(true);
                reverseBackground.SetActive(false);
                shadowRenderer.material = shadowNormal;
                shadowSwordRenderer.material = shadowNormal;
                mainCamera.backgroundColor = normalBackgroundColor;
            }
        } 
        
    }
    
}
