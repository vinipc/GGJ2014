using UnityEngine;
using System.Collections;

public class JumperPlayer : JumperController {
	public Collider2D footCollider;
	public static Vector3 position;

	Animator anim;

	protected void Start()
	{
		anim = GetComponent<Animator>();
	}

	protected override void Update ()
	{
		base.Update();
		position = transform.position;
		anim.SetFloat("speed", Mathf.Abs(Input.GetAxis("Horizontal")));
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
		position = transform.position = Vector3.one * 1000;

		Destroy (gameObject);
	}
	
}