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

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if(grounded && !Physics2D.OverlapCircle(fallCheck[0].position, 0.04f, whatIsGround))
			move = -move;
	}

	bool SafeFall()
	{
		bool safeFall = true; 
		for(int i = 0; i < fallCheck.Length; i++)
		{
			safeFall = safeFall && Physics2D.OverlapCircle(fallCheck[i].position, 0.04f, whatIsGround);
		}

		return safeFall;
	}

	public override void GetHit()
	{
		print ("got hit");
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
		return ((jumpCheck.OverlapPoint(JumperPlayer.position) && Random.value < 0.08f)
		        || (kickdownCheck.OverlapPoint(JumperPlayer.position) && Random.value < 0.1f)
		        || (!grounded && rigidbody2D.velocity.y < 0f && !SafeFall()));
	}

	protected override float GetHorizontalInput ()
	{
		return move;
	}
	
}