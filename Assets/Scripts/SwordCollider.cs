using UnityEngine;
using System.Collections;

public class SwordCollider : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		gameObject.collider2D.enabled = false;
		if (other.gameObject.layer == 8 || other.gameObject.layer == 10)
			other.gameObject.GetComponent<SwordController>().GotHit();
		// gameObject.transform.parent.GetComponent<SwordController>().attacking = false;
	}
}
