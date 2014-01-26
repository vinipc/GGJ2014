using UnityEngine;
using System.Collections;

public class SwordAIController : SwordController {

	public float maxDistance = 1300f;
	public float minDistance = 100f;
	public float atkDistance = 2f;
	public Transform forwardCheck;
	float distance;

	public float defTime;
	public float atkChance;
	public bool mustAtk;
	public float defendChance;

	protected override void Start ()
	{
		hp = 1;
		base.Start ();
	}

	void LateUpdate ()
	{
		distance = (transform.position - SwordCharController.instance.transform.position).sqrMagnitude;
		if(SwordCharController.instance.grounded && Input.GetButtonDown("Jump") && SwordCharController.instance.jumping && distance < minDistance)
		{
			StartCoroutine("Jump");
		}

		if (defending)
		{
			defTime -= Time.deltaTime;
			if (defTime <= 0)
				Bash();
		}
		if (distance < atkDistance && !attacking && grounded && !disabled && !defending && !bashing)
		{
			float rand = Random.value;
			if (rand < atkChance || mustAtk)
			{
				mustAtk = false;
				Atk();
			}
			else if (rand < defendChance)
			{
				Defend();
				defTime = Random.value * 5 + 1;
			}
		}
	}

	protected override void Bashed ()
	{
		mustAtk = true;
	}

	protected override void FixedUpdate()
	{
		distance = (transform.position - SwordCharController.instance.transform.position).sqrMagnitude;
		base.FixedUpdate();
	}

	protected override float HorizontalInputMethod ()
	{
		float move = 0;

		if (distance < atkDistance)
		{
			if (SwordCharController.instance.grounded)
			{
				if (transform.position.x > SwordCharController.instance.transform.position.x)
				{
					if (!facingLeft)
						Flip();
				}
				else
				{
					if (facingLeft)
						Flip();
				}
			}
		}
		else if (distance < minDistance)
		{
			if (transform.position.x > SwordCharController.instance.transform.position.x)
				move = -1;
			else
				move = 1;

			if ((move == -1 && facingLeft) || (move == 1 && !facingLeft))
				if (!Physics2D.OverlapCircle(forwardCheck.position, groundRadius, whatIsGround))
					move = 0;
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
		Destroy(gameObject);
	}
}