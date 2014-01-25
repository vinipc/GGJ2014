using UnityEngine;
using System.Collections;

public abstract class SwordController : MonoBehaviour {

	public float maxSpeed = 10f;
	public float groundRadius = 0.04f;
	public float jumpForce = 550f;
	public float maxAirTime = 0.3f;
	public float jumpHoldFactor = 15f;

	protected bool facingLeft;

	public Transform groundCheck;
	public LayerMask whatIsGround;
	protected bool grounded;

	Collider2D sword;

	protected bool attacking;
	float atkTime;
	public float atkStart;
	public float atkStop;

	protected bool defending;
	float bashTime;

	protected int hp;

	Animator anim;

	protected virtual void Start()
	{
		anim = GetComponent<Animator>();
		sword = transform.FindChild("Sword").collider2D;
	}

	protected virtual void Update ()
	{
		if (attacking)
		{
			atkTime += Time.deltaTime;
			if (atkTime > atkStop)
			{
				sword.enabled = false;
				attacking = false;
			}
			else if (atkTime > atkStart && !sword.enabled)
				sword.enabled = true;
		}
	}


	protected abstract float HorizontalInputMethod ();

	protected virtual void FixedUpdate () 
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

		float move = HorizontalInputMethod();

		anim.SetFloat("XSpd", Mathf.Abs(move));

		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		if(move > 0 && facingLeft)
			Flip();
		else if (move < 0 && !facingLeft)
			Flip();
	}

	protected abstract bool JumpInputMethod ();

	IEnumerator Jump()
	{
		float airTime = 0f;
		rigidbody2D.AddForce(new Vector2(0, jumpForce));

		do
		{
			rigidbody2D.AddForce(new Vector2(0, Time.deltaTime*50*jumpForce/jumpHoldFactor));
			airTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		while (JumpInputMethod() && airTime < maxAirTime);

		rigidbody2D.AddForce(new Vector2(0, -jumpForce/2));
	}
	
	void Flip()
	{
		facingLeft = !facingLeft;
		Vector3 temp = transform.localScale;
		temp.x = - temp.x;
		transform.localScale = temp;
	}

	protected void Atk ()
	{
		anim.SetTrigger("Atk");
		attacking = true;
		atkTime = 0;
	}

	protected abstract void Death ();

	public void GotHit()
	{
		if (defending)
			bashTime = 0.3f;
		else
		{
			hp --;
			if (hp <= 0)
				Death ();
		}
	}
}