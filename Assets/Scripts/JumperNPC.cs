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
		gameObject.SetActive(false);
	}

	protected override bool GetJumpInput ()
	{
		return (Random.value < 0.8f);
	}

	protected override bool GetJumpInputDown ()
	{
		return (jumpCheck.OverlapPoint(JumperPlayer.position)
		        || kickdownCheck.OverlapPoint(JumperPlayer.position));
	}

	protected override float GetHorizontalInput ()
	{
		return move;
	}
	
}