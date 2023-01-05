using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour {

	// public ProceduralMaterial substance;
    public Power[] powers;
    public Power activePower;
    public GameObject gun;
    internal GameObject powerHolder;

    internal int score = 0;
    internal int hp = 100;
    public int mp = 100;
    internal Animator anim;

    //item variables
    internal bool usingItem = false;

    //climbing variables
    internal bool climbing = false;
    internal GameObject ladder;

    //movement variables
    internal CharacterController controller;
    internal Vector3 startDepth;
    public float speed = 5.5f;
    internal Vector3 velocity = Vector3.zero;

    //jump variables
    internal bool canJump = true;
    internal bool jumping = false;
    Vector3 gravity = Vector3.zero;
    RaycastHit hit;
    int[] numberArray = new int[15];
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
            anim.SetFloat("Speed", 0);

                gravity.y = 3*Input.GetAxis("Vertical");

           if (Input.GetButtonDown("Jump") && Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.1f) { gravity = Vector3.zero; StartCoroutine("Jump");  }

        }
        else
        {
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

        if (jumping)
        {
            anim.SetFloat("Speed", 0);
        }


    }

	public virtual void FixedUpdate () 
    {
        canJump = controller.isGrounded;

        anim.SetBool("Move", canJump);

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

        // if (substance.GetProceduralFloat("Snow") >= 1)
        // {
        //     SummonBoss();
        // }
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
        anim.SetTrigger("Damaged");
        if (hp < 1) { StartCoroutine("Death"); }
        else { Camera.main.GetComponentInParent<LevelCamera>().RemoveHP(); velocity = Vector3.zero;}

    }

    public bool UseMP(int amountUsed)
    {
        if (amountUsed == 0) { return true;}
        if (mp < 0) { Debug.Log("NoMana"); return false; }

        mp -= amountUsed;
        if (mp < 0) { Debug.Log("NoMana"); return false; }
        Camera.main.GetComponentInParent<LevelCamera>().RemoveMP();
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

    public void UseItem(Item itemToUse)
    {
        if (usingItem) { return; }

        switch (itemToUse.itemType)
        {
            case ItemType.hp:
                usingItem = true;
                GainHP(itemToUse.gainAmount);
                break;
            case ItemType.mp:
                usingItem = true;
                GainMP(itemToUse.gainAmount);
                break;
            case ItemType.score:
                usingItem = true;
                GainScore(itemToUse.gainAmount);
                break;
        }
    }

    private void GainScore(int p)
    {
        if (!usingItem) { return; }
        Camera.main.GetComponentInParent<LevelCamera>().score += p;
        Camera.main.GetComponentInParent<LevelCamera>().scoreText.text = "" + score;

        usingItem = false;

    }

    private void GainMP(int p)
    {
        mp += p;
        if (mp > 100) { mp = 100; }
        if (mp < 1) { mp = 0; }
        Camera.main.GetComponentInParent<LevelCamera>().AddMP();
        usingItem = false;

    }

    private void GainHP(int p)
    {
        hp += p;
        if (hp > 100) { hp = 100; }
        if (hp < 1) { hp = 0; }
        Camera.main.GetComponentInParent<LevelCamera>().AddHP();
        usingItem = false;

    }
}