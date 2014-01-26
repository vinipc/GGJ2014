using UnityEngine;
using System.Collections;

public class backMusic : MonoBehaviour {
	static bool started = false;


	// Use this for initialization
	void Start () {
		if (backMusic.started)
			Destroy(gameObject);
		else
		{
			backMusic.started = true;
			DontDestroyOnLoad(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
