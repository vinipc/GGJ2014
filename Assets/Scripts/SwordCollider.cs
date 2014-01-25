using UnityEngine;
using System.Collections;

public class SwordCollider : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		other.gameObject.GetComponent<SwordController>().GotHit();
		gameObject.collider2D.enabled = false;
	}
}
