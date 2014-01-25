using UnityEngine;
using System.Collections;

public class SwordCharController : SwordController {
	public static Transform playerPos;

	void Awake ()
	{
		SwordCharController.playerPos = transform;
	}

	protected override void Start ()
	{
		hp = 3;
		base.Start ();
	}

	protected override void Update()
	{
		base.Update();

		if(Input.GetButtonDown("Jump") && grounded)
		{
			StartCoroutine("Jump");
		}

		else if (Input.GetButtonDown("Fire1"))
		{
			Atk();
		}
	}
	
	protected override float HorizontalInputMethod ()
	{
		return Input.GetAxis("Horizontal");
	}
	
	protected override bool JumpInputMethod ()
	{
		return Input.GetButton("Jump");
	}

	protected override void Death ()
	{
		throw new System.NotImplementedException ();
	}
}