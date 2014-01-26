using UnityEngine;
using System.Collections;

public class MovingPlataform : MonoBehaviour {
	public Vector3 startPosition;
	public Vector3 finishPosition;
	public float moveTime = 10f;
	
	void Start () {
		transform.position = startPosition;
		iTween.MoveTo(gameObject, iTween.Hash("position", finishPosition, "time", moveTime, "easetype", "linear", "looptype", "pingPong"));
	}

	[ContextMenu("Set Start Position")]
	private void SetStartPosition()
	{
		startPosition = transform.position;
	}

	[ContextMenu("Set Finish Position")]
	private void SetFinishPosition()
	{
		finishPosition = transform.position;
	}
}
