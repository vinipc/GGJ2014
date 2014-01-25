using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public Vector3 offset;

	// Update is called once per frame
	void LateUpdate () {
		transform.position = target.position - offset;
	}
}
