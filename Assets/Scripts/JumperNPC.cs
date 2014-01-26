using UnityEngine;
using System.Collections;

public class JumperNPC : JumperController {
	public Transform[] fallCheck;
	public Collider2D kickdownCheck;
	public Collider2D jumpCheck;

	private float move;

	protected override void Start()
	{
		base.Start();
		move = 1;
	}

	protected override void Update()
	{
		base.Update();
		jumpCheck.enabled = grounded;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if(grounded && !Physics2D.OverlapCircle(fallCheck[0].position, 0.04f, whatIsGround))
			move = -move;

		if(Mathf.Abs(JumperPlayer.position.y - transform.position.y) > 0.2f 
		   || Mathf.Abs(JumperPlayer.position.x - transform.position.x) > 25f)
			return;

		if(JumperPlayer.staticGrounded && this.grounded)
		{
			if(JumperPlayer.position.x < transform.position.x)
				move = -1;
			else
				move = 1;
		}
	}

	bool SafeFall()
	{ 
		for(int i = 1; i < fallCheck.Length; i++)
		{
			if(Physics2D.OverlapCircle(fallCheck[i].position, 0.01f, whatIsGround))
				return true;
		}
		return false;
	}

	public override void GetHit()
	{
		rigidbody2D.velocity = Vector2.zero;
		move = 0;
		anim.SetTrigger("death");
		Destroy(gameObject, 0.3f);
	}

	protected override bool GetJumpInput ()
	{
		return true;
	}

	protected override bool GetJumpInputDown ()
	{
		if(jumpCheck.OverlapPoint(JumperPlayer.position) && Random.value < 0.8f)
		{
			jumpCheck.enabled = false;
			return true;
		}
		if(kickdownCheck.OverlapPoint(JumperPlayer.position) && Random.value < 0.08f)
		{
			return true;
		}
		if(!grounded && rigidbody2D.velocity.y < 0f && !SafeFall())
		{
			return true;
		}
		return false;

	}

	protected override float GetHorizontalInput ()
	{
		return move;
	}
	
}