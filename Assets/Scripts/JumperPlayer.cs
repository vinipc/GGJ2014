using UnityEngine;
using System.Collections;

public class JumperPlayer : JumperController {
	public static Vector3 position;
	public static bool staticGrounded;
	private int lives;

	protected override void Start ()
	{
		base.Start();
		lives = 3;
	}

	protected override void Update ()
	{
		base.Update();
		position = transform.position;
		staticGrounded = grounded;
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
		lives--;
		if(lives == 0)
		{
			position = transform.position = Vector3.one * 1000;
			Destroy (gameObject);
		}
	}	
}