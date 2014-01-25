using UnityEngine;
using System.Collections;

public class JumperNPC : JumperController {
	public float chasingDist, attackDist;

	protected override void Update()
	{

	}

	public void GetHit()
	{
		Debug.Log ("FUUUUUUUCK");
		gameObject.SetActive(false);
	}

	protected override bool GetJumpInput ()
	{
		return false;
	}

	protected override bool GetJumpInputDown ()
	{
		return false;
	}

	protected override float GetHorizontalInput ()
	{
		return 0;
	}
	
}