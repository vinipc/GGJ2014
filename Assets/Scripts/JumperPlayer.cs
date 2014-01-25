using UnityEngine;
using System.Collections;

public class JumperPlayer : JumperController {
	public Collider2D footCollider;

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

	void OnCollisionEnter2D(Collision2D collision)
	{
		JumperNPC enemy = collision.collider.GetComponent<JumperNPC>();
		if(enemy == null)
			return;

		if(transform.position.y > collision.collider.transform.position.y + 0.5f)
		{
				enemy.GetHit();
		}
	}
}