using UnityEngine;
using System.Collections;

public abstract class SwordController : MonoBehaviour {

	public float maxSpeed = 10f;
	public float groundRadius = 0.04f;
	public float jumpForce = 550f;
	public float maxAirTime = 0.3f;
	public float jumpHoldFactor = 15f;

	public bool facingLeft;

	public Transform groundCheck;
	public LayerMask whatIsGround;
	public bool grounded;

	Collider2D sword;

	public bool attacking;
	public bool atkSet;
	public float atkTime;
	public float atkStart;
	public float atkStop;

	public bool defending;
	public float bashTime;
	public bool bashing;

	public bool disabled;
	public float disableTime;

	public int hp;
	public bool gotHit;

	public bool jumping;

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
			else if (atkTime > atkStart && !sword.enabled && !atkSet)
			{
				sword.enabled = true;
				atkSet = true;
			}
		}
		else if (bashing)
		{
			bashTime -= Time.deltaTime;
			if (bashTime < 0)
				bashing = false;
		}
		else if (disabled)
		{
			disableTime -= Time.deltaTime;
			if (disableTime <= 0)
			{
				disabled = false;
				anim.SetBool("Disabled", false);
			}
		}
	}

	protected abstract float HorizontalInputMethod ();

	protected virtual void FixedUpdate () 
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("Grounded", grounded);
		float move;
		if (attacking || disabled || defending || bashing)
			move = 0;
		else
			move = HorizontalInputMethod();

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
		if (attacking || disabled || bashing || !grounded || jumping)
			yield break;
		else if (defending)
		{
			defending = false;
			anim.SetBool("Def", false);
		}

		jumping = true;
		float airTime = 0f;
		rigidbody2D.AddForce(new Vector2(0, jumpForce));

		do
		{
			rigidbody2D.AddForce(new Vector2(0, Time.deltaTime*50*jumpForce/jumpHoldFactor));
			airTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		while (JumpInputMethod() && airTime < maxAirTime);

		jumping = false;
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
		if (disabled || !grounded || bashing || attacking)
			return;
		else if (defending)
		{
			defending = false;
			anim.SetBool("Def", false);
		}

		anim.SetTrigger("Atk");
		attacking = true;
		atkSet = false;
		atkTime = 0;
	}

	protected void Defend ()
	{
		if (attacking || !grounded || bashing || disabled)
			return;

		defending = true;
		anim.SetBool("Def", true);
	}

	protected void Bash ()
	{
		if (!defending)
			return;
		
		anim.SetTrigger("Bash");
		anim.SetBool("Def", false);
		defending = false;
		bashing = true;
		bashTime = 0.5f;
	}

	protected abstract void Death ();

	protected virtual void Bashed ()
	{
	}

	public bool GotHit()
	{
		if (defending)
			return false;
		else if (bashTime > 0)
		{
			Bashed ();
			return true;
		}
		else
		{
			hp --;
			if (hp <= 0)
				Death ();
			disabled = false;
			disableTime = 0;
			anim.SetBool("Disabled", false);
			gotHit = true;
			return false;
		}
	}

	public void GotBashed ()
	{
		disabled = true;
		disableTime = 2.4f;
		anim.SetBool("Disabled", true);
		attacking = false;
	}
}