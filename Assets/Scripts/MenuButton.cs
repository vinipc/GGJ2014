using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {
	public string scene;

	void OnMouseDown () {
		Application.LoadLevel(scene);
	}
}
