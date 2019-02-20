using UnityEngine;
using System.Collections;

namespace AIFighterJet
{
	public class AIJetSpawner : MonoBehaviour
	{
		public GameObject jet;

		public int maxJets;

		public float spawnDelay;
		public float spawnRate;

		private int numberOfJets;

		void Start()
		{
			InvokeRepeating("SpawnJet", spawnDelay, spawnRate);
			InvokeRepeating("CountNumberOfJets", 0f, 0.2f);
		}

		void CountNumberOfJets()
		{
			numberOfJets = GameObject.FindGameObjectsWithTag("AI Jet").Length;
		}

		void SpawnJet()
		{
			if (numberOfJets < maxJets)
			{
				GameObject j = Instantiate(jet, transform.position, Quaternion.identity) as GameObject;
				j.transform.parent = GameObject.Find("Jets").transform;
			}
		}
	}
}
