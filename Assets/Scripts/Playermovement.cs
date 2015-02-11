using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour {

    public LayerMask ignoreLayers;
	public ProceduralMaterial substance;
    public Power[] powers;
    public Power activePower;
    internal GameObject powerHolder;

    internal int hp = 100;
    internal bool canJump = true;
    internal Animator anim;
    internal Rigidbody controller;
    internal Vector3 startDepth;
    float speed = 3;

    //jump variables
    int[] numberArray = new int[15];
    int a = 89, b = 55, c = 0, i = 1;
    Vector3 groundedHeight;
    static Vector3 velocity;
    bool jumping = false;
    bool falling = false;
    float temp;
    float timer;
    
    float tempValue;
    int powerCounter = 0;
    RaycastHit rayhit;

	public virtual void Start () 
    {
        startDepth = transform.position;
        anim = GetComponent<Animator>();
		controller = GetComponent<Rigidbody>();
        
        anim.SetBool("Move", true);
        
        activePower = powers[powerCounter];
        powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject;
	}
	
	public virtual void FixedUpdate () 
    {
        ChangeVelocity(0,velocity.y,velocity.z);
        temp = Mathf.Abs(transform.position.y - groundedHeight.y);
        

        if (Input.GetAxis("Horizontal")>0.1f || Input.GetAxis("Horizontal")< -0.1f)
        {
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                transform.LookAt(transform.position + Camera.main.transform.right);
                ChangeVelocity(velocity.x,velocity.y,1 * speed);
            }
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                transform.LookAt(transform.position - Camera.main.transform.right);
                ChangeVelocity(velocity.x, velocity.y, -1 * speed);
            }

        }
        else
        {
            ChangeVelocity(velocity.x, velocity.y, 0);

            anim.SetBool("Move", false);
        }
        if (canJump)
        {
            anim.SetBool("Move", true);
            anim.SetFloat("Speed", velocity.z);
        }

        #region Fixes
        if (transform.position.x > startDepth.x)
        {
            transform.position = new Vector3(startDepth.x,transform.position.y,transform.position.z);
        }

        #endregion

        if (Input.GetButton("Jump") && canJump)
        {
            anim.SetBool("Move", false);
            StartCoroutine("Jump");
        }

        if (Input.touchCount >= 1 || Input.GetMouseButtonDown(0))
        {
            activePower = powers[powerCounter];
            if (powerHolder.name != activePower.name)
            {
                Destroy(powerHolder);
                powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject;
            }

            tempValue = activePower.value;
        }

        //Input.touchCount <= 0 || 
        if (Input.GetMouseButtonUp(0))
		{
            activePower.value = tempValue;
            activePower.Attack(transform);

            tempValue = 0.1f;
            activePower.value = tempValue;
		}
        
        if (Input.GetMouseButtonDown(1))
        {
            SwitchPower();
        }

        if (tempValue >= 2) { tempValue = 2f; }

        if (substance.GetProceduralFloat("Snow") >= 1)
        {
            SummonBoss();
        }

        if (temp < 0.5f && !falling )
        {
            StartCoroutine("Falling");
            //Debug.Log(temp);
        }

        controller.velocity = velocity;
	}

    public IEnumerator Falling()
    {
        if (falling) { yield return null; }
        
            for (i = 0; i < 10; i++)
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
                if (i > 3)
                {
                    numberArray[i] = numberArray[i - 1] + numberArray[i - 2];

                    ChangeVelocity(velocity.x, -(numberArray[i] / 3), Input.GetAxis("Horizontal") * speed * 1.2f);
                    controller.velocity = velocity;
                    yield return new WaitForSeconds(0.1f);
                }
            }

            yield return null;
    }

    public IEnumerator Jump()
    {
        if (jumping) { yield return null; }
        canJump = false;
        anim.SetTrigger("Jump");
        a = 89;
        b = 55;

        yield return new WaitForSeconds(0.1f);

        while (Input.GetButton("Jump") && i>3 && !canJump)
        {
            jumping = true;
            for (i = a; i >3; i = i - c)
            {
                c = a - b;
                a = b;
                b = c;

                ChangeVelocity(velocity.x, c / 4, Input.GetAxis("Horizontal") * speed);
                controller.velocity = velocity;
                yield return new WaitForSeconds(0.3f);
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
            StopCoroutine("Falling");
            velocity.y = 0;
            falling = false;
            jumping = false;
            groundedHeight = transform.position;
            canJump = true;
            Debug.Log("On the ground");
        }
    }

    public static void ChangeVelocity(float x, float y, float z)
    {
        velocity = new Vector3(x, y, z);
    }
}