using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public float sightRange;
    public float hearRange;
    public float attackRange;
    public float longAttackRange;
    public float decisionRange;
    public CubicPlayerController.Orient enemyFaceOrient;
    public bool awake;
    public bool lost;
    public float wakeTime;
    public float speed;
    public float walkSpeed = 0.8f;
    private Vector3 _tempDir;
    private Vector3 _dir;
    private Vector3 _prevDir = new Vector3(0,0,0);

    private float _decisionCounter;
    private bool _decisionMade = false;
    private bool _backUp = false;

    private float _speedScale;
    private float _walkSpeedScale = 1;

    public GameObject enemyModel;
    public Animator myAnimator;
    public Animator shadowAnimator;

    public bool isAttacking = false;

    public float damageDelay;
    private float _attackTimer;
    public bool performDamage;

    public float jumpSpeed;
    private float _jumpSpeed;
    public float gravity;
    private float _gravity;
    public bool grounded = true;
    public bool Jump = false;
    public bool JumpAttacking;

    private bool _crouchAttack = false;


    public bool jumpAllowed = true;

    public ShadowController shadowPrefab;
    public ShadowController myShadow;

    public bool Death;
    public Collider weaponCollider;
    
    

    
    // Start is called before the first frame update
    void Start()
    {
        
        if (enemyFaceOrient == CubicPlayerController.Orient.Left) enemyModel.transform.localEulerAngles = new Vector3(0,-90,0);
        else enemyModel.transform.localEulerAngles = new Vector3(0,90,0);
        myAnimator = GetComponent<Animator>();
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;        
        myShadow = Instantiate(shadowPrefab, pos, rotation);
        shadowAnimator = myShadow.gameObject.GetComponent<Animator>();
        myShadow.baseModel = enemyModel.transform;
        myShadow.baseTransform = transform;
        


    }

    // Update is called once per frame
    void Update()
    {
        if (Death) return;
        if (CubicPlayerController.Singleton.Death)
        {
            myAnimator.SetBool("Run", false);
            shadowAnimator.SetBool("Run", false);
        }
        if (PlayerInSight(awake) && !awake)
        {
            StartCoroutine(WakeUp());
            //_prevDir = Vector3.Normalize()

        }


        if (awake && !CubicPlayerController.Singleton.Death && !lost) ChasePlayer();
        isAttacking = myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("CrouchAttack");
        //if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("CrouchAttack")) _crouchAttack = false;
        if (!isAttacking)
        {
            performDamage = false;
            _attackTimer = 0;
        }
        
        if ( myAnimator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") && !grounded) transform.position += 1f * _dir * Time.deltaTime;

        if (lost) LostIdle();

    }

    private void FixedUpdate()
    {
        if (Jump) DoJump();
    }


    private void LostIdle()
    {
        
        myAnimator.SetBool("Lost", true);
        shadowAnimator.SetBool("Lost", true);
        transform.position += _dir * walkSpeed * _walkSpeedScale * Time.deltaTime;
        Debug.Log("Move");
        


    }

    bool PlayerInSight(bool a)
    {
        var dis = CubicPlayerController.Singleton.transform.position.x - transform.position.x;
        if (a && !CubicPlayerController.Singleton.ShadowMode) return true;
        if (a && CubicPlayerController.Singleton.ShadowMode)
        {
            
            StartCoroutine(LostCount(Random.Range(2, 5)));
            myAnimator.SetBool("Awake", false);
            shadowAnimator.SetBool("Awake", false);
            awake = false;
            return false;
        }
        if (!a  && !CubicPlayerController.Singleton.ShadowMode && dis <= hearRange && dis >= -sightRange && enemyFaceOrient == CubicPlayerController.Orient.Left) return true;
        if (!a  && !CubicPlayerController.Singleton.ShadowMode && dis >= -hearRange && dis <= sightRange && enemyFaceOrient == CubicPlayerController.Orient.Right) return true;
        return false;

    }
    
    private IEnumerator LostCount( float t)
    {
        lost = true;
        yield return new WaitForSeconds(t);
        myAnimator.SetBool("Lost", false);
        shadowAnimator.SetBool("Lost", false);
        lost = false;
    }
    
    
    private IEnumerator WakeUp()
    {
        myAnimator.SetBool("Awake", true);
        shadowAnimator.SetBool("Awake", true);
        yield return new WaitForSeconds(wakeTime);
        awake = true;
        
    }


    private void ChasePlayer()
    {
        var px = CubicPlayerController.Singleton.transform.position.x;
        var py = CubicPlayerController.Singleton.transform.position.y;
        var pz = CubicPlayerController.Singleton.transform.position.z;
        
        _tempDir = CubicPlayerController.Singleton.transform.position - transform.position;
        _dir = Vector3.Normalize(new Vector3(_tempDir.x, 0, _tempDir.z));

        if (Vector3.Dot(_dir, _prevDir) < 0.5f) _dir = 0.1f * (_dir + 9 * _prevDir);
        var dis = Vector3.Distance(new Vector3(px, 0, pz), new Vector3(transform.position.x, 0, transform.position.z));

        if ( dis > decisionRange && !_backUp && !isAttacking)
        {
            if (!_decisionMade)
            {
                myAnimator.SetBool("Run", true);
                shadowAnimator.SetBool("Run", true);
                transform.position += speed * _dir * Time.deltaTime;
                
            }
            else
            {
                myAnimator.SetBool("Run", false);
                shadowAnimator.SetBool("Run", false);
            }

        } 
        else if (dis < decisionRange && dis > attackRange)
        {           
            if (!_decisionMade)
            {
                var chance = Random.value;
              _backUp = chance  > 0.7f ? true : false;
              _speedScale = chance > 0.7f ? Random.Range(0.9f, 1.3f) : Random.Range(0.6f, 1f);
              _decisionMade = true;
              _decisionCounter = _backUp ? Random.Range(50,100) : Random.Range(350, 500);
              
              
            }
            else if (_decisionMade && !_backUp)
            {
                myAnimator.SetBool("Run", true);
                shadowAnimator.SetBool("Run", true);
                transform.position += speed * _dir * Time.deltaTime;                
                myAnimator.speed = 1;
                shadowAnimator.speed = 1;
            }
        }
        else if (dis <= attackRange && !CubicPlayerController.Singleton.Death)
        {
            Attack();
        }

        if (dis <= longAttackRange && dis > attackRange && !CubicPlayerController.Singleton.Death)
        {
            
            var b = CubicPlayerController.Singleton.MyState == CubicPlayerController.PlayerState.Attack ? 0.8f : 0.01f;
            if (Random.value < b)
            {
                _crouchAttack = true;
                Attack();
            }
        }
        
        
        


        if (_backUp && !isAttacking)
        {
            myAnimator.SetBool("Backward", true);
            shadowAnimator.SetBool("Backward", true);
            _speedScale = dis > sightRange ? _speedScale : Mathf.Lerp(_speedScale, 0, 0.02f);
            transform.position -= _speedScale * speed * _dir * Time.deltaTime;
            if (_speedScale > 0.5f)
            {
                myAnimator.SetBool("Run", true);
                shadowAnimator.SetBool("Run", true);
                myAnimator.speed = _speedScale * 1.2f;
                shadowAnimator.speed = _speedScale * 1.2f;
            }
            else
            {
                _speedScale = 0;
                myAnimator.SetBool("Run", false);
                shadowAnimator.SetBool("Run", false);
                myAnimator.speed = 1.2f;
                shadowAnimator.speed = 1.2f;
            }

        }
        else
        {
            myAnimator.SetBool("Backward", false);
            shadowAnimator.SetBool("Backward", false);
        }

        if (_decisionMade && _decisionCounter > 0)
        {
            _decisionCounter--;
            
        } else if (_decisionMade && _decisionCounter <= 0)
        {
            _decisionMade = false;
            _decisionCounter = 0;
            _speedScale = 1;
            _backUp = false;
            myAnimator.SetBool("Run", true);
            shadowAnimator.SetBool("Run", true);
            myAnimator.speed = _speedScale;
            shadowAnimator.speed = _speedScale;
        }

        if (dis < decisionRange - 1 && _decisionMade && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Wake_Idle"))
        {
            var chance = Random.value;
            if (chance > 0.5f)
            {
                _backUp = true;
            }
            else
            {
                _decisionMade = false;
                _decisionCounter = 0;
                _speedScale = 1;
                _backUp = false;
                myAnimator.SetBool("Run", true);
                shadowAnimator.SetBool("Run", true);
                myAnimator.SetBool("Backward", false);
                shadowAnimator.SetBool("Backward", false);
                myAnimator.speed = _speedScale;
                shadowAnimator.speed = _speedScale;
                
            }
            _speedScale = 1;
        }
        
        
        _prevDir = _dir;

        if (px < transform.position.x)
        {
            if (enemyFaceOrient != CubicPlayerController.Orient.Left && !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
            {
                    enemyFaceOrient = CubicPlayerController.Orient.Left;
                    StartCoroutine(TurnAround(-1, 0f));             
            }
        } else if (enemyFaceOrient != CubicPlayerController.Orient.Right &&
                   !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
        {
            enemyFaceOrient = CubicPlayerController.Orient.Right;
            StartCoroutine(TurnAround(1, 0f));
        }

        if (dis < attackRange + 0.5f && grounded && !CubicPlayerController.Singleton.Death && jumpAllowed)
        {
            var chance = Random.value;
            var b = py > transform.position.y ? 0.7f : 0.3f;
            if (chance < b && !isAttacking)
            {
                _jumpSpeed = jumpSpeed;
                _gravity = 0.7f * gravity;
                grounded = false;
                Jump = true;
                jumpAllowed = false;
                myAnimator.SetBool("Jump", true);
                shadowAnimator.SetBool("Jump", true);
            }
        }

    }

    public void DoJump()
    {

        var index = Mathf.Sign(gravity);
        

            _gravity += index * 0.5f * Time.deltaTime;
            _jumpSpeed -= _gravity * Time.deltaTime;
            transform.Translate(Vector3.up * _jumpSpeed);
            
            if ((gravity > 0 && transform.position.y < -0.01f) || (gravity < 0 && transform.position.y > 0.01f))
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack")) grounded = true;
                Jump = false;
                _gravity = 0;
                _jumpSpeed = 0;
                StartCoroutine(JumpDelay(1, 3));
                myAnimator.SetBool("Jump", false);
                shadowAnimator.SetBool("Jump", false);
            }       
        
    }

    public IEnumerator JumpDelay(float min, float max)
    {
        var t = Random.Range(min, max);
        yield return new WaitForSeconds(t);
        jumpAllowed = true;
    }

    public IEnumerator TurnAround(float s, float t)
    {
        yield return new WaitForSeconds(t);
        enemyModel.transform.localEulerAngles = new Vector3(0, s * 90, 0);
    }
    
    private void Attack()
    {
        _attackTimer++;

        if (_crouchAttack)
        {
            performDamage = _attackTimer > damageDelay/2;
            myAnimator.SetTrigger("CrouchAttack");
            shadowAnimator.SetTrigger("CrouchAttack");
        }
        else
        {
            performDamage = _attackTimer > damageDelay;
            myAnimator.SetTrigger("Attack");
            shadowAnimator.SetTrigger("Attack");
        }
        
        if (Jump)
        {
            StartCoroutine(JumpAttackGroundedDelay());
        }
    }

    private IEnumerator JumpAttackGroundedDelay()
    {
        grounded = false;
        yield return new WaitForSeconds(0.5f);
        grounded = true;
    }

    public void Die()
    {
        
        Death = true;
        weaponCollider.enabled = false;
        
        myAnimator.SetTrigger("Death01");
        shadowAnimator.SetTrigger("Death01");
        
    }

    private void OnDestroy()
    {
        //Destroy(myShadow.gameObject);
    }
}
