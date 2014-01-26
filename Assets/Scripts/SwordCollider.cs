using UnityEngine;
using System.Collections;

public class SwordCollider : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		gameObject.collider2D.enabled = false;
		
		SwordController obj;
		SwordController aux;
		if (other.gameObject.name == "Shield")
			obj = other.transform.parent.gameObject.GetComponent<SwordController>();
		else
			obj = other.gameObject.GetComponent<SwordController>();


		aux = transform.parent.GetComponent<SwordController>();
		if (obj.GotHit(aux.facingLeft))
			aux.GotBashed();
	}
}
