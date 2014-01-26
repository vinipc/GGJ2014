using UnityEngine;
using System.Collections;

public class LevelManagerSword: MonoBehaviour {
	public Transform exitPortal;
	public Transform[] possiblePortals;

	void Start()
	{
		StartCoroutine("Initialize");
	}

	IEnumerator Initialize()
	{
		yield return new WaitForEndOfFrame();
		Transform entrance = possiblePortals[Random.Range(0, possiblePortals.Length)];
		SwordCharController.instance.transform.position = entrance.position;
		
		Transform exit;
		
		do
		{
			exit = possiblePortals[Random.Range (0, possiblePortals.Length)];
		} while (exit == entrance);
		
		exitPortal.position = exit.position;
	}
}
