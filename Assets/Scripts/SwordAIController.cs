using UnityEngine;
using System.Collections;

public class SwordAIController : SwordController {

	public float maxDistance = 1300f;
	public float minDistance = 100f;
	public float atkDistance = 2f;
	public Transform forwardCheck;
	float distance;

	protected override void Start ()
	{
		hp = 1;
		base.Start ();
	}

	protected override void Update()
	{
		base.Update();

		distance = (transform.position - SwordCharController.playerPos.position).sqrMagnitude;
		if(Input.GetButtonDown("Jump") && grounded && distance < minDistance)
		{
			StartCoroutine("Jump");
		}
	}

	protected override void FixedUpdate()
	{
		distance = (transform.position - SwordCharController.playerPos.position).sqrMagnitude;
		base.FixedUpdate();
	}
	
	protected override float HorizontalInputMethod ()
	{
		float move = 0;

		if (distance < atkDistance)
			move = 0;
		else if (distance < minDistance)
		{
			if (transform.position.x > SwordCharController.playerPos.position.x)
				move = -1;
			else
				move = 1;
		}
		else if(distance < maxDistance)
		{
			if (facingLeft)
				move = -1;
			else
				move = 1;

			if (!Physics2D.OverlapCircle(forwardCheck.position, groundRadius, whatIsGround))
				move *= -1;
		}

		return move;
	}
	
	protected override bool JumpInputMethod ()
	{
		if (distance < minDistance)
			return Input.GetButton("Jump");
		else
			return false;
	}
	
	protected override void Death ()
	{
		print ("So long fuckers");
	}
}