using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour {

    public LayerMask ignoreLayers;
	public ProceduralMaterial substance;
    public Power[] powers;
    public Power activePower;
	public int height = 4;
    internal GameObject powerHolder;

    internal bool canJump = true;
    internal Animator anim;
    internal Rigidbody controller;
    internal Vector3 startDepth;
    float tempValue;
    int powerCounter = 0;
	int timer;
    RaycastHit rayhit;

	public virtual void Start () {
        startDepth = transform.position;
        anim = GetComponent<Animator>();
		controller = GetComponent<Rigidbody>();
        
        activePower = powers[powerCounter];
        powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject;
	}
	
	public virtual void FixedUpdate () 
    {
        controller.AddForce(transform.forward * 2 * 50);
        anim.SetFloat("Speed",1f);

        #region Fixes
        if (transform.position.x > startDepth.x)
        {
            transform.position = new Vector3(startDepth.x,transform.position.y,transform.position.z);
        }
        if (transform.rotation.y > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        if (transform.position.y > 2)
        {
            transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        }
        #endregion

        if (Input.GetButtonUp("Jump") && canJump)
        {
            anim.SetBool("Move", false);
            Jump();
            //StartCoroutine("JumpCheck");
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
    public void Jump()
    {
        canJump = false;
        anim.SetTrigger("Jump");
        controller.AddForce(transform.up * Time.deltaTime * height*1000,ForceMode.Impulse);
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
            anim.SetBool("Move", true);
            canJump = true;
            Debug.Log("On the ground");
        }
    }
}