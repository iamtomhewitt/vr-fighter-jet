using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
	public FadeHelper fadeHelper;
	public AudioManager audioManager;

	[Space()]
	public string[] transforms;

	private void Awake()
	{
		if (GameObject.FindObjectOfType<FadeHelper>() == null)
			Instantiate(fadeHelper);

		if (GameObject.FindObjectOfType<AudioManager>() == null)
			Instantiate(audioManager);

		foreach (string t in transforms)
		{
			if (GameObject.Find(t) == null)
				new GameObject(t);
		}
	}
}
