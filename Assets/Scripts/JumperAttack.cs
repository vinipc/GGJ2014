using UnityEngine;
using System.Collections;

public class JumperAttack : MonoBehaviour {

	public LayerMask otherJumperLayer;

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		print (otherCollider);
		JumperController otherJumper = otherCollider.GetComponent<JumperController>();
		otherJumper.GetHit();
	}

}
