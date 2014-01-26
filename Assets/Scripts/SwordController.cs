using UnityEngine;
using System.Collections;

public abstract class SwordController : MonoBehaviour {
	
	public AudioClip jumpA;
	public AudioClip fallA;
	public AudioClip hitA;
	public AudioClip getHitA;
	public AudioClip dieA;
	public AudioClip bashA;
	public AudioClip blockA;
	public AudioSource player;
	public AudioSource looper;
		
	public float maxSpeed = 10f;
	public float groundRadius = 0.04f;
	public float jumpForce = 550f;
	public float maxAirTime = 0.3f;
	public float jumpHoldFactor = 15f;

	public bool facingLeft;
	
	public Transform wallCheck;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public bool grounded;
	
	public Collider2D top;
	Collider2D sword;
	public Transform center;

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
	public float hitTime;

	public bool jumping;
	public bool dying;
	public float deathTime;

	Animator anim;

	protected virtual void Start()
	{
		anim = GetComponent<Animator>();
		sword = transform.FindChild("Sword").collider2D;
		center = transform.FindChild("Center").transform;
	}

	protected virtual void Update ()
	{
		if (dying)
		{
			deathTime -= Time.deltaTime;
			if (deathTime <= 0)
				Death ();
			return;
		}

		if (gotHit)
		{
			hitTime -= Time.deltaTime;
			if ((hitTime % 1) > 0.8)
				renderer.enabled = false;
			else
				renderer.enabled = true;

			if (hitTime < 0)
			{
				renderer.enabled = true;
				gotHit = false;
			}
		}

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
				player.clip = hitA;
				player.Play();
				sword.enabled = true;
				atkSet = true;
			}
		}
		else if (bashing)
		{
			bashTime -= Time.deltaTime;
			if (bashTime < 0)
			{
				anim.SetBool("Bash", false);
				bashing = false;
			}
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
		if (dying)
			return;

		bool aux = grounded;
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		if (grounded && !aux)
		{
			player.clip = fallA;
			player.Play();
		}

		if (grounded && (Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround) as Collider2D).gameObject.tag == "movingPlatform")
			rigidbody2D.velocity = new Vector2(0,-5);

		anim.SetBool("Grounded", grounded);
		float move;
		if (attacking || disabled || bashing)
			move = 0;
		else
			move = HorizontalInputMethod();

		if (Physics2D.OverlapCircle(wallCheck.position, groundRadius, whatIsGround))
		{
			if ((move < 0 && facingLeft) || (move > 0 && !facingLeft))
				move = 0;
		}

		if(move > 0 && facingLeft)
			Flip();
		else if (move < 0 && !facingLeft)
			Flip();

		if (defending)
			move = 0;

		if (grounded && move != 0)
		{
			if (!looper.isPlaying)
				looper.Play();
		}
		else if (looper.isPlaying)
			looper.Stop();

		anim.SetFloat("XSpd", Mathf.Abs(move));

		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
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

		player.clip = jumpA;
		player.Play();
		jumping = true;
		float airTime = 0f;
		rigidbody2D.velocity = Vector2.zero;
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
	
	protected void Flip()
	{
		facingLeft = !facingLeft;
		Vector3 aux = center.position;
		Vector3 temp = transform.localScale;
		temp.x = - temp.x;
		transform.localScale = temp;

		aux = aux - center.position;
		transform.position += aux;
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
		
		anim.SetBool("Bash", true);
		anim.SetBool("Def", false);
		defending = false;
		bashing = true;
		bashTime = 0.5f;
	}

	void StartDying ()
	{
		top.enabled = false;
		collider2D.enabled = false;
		transform.FindChild("Shield").collider2D.enabled = false;
		rigidbody2D.gravityScale = 0;
		transform.FindChild("GroundCheck").collider2D.enabled = false;
		renderer.enabled = true;

		anim.SetTrigger("Death");
		dying = true;
		player.clip = dieA;
		player.Play();
	}

	protected abstract void Death ();

	protected virtual void Bashed ()
	{
	}

	public bool GotHit(bool faceLeft)
	{
		if (defending && faceLeft != facingLeft)
		{
			player.clip = blockA;
			player.Play();
			return false;
		}
		else if (bashTime > 0)
		{
			player.clip = bashA;
			player.Play();
			Bashed ();
			return true;
		}
		else
		{
			hp --;
			if (hp <= 0)
				StartDying ();
			else
			{
				player.clip = getHitA;
				player.Play();
			}
			disabled = false;
			disableTime = 0;
			anim.SetBool("Disabled", false);
			gotHit = true;
			hitTime = 3f;
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