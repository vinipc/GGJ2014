using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	void Update () {
		if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"))
			Application.LoadLevel("Menu");
	}
}
