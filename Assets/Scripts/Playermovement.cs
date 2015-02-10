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
    int a = 89, b = 55, c = 0, i = 0;
    Vector3 groundedHeight;
    Vector3 velocity;
    bool jumping = false;
    float timer;
    
    float tempValue;
    int powerCounter = 0;
    RaycastHit rayhit;

	public virtual void Start () 
    {
        groundedHeight = transform.position;
        startDepth = transform.position;
        anim = GetComponent<Animator>();
		controller = GetComponent<Rigidbody>();
        
        anim.SetBool("Move", true);
        
        activePower = powers[powerCounter];
        powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject;
	}
	
	public virtual void FixedUpdate () 
    {
        velocity.x = 0;

        if (Input.GetAxis("Horizontal")>0.1f|| Input.GetAxis("Horizontal")< -0.1f)
        {
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                transform.LookAt(transform.position + Camera.main.transform.right);
                velocity.z = 1 * speed;
                controller.velocity = velocity;
            }
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                transform.LookAt(transform.position - Camera.main.transform.right);
                velocity.z = -1* speed;
                controller.velocity = velocity;
            }

        }
        else
        {
            velocity.z = 0;
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
        if (i<=1 && !canJump)
        {
            StopAllCoroutines();
            StartCoroutine("Falling");
           //velocity = controller.velocity;
           //velocity.y = -5;
           //controller.velocity = velocity;
        }

        #endregion

        if (Input.GetButton("Jump") && canJump)
        {
            anim.SetBool("Move", false);
            StopAllCoroutines();
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

            //Input.GetTouch(0).position.x > Screen.width / 2 ||
            if ( Input.GetMouseButton(0))
            {
                tempValue = tempValue * 1.1f;
            }
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

	}

    public IEnumerator Falling()
    {
        for (i = 0; i < 10; i++)
        {
            if (i == 0) 
            {
                numberArray[i] = i; 
                Debug.Log(numberArray[i] + " iteration: " + i);
            }

            if (i == 1) 
            {
                
                numberArray[i] = i; 
                Debug.Log(numberArray[i] + " iteration: " + i);

            }
            if (i == 2)
            {
                numberArray[i] = numberArray[i - 1] + numberArray[i - 2];
                Debug.Log(numberArray[i] + " iteration: " + i);
            }
            if (i > 2)
            {
                numberArray[i] = numberArray[i - 1] + numberArray[i - 2];

                Debug.Log(numberArray[i] + " iteration: " + i);
                velocity.z = Input.GetAxis("Horizontal") * speed * 1.2f;
                velocity.y = -(numberArray[i] / 2);
                controller.velocity = velocity;
                yield return new WaitForSeconds(0.15f);
            }

        }

        yield return null;
    }

    public IEnumerator Jump()
    {
        canJump = false;
        anim.SetTrigger("Jump");
        a = 89;
        b = 55;

        yield return new WaitForSeconds(0.1f);

        //if (Input.GetButtonUp("Jump")) { jumping = false; yield return null; }


        while (Input.GetButton("Jump") && i>2)
        {
            jumping = true;
            for (i = a; i > 2; i = i - c)
            {
                c = a - b;
                a = b;
                b = c;

                velocity.z = Input.GetAxis("Horizontal") * speed * 1.2f;
                velocity.y = c/2;
                controller.velocity = velocity;
                yield return new WaitForSeconds(0.1f);
            }
        }


        jumping = false;
        Debug.Log("Back on the ground");
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
            velocity.y = 0;
            groundedHeight = transform.position;
            canJump = true;
            Debug.Log("On the ground");
        }
    }
}