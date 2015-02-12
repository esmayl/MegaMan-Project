using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour {

	public ProceduralMaterial substance;
    public Power[] powers;
    public Power activePower;
    public GameObject gun;
    internal GameObject powerHolder;

    
    internal int hp = 100;
    internal Animator anim;

    //movement variables
    internal Rigidbody controller;
    internal Vector3 startDepth;
    float speed = 3;

    //jump variables
    internal bool canJump = true;
    static Vector3 velocity;
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
		controller = GetComponent<Rigidbody>();
        
        anim.SetBool("Move", true);
        
        activePower = powers[powerCounter];
        powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject;


        anim.SetBool("Move", false);

	}

    public virtual void Update()
    {
        if (velocity.y < -0.1f) { Debug.Log(velocity); }

        //Makes player fall correctly when falling off a platform
        //Fix canJump bug where grounded but canJump != true
        if (timer > 0.5f)
        {
            if (Physics.Raycast(new Ray(transform.position, -transform.up), out hit, 1f))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {

                    if (hit.distance > 0.7f && !jumping && canJump)
                    {
                        canJump = false;
                        StopCoroutine("Jumping");

                        StartCoroutine("Falling");
                    }
                    if (hit.distance < 0.5f && !canJump)
                    {
                        canJump = true;
                        velocity.y = 0;
                    }
                }
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("LevelEnd"))
                {

                    if (hit.distance > 0.7f && !jumping && canJump)
                    {

                        canJump = false;
                        StopCoroutine("Jumping");

                        StartCoroutine("Falling");
                    }
                }
            }
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && canJump)
        {
            anim.SetBool("Move", false);
            StopCoroutine("Falling");
            StartCoroutine("Jump");
        }


    }
	public virtual void FixedUpdate () 
    {
        ChangeVelocity(0,velocity.y,velocity.z);
        
        //Set movement speed & direction
        if (Input.GetAxis("Horizontal")>0.1f || Input.GetAxis("Horizontal")< -0.1f)
        {
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                transform.LookAt(transform.position + Camera.main.transform.right);
                ChangeVelocity(velocity.x,velocity.y,1 * speed*1.2f);
            }
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                transform.LookAt(transform.position - Camera.main.transform.right);
                ChangeVelocity(velocity.x, velocity.y, -1 * speed*1.2f);
            }

        }
        else
        {
            ChangeVelocity(velocity.x, velocity.y, 0);

            anim.SetBool("Move", false);
        }

        //Enable / dissable running animation
        if (canJump)
        {
            anim.SetBool("Move", true);
            anim.SetFloat("Speed", velocity.z);
            velocity.y = 0;
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }

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


        controller.velocity = velocity;
	}

    public IEnumerator Falling()
    {
        if (falling) { yield return null; }

        if (!jumping && !canJump)
        {
            for (i = 0; i < 11; i++)
            {
                falling = true;
                if (i == 0)
                {
                    numberArray[i] = i;
                }

                if (i == 1)
                {
                    numberArray[i] = i;
                }
                if (i >= 2)
                {
                    numberArray[i] = numberArray[i - 1] + numberArray[i - 2];
                }
                if (i > 5)
                {
                    ChangeVelocity(velocity.x, -(numberArray[i] / 5), Input.GetAxis("Horizontal") * speed * 1.2f);
                    controller.velocity = velocity;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

            falling = false;
            yield return null;
    }

    public IEnumerator Jump()
    {
        if (jumping) { yield return null; }
            canJump = false;
        if (i > 1 && !falling) 
        {
            a = 89;
            b = 55;
            anim.SetTrigger("Jump");
            yield return new WaitForSeconds(0.05f); 
        }

        else { yield return null; }


        while (i>2 && !canJump)
        {
            jumping = true;
            for (i = a; i >2 ; i = i - c)
            {
                c = a - b;
                a = b;
                b = c;

                if (c > 4)
                {
                    ChangeVelocity(velocity.x, c / 5, Input.GetAxis("Horizontal") * speed);
                    controller.velocity = velocity;
                    yield return new WaitForSeconds(0.15f);
                }
                if (c < 4)
                {
                    ChangeVelocity(velocity.x, c / 5, Input.GetAxis("Horizontal") * speed);
                    controller.velocity = velocity;
                }
            }
        }
        jumping = false;
        StartCoroutine("Falling");
        yield return null;       
    }

    public void SummonBoss()
    {

    }

    public void SwitchPower()
    {
        powerCounter++;
        if (powerCounter >= powers.Length) { powerCounter = 0;}
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && !canJump)
        {
            canJump = true;
            Debug.Log("On the ground");
        }
    }

    public static void ChangeVelocity(float x, float y, float z)
    {
        velocity = new Vector3(x, y, z);
    }
}