using UnityEngine;
using System.Collections;
/// <summary>
/// Unsure if this works, but plays a random sound out of a possible four sounds
/// </summary>
public class TargetExplosion : MonoBehaviour 
{
	public AudioSource[] clips;
    public AudioSource clip;

	void Start()
	{
        clip = clips[Random.Range(0, 3)];
        clip.Play();
	}
}
