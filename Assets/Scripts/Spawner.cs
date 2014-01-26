using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public float maxHeight;
	public float maxWidth;
	public float totalTime;

	public GameObject prefab;

	public static Spawner instance;

	// Use this for initialization
	void Start () {
		Spawner.instance = this;
	}

	public void ReturnObj(GameObject obj)
	{
		Vector3 aux = new Vector3(Random.Range(-maxWidth, maxWidth), Random.Range(0, maxHeight), 0);
		obj.transform.position = transform.position + aux;

		GameObject newObj = (GameObject)GameObject.Instantiate(prefab);
		aux = new Vector3(Random.Range(-maxWidth, maxWidth), Random.Range(0, maxHeight), 0);
		newObj.transform.position = transform.position + aux;
	}

	// Update is called once per frame
	void Update () {
		totalTime += Time.deltaTime;
	}
}
