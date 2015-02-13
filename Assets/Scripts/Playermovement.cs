using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour {

	public ProceduralMaterial substance;
    public Power[] powers;
    public Power activePower;
    public GameObject gun;
    internal GameObject powerHolder;

    
    internal int hp = 100;
    internal Animator anim;

    //movement variables
    internal CharacterController controller;
    internal Vector3 startDepth;
    float speed = 3f;

    //jump variables
    internal bool canJump = true;
    Vector3 gravity = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    RaycastHit hit;
    int[] numberArray = new int[15];
    int a = 89, b = 55, c = 0, i = 1;
    bool jumping = false;
    bool falling = false;
    float timer;
    

    int powerCounter = 0;

	public virtual void Start () 
    {
        timer = 0;
        startDepth = transform.position;
        anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
        
        anim.SetBool("Move", true);
        
        activePower = powers[powerCounter];
        powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject;
	}

    public virtual void Update()
    {
        velocity = new Vector3(0, 0, Mathf.Abs(Input.GetAxis("Horizontal")));
        velocity.Normalize();
        velocity = transform.TransformDirection(velocity);

        velocity *= speed;


        if (canJump)
        {
            anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        }
        if (Input.GetAxis("Horizontal")>0.1f || Input.GetAxis("Horizontal")< -0.1f)
        {
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                transform.LookAt(transform.position + Camera.main.transform.right);
            }
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                transform.LookAt(transform.position - Camera.main.transform.right);
            }

        }

        if(Input.GetButton("Jump") && canJump && !jumping)
        {
            Debug.Log(timer);
            anim.SetBool("Move", false);
            jumping = true;
            timer += Time.deltaTime*2;
        }
        else if(timer > 0.04f)
        {
            timer = 0;
            jumping = false;
            canJump = false;
        }
        
        if (Input.GetButtonUp("Jump"))
        {
            Debug.Log(timer+" Let Go");
        
            timer = 0;
            jumping = false;
        }
        
        if (!canJump)
        {
            if (!jumping)
            {
                gravity += Physics.gravity * Time.deltaTime * 3;
            }
        }
        else if(jumping)
        {
            gravity.y = timer * 120f;
        }
        else
        {
            gravity = Vector3.zero;
        }
        velocity += gravity;
        velocity.x = 0;
        controller.Move(velocity * Time.deltaTime);
        canJump = controller.isGrounded;

    }
	public virtual void FixedUpdate () 
    {

        #region Fixes

        //Sets player x position back to begin x position when changed
        if (transform.position.x > startDepth.x)
        {
            transform.position = new Vector3(startDepth.x,transform.position.y,transform.position.z);
        }

        

        #endregion

        //Set Power
        if (Input.touchCount >= 1 || Input.GetMouseButtonDown(0))
        {
            activePower = powers[powerCounter];
            if (powerHolder.name != activePower.name)
            {
                Destroy(powerHolder);
                powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject;
            }
        }

        //Fire power
        if (Input.GetMouseButtonUp(0))
		{
            activePower.Attack(transform);
		}
        
        //Switch to next power
        if (Input.GetMouseButtonDown(1))
        {
            SwitchPower();
        }

        if (substance.GetProceduralFloat("Snow") >= 1)
        {
            SummonBoss();
        }


        //controller.velocity = velocity;
	}

    //public IEnumerator Jump()
    //{
    //    canJump = false;
    //
    //    if (jumping) { yield return null; }
    //
    //    if (i > 2 && !falling) 
    //    {
    //        a = 89;
    //        b = 55;
    //        anim.SetTrigger("Jump");
    //        yield return new WaitForSeconds(0.05f); 
    //    }
    //    else { yield return null; }
    //
    //
    //    while (i>2 && !canJump)
    //    {
    //        jumping = true;
    //        for (i = a; i >2 ; i = i - c)
    //        {
    //            c = a - b;
    //            a = b;
    //            b = c;
    //
    //            if (c > 4)
    //            {
    //                ChangeVelocity(velocity.x, c /5 , Input.GetAxis("Horizontal") * speed);
    //                controller.Move(velocity.y * transform.up);
    //
    //                yield return new WaitForSeconds(0.15f);
    //            }
    //            if (c < 4)
    //            {
    //                ChangeVelocity(velocity.x, c /5, Input.GetAxis("Horizontal") * speed);
    //                controller.Move(velocity.y * transform.up);
    //
    //            }
    //        }
    //    }
    //    jumping = false;
    //    //StartCoroutine("Falling");
    //    yield return null;       
    //}
    //
    //public IEnumerator Falling()
    //{
    //    if (falling || jumping || falling && jumping) { yield return null; }
    //
    //    if (!jumping && !canJump)
    //    {
    //        for (i = 0; i < 11; i++)
    //        {
    //            falling = true;
    //            if (i == 0)
    //            {
    //                numberArray[i] = i;
    //            }
    //
    //            if (i == 1)
    //            {
    //                numberArray[i] = i;
    //            }
    //            if (i >= 2)
    //            {
    //                numberArray[i] = numberArray[i - 1] + numberArray[i - 2];
    //            }
    //            if (i > 5)
    //            {
    //                ChangeVelocity(velocity.x, -(numberArray[i]/5 ), Input.GetAxis("Horizontal") * speed * 1.2f);
    //                controller.Move(velocity.y * transform.up);
    //
    //                yield return new WaitForSeconds(0.1f);
    //            }
    //        }
    //    }
    //        falling = false;
    //        //canJump = true;
    //        yield return null;
    //}


    public void SummonBoss()
    {

    }

    public void SwitchPower()
    {
        powerCounter++;
        if (powerCounter >= powers.Length) { powerCounter = 0;}
    }
}