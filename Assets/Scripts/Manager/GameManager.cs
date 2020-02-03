using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
	[SerializeField] private FadeHelper fadeHelper;
	[SerializeField] private AudioManager audioManager;
	[SerializeField] private string[] parentTransforms;

	private void Awake()
	{
		if (FindObjectOfType<FadeHelper>() == null)
		{
			Instantiate(fadeHelper);
		}

		if (FindObjectOfType<AudioManager>() == null)
		{
			Instantiate(audioManager);
		}

		foreach (string t in parentTransforms)
		{
			if (GameObject.Find(t) == null)
			{
				new GameObject(t);
			}
		}
	}
}