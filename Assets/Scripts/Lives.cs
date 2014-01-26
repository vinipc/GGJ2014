using UnityEngine;
using System.Collections;

public class Lives : MonoBehaviour {
	public Renderer[] lives;

	void Update()
	{
		for(int i = 0; i < lives.Length; i++)
		{
			lives[i].renderer.enabled = i < JumperPlayer.instance.lives;
		}
	}
}