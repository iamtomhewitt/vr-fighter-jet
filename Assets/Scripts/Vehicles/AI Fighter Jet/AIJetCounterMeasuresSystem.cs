using System.Collections;
using UnityEngine;
using Vehicle;

namespace AIFighterJet
{
	public class AIJetCounterMeasuresSystem : CounterMeasuresSystem
	{
		[SerializeField] private LockOnGraphic lockOnGraphic;

		private void Start()
		{
			InvokeRepeating("FireCounterMeasures", Random.Range(10f, counterMeasureDeploySpeed), Random.Range(10f, counterMeasureDeploySpeed));
		}

		public override IEnumerator SpawnCounterMeasures()
		{
			canUseCounterMeasures = false;

			for (int i = 0; i <= amountOfCounterMeasures; i++)
			{
				GameObject c = Instantiate(counterMeasure, spawn.position, Quaternion.identity) as GameObject;
				Destroy(c, 6f);
				yield return new WaitForSeconds(counterMeasureDeploySpeed);
			}

			StartCoroutine(ReloadCounterMeasures());
		}

		public override IEnumerator ReloadCounterMeasures()
		{
			canUseCounterMeasures = true;
			yield return null;
		}
	}
}
