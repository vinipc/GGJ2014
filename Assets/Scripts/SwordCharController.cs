using UnityEngine;
using System.Collections;

public class SwordCharController : SwordController {
	public static SwordCharController instance;

	void Awake ()
	{
		SwordCharController.instance = this;
	}

	protected override void Start ()
	{
		hp = 3;
		base.Start ();
	}

	protected override void Update()
	{
		base.Update();

		if(Input.GetButtonDown("Jump"))
		{
			StartCoroutine("Jump");
		}

		else if (Input.GetButtonDown("Fire1"))
		{
			Atk();
		}
		else if (Input.GetButton("Fire2") && !defending)
		{
			Defend();
		}
		else if (Input.GetButtonUp("Fire2"))
		{
			Bash();
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
	}
}