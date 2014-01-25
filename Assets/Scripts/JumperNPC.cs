using UnityEngine;
using System.Collections;

public class JumperNPC : JumperController {
	public Transform fallCheck;
	public Collider2D kickdownCheck;
	public Collider2D jumpCheck;

	private float move;

	protected void Start()
	{
		move = 1;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if(grounded && !Physics2D.OverlapCircle(fallCheck.position, 0.04f, whatIsGround))
			move = -move;
	}

	public override void GetHit()
	{
		Destroy(gameObject);
	}

	protected override bool GetJumpInput ()
	{
		return (Random.value < 0.9f);
	}

	protected override bool GetJumpInputDown ()
	{
		return ((jumpCheck.OverlapPoint(JumperPlayer.position) && Random.value < 0.08f)
		        || (kickdownCheck.OverlapPoint(JumperPlayer.position) && Random.value < 0.1f));
	}

	protected override float GetHorizontalInput ()
	{
		return move;
	}
	
}