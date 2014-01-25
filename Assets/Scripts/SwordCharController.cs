using UnityEngine;
using System.Collections;

public class SwordCharController : SwordController {
	public static Transform playerPos;

	void Awake ()
	{
		SwordCharController.playerPos = transform;
	}

	void Update()
	{
		if(Input.GetButtonDown("Jump") && grounded)
		{
			StartCoroutine("Jump");
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
}