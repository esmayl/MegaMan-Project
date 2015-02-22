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
    public int mp = 100;
    internal Animator anim;

    //climbing variables
    internal bool climbing = false;

    //movement variables
    internal CharacterController controller;
    internal Vector3 startDepth;
    public float speed = 5.5f;

    //jump variables
    internal bool canJump = true;
    internal bool jumping = false;
    Vector3 gravity = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    RaycastHit hit;
    int[] numberArray = new int[15];
    int a = 89, b = 55, c = 0, i = 1;
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

        if (climbing)
        {
            if (Input.GetAxis("Vertical")>0.1f)
            {
                gravity.y = 3;
            }
            else
            {
                gravity = Vector3.zero;
            }

        }
        else
        {
            canJump = controller.isGrounded;
        }

        //Fire power
        if (Input.GetMouseButtonUp(0))
        {
            if (UseMP(activePower.mpCost))
            {
                Debug.Log(activePower.mpCost + " mp - " + mp);
                activePower.Attack(transform);
            }
            else
            {
                Debug.Log("No mana to use skill");
            }
        }

        //Switch to next power
        if (Input.GetMouseButtonDown(1))
        {
            SwitchPower();
        }

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

        velocity = new Vector3(0, 0, Mathf.Abs(Input.GetAxis("Horizontal") * speed));
        velocity.Normalize();
        velocity = transform.TransformDirection(velocity);

        velocity *= speed;

        if (jumping)
        {
            if (timer > 0.9f)
            {
                timer = 0;
                jumping = false;
            }
            timer += Time.deltaTime * 4;
        }

        if (Input.GetButtonDown("Jump") && canJump && !jumping)
        {
            StartCoroutine("Jump");
            timer = 0.06f;
        }

        if (Input.GetButton("Jump") && canJump && !jumping)
        {
            StartCoroutine("Jump");
        }


        if (Input.GetButtonUp("Jump"))
        {

            timer = 0;
            jumping = false;
        }

        if (!canJump)
        {
            if (!jumping && !climbing)
            {
                gravity += Physics.gravity * Time.deltaTime * 3;
            }
        }
        else if (jumping)
        {
            if (timer < 0.1f) { gravity.y = 0.04f * 180f; }
            else
            {
                gravity.y = timer * 180f;
                if (gravity.y > 3f) { gravity.y = 3f; }
            }
        }
        else
        {
            gravity = Vector3.zero;
        }
        velocity += gravity;
        velocity.x = 0;
        controller.Move(velocity * Time.deltaTime);

        if (substance.GetProceduralFloat("Snow") >= 1)
        {
            SummonBoss();
        }
	}

    public void Jump()
    {
        anim.SetTrigger("Jump");
        anim.SetBool("Move", false);

        jumping = true;
    }
    
    public IEnumerator Death()
    {
        //spawn particle system
        //emit 10 or so ominidirectional with huge particles
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0;
        Debug.Log(gameObject.name);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Application.LoadLevel(Application.loadedLevel);
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp < 1) { StartCoroutine("Death"); }

    }

    public bool UseMP(int amountUsed)
    {
        if (amountUsed == 0) { return true;}
        if (mp < 0) { Debug.Log("NoMana"); return false; }

        mp -= amountUsed;
        if (mp < 0) { Debug.Log("NoMana"); return false; }       
        return true;
    }
    
    public void SummonBoss()
    {

    }

    public void SwitchPower()
    {
        powerCounter++;
        if (powerCounter >= powers.Length) { powerCounter = 0;}
        activePower = powers[powerCounter];
       
        //Set Power

        if (powerHolder.name != activePower.name)
        {
            Destroy(powerHolder);
            powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject;
            powerHolder.GetComponent<Power>().gun = gun;
        }
    }
}