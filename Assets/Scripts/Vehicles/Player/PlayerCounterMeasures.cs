using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UI;
using Vehicle;

namespace Player
{
	public class PlayerCounterMeasures : CounterMeasuresSystem
	{
		[SerializeField] private Text flareStatusText;
		[SerializeField] private KeyCode flareButton;

		private void Start()
		{
			Hud.instance.SetFlareStatusText(HudConstants.READY);
		}

		private void Update()
		{
			if (Input.GetKeyDown(flareButton) && canUseCounterMeasures)
			{
				Hud.instance.SetFlareStatusText("");
				FireCounterMeasures();
			}
		}

		public override IEnumerator SpawnCounterMeasures()
		{
			canUseCounterMeasures = false;

			AudioManager.instance.Play("Cockpit Flare Deploy");

			for (int i = 0; i < amountOfCounterMeasures; i++)
			{
				GameObject f = Instantiate(counterMeasure, spawn.position, Quaternion.identity) as GameObject;
				Destroy(f, 5f);
				AudioManager.instance.Play("Jet Flare");
				yield return new WaitForSeconds(counterMeasureDeploySpeed);
			}

			StartCoroutine(ReloadCounterMeasures());
		}

		public override IEnumerator ReloadCounterMeasures()
		{
			yield return new WaitForSeconds(counterMeasureReloadRate);
			Hud.instance.SetFlareStatusText(HudConstants.READY);
			AudioManager.instance.Play("Cockpit Beep");
			canUseCounterMeasures = true;
		}
	}
}