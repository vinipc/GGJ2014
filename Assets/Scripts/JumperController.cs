using UnityEngine;
using System.Collections;

public abstract class JumperController : MonoBehaviour {

	public float maxSpeed = 10f;
	public float groundRadius = 0.02f;
	public float jumpForce = 650f;
	public float maxAirTime = 0.3f;
	public float jumpHoldFactor = 15f;
	public PhysicsMaterial2D headMaterial;

	bool facingLeft;

	public Transform groundCheck;
	public LayerMask whatIsGround;
	public bool grounded;
	public bool attacking;

	public Collider2D jumpAttack;

	protected Animator anim;

	public AudioClip jump;
	public AudioClip hit;
	public AudioClip kick;
	
	protected virtual void Start()
	{
		anim = GetComponent<Animator>();
	}

	protected virtual void Update()
	{
		anim.SetFloat("speed", Mathf.Abs(GetHorizontalInput()));

		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

		if(grounded && (Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround) as Collider2D).gameObject.tag == "movingPlatform")
			rigidbody2D.velocity += new Vector2(0, -5);

		jumpAttack.enabled = !grounded;
		if(grounded && attacking)
			attacking = false;

		anim.SetBool("grounded", grounded);
		if(!grounded)
			anim.SetBool("attacking", attacking);

		if(GetJumpInputDown())
		{
			if(grounded)
				StartCoroutine("Jump");
			else if (!attacking)
				Kickdown();
		}

		if(Input.GetKeyDown (KeyCode.R))
			Application.LoadLevel(Application.loadedLevel);
	}

	protected virtual void FixedUpdate () 
	{
		float move = GetHorizontalInput();

		if(!attacking)
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		if(move > 0 && facingLeft)
			Flip();
		else if (move < 0 && !facingLeft)
			Flip();
	}

	IEnumerator Jump()
	{
		if(audio)
			audio.PlayOneShot(jump);

		grounded = false;
		float airTime = 0f;
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.AddForce(new Vector2(0, jumpForce));

		do
		{
			rigidbody2D.AddForce(new Vector2(0, Time.deltaTime*50*jumpForce/jumpHoldFactor));
			airTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		while (GetJumpInput() && airTime < maxAirTime);

		rigidbody2D.AddForce(new Vector2(0, -jumpForce/2));
	}

	protected void Kickdown()
	{
		audio.PlayOneShot(kick);
		rigidbody2D.velocity = Vector2.zero;
		attacking = true;
		rigidbody2D.AddForce (new Vector2(0, -2f*jumpForce));
	}

	void Flip()
	{
		facingLeft = !facingLeft;
		Vector3 temp = transform.localScale;
		temp.x = - temp.x;
		transform.localScale = temp;
	}

	protected abstract bool GetJumpInput();
	protected abstract bool GetJumpInputDown();
	protected abstract float GetHorizontalInput();
	public abstract void GetHit();
}