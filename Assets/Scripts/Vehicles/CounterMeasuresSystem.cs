using UnityEngine;
using System.Collections;

namespace Vehicle
{
	/// <summary>
	/// A base class for common counter measure characteristics.
	/// </summary>
	public abstract class CounterMeasuresSystem : MonoBehaviour
	{
		[SerializeField] protected GameObject counterMeasure;
		[SerializeField] protected Transform spawn;
						 
		[SerializeField] protected int amountOfCounterMeasures;
		[SerializeField] protected float counterMeasureReloadRate;
		[SerializeField] protected float counterMeasureDeploySpeed;
		[SerializeField] protected bool canUseCounterMeasures;

		public abstract IEnumerator SpawnCounterMeasures();
		public abstract IEnumerator ReloadCounterMeasures();

		public void FireCounterMeasures()
		{
			if (canUseCounterMeasures)
			{
				StartCoroutine(SpawnCounterMeasures());
			}
		}
	}
}