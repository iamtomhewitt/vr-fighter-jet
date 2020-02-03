using UnityEngine;
using Utilities;

namespace Manager
{
	public class EnemyManager : MonoBehaviour
	{
		[SerializeField] private GameObject jet;

		[SerializeField] private int maxJets;
		[SerializeField] private float spawnDelay;
		[SerializeField] private float spawnRate;

		private int numberOfJets;

		private void Start()
		{
			InvokeRepeating("SpawnJet", spawnDelay, spawnRate);
			InvokeRepeating("CountNumberOfJets", 0f, 0.2f);
		}

		private void CountNumberOfJets()
		{
			numberOfJets = GameObject.FindGameObjectsWithTag(Tags.AI_JET).Length;
		}

		private void SpawnJet()
		{
			if (numberOfJets < maxJets)
			{
				GameObject j = Instantiate(jet, transform.position, Quaternion.identity) as GameObject;
				j.transform.parent = GameObject.Find(Tags.JETS).transform;
			}
		}
	}
}