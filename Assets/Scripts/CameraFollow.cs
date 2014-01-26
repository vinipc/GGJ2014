using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public Vector3 offset;

	// Update is called once per frame
	void LateUpdate () {
		if(target)
			transform.position = target.position - offset;
	}
}
