using UnityEngine;
using System.Collections;

public class JumperPlayer : JumperController {
	public Collider2D footCollider;
	public static Vector3 position;

	protected override void Update ()
	{
		base.Update();
		position = transform.position;
	}

	protected override float GetHorizontalInput ()
	{
		return Input.GetAxis("Horizontal");
	}

	protected override bool GetJumpInput ()
	{
		return Input.GetButton ("Jump");
	}

	protected override bool GetJumpInputDown ()
	{
		return Input.GetButtonDown("Jump");
	}

	public override void GetHit()
	{
		gameObject.SetActive(false);
	}
	
}