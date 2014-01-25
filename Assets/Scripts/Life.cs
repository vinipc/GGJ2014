using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {
	public int hp;

	void Update (){
		if (SwordCharController.instance.hp < hp)
			renderer.enabled = false;
	}
}
