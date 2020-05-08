using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SideAttackTest : MonoBehaviour
{

    public float Speed;
    private float _initialY;
    public float Gravity;
    private float vy;
    public float JumpForce;

    private float rushTimer = -999;
    private float rushDir;

    public bool Grounded = true;

    public MeshRenderer MyRenderer;
    public MeshRenderer ShadowRenderer;
    public Material PrimaryMat;
    public Material SecondaryMat;

    public GameObject Shadow;
    public GameObject TrailPrefab;


    public Vector3 LastDraw;
    public RectangularMeshTrail TrailScript;

    public enum Mode
    {
        Body,
        Shadow
    }

    public Mode PlayerMode;
    

    public enum State
    {
        Idle,
        Run,
        Dash
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _initialY = transform.position.y;
        MyRenderer = GetComponent<MeshRenderer>();
        ShadowRenderer = Shadow.GetComponent<MeshRenderer>();
        PlayerMode = Mode.Body;
        //TrailScript.enabled = false;

    }

    void Update()
    {

       

        Shadow.GetComponent<Rigidbody>().isKinematic = PlayerMode == Mode.Body;
        GetComponent<Rigidbody>().isKinematic = PlayerMode == Mode.Shadow;

                 
      /*  
        if (!Grounded && Input.GetMouseButtonUp(0)) ExecuteSwitch();
        if (rushTimer > -0.1f)
        {
            var targetYPos = Mathf.Lerp(-transform.position.y, transform.position.y, rushTimer / 0.1f) * (PlayerMode == Mode.Body ? -1 : 1);

            TrailScript.enabled = true;
            
            /*Instantiate(TrailPrefab,
                new Vector3(transform.position.x, targetYPos, transform.position.z),
                Quaternion.identity);
                */
            /*var num = 10f;
            for (int i = 0; i < num; i++)
            {
                var t = Instantiate(TrailPrefab,
                    Vector3.Lerp(LastDraw, new Vector3(transform.position.x, targetYPos, transform.position.z), i/num),
                    Quaternion.identity);
                t.transform.localScale *= Mathf.Lerp(0.9f, 0.5f, rushTimer / 0.1f) * Mathf.Pow(0.95f, (num-i));
            }
            
            LastDraw = new Vector3(transform.position.x, targetYPos, transform.position.z);
        }
        else
        {
            TrailScript.enabled = false;
            MyRenderer.enabled = true;
            ShadowRenderer.enabled = true;
            LastDraw = PlayerMode == Mode.Body ? transform.position : new Vector3(transform.position.x, -transform.position.y, transform.position.z);
        }


        */

    }
    
    
    // Update is called once per frame
    void FixedUpdate()
    {
        var xInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right  * xInput * Speed * Time.deltaTime);

        var x = transform.position.x;
        var y = transform.position.y;
        

        if (!Grounded)
        {
            vy -= Gravity * Time.fixedDeltaTime * (rushTimer > 0 ? 0.2f : 1);
            
        }
        
        if (Input.GetKey(KeyCode.Space) && Grounded)
        {
            vy += JumpForce  * Time.fixedDeltaTime;
            Grounded = false;           
        } else if (transform.position.y < _initialY)
        {
            Grounded = true;
            y = _initialY;
            vy = 0;
        }

        
        if (rushTimer > -0.1f) rushTimer -= Time.fixedDeltaTime;
        if (rushTimer > 0)
        {
            
            x += 10 * Time.fixedDeltaTime * rushDir;

        }
        
        transform.position = new Vector3(x, y + vy, transform.position.z);
        Shadow.transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
        
    }


    public void ExecuteSwitch()
    {
        if (PlayerMode == Mode.Body)
        {
            PlayerMode = Mode.Shadow;
            MyRenderer.material = SecondaryMat;
            ShadowRenderer.material = PrimaryMat;


        } else if (PlayerMode == Mode.Shadow)
        {
            PlayerMode = Mode.Body;
            MyRenderer.material = PrimaryMat;
            ShadowRenderer.material = SecondaryMat;
        }

        rushTimer = 0.1f;
        rushDir = Mathf.Sign(Input.GetAxis("Horizontal"));
        MyRenderer.enabled = false;
        ShadowRenderer.enabled = false;





    }
    
    private void OnDestroy()
    {
        Destroy(Shadow);
    }
    
    
}
