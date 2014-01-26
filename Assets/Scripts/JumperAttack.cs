using UnityEngine;
using System.Collections;

public class JumperAttack : MonoBehaviour {

	public LayerMask otherJumperLayer;

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		JumperController otherJumper = otherCollider.GetComponent<JumperController>();
		otherJumper.GetHit();
		transform.parent.GetComponent<JumperController>().attacking = false;
	}
}