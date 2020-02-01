using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UI;

namespace Player
{
	public class CounterMeasuresSystem : MonoBehaviour
	{
		[SerializeField] private GameObject flare;
		[SerializeField] private Transform flareSpawn;
		[SerializeField] private Text flareStatusText;

		[SerializeField] private KeyCode flareButton;

		[SerializeField] private int amountOfFlares;
		[SerializeField] private float flareReloadRate;
		[SerializeField] private float flareDeploySpeed;

		private bool canUseFlares;

		private void Start()
		{
			Hud.instance.SetFlareStatusText(HudConstants.READY);
		}

		private void Update()
		{
			if (Input.GetKeyDown(flareButton) && canUseFlares )
			{
				Hud.instance.SetFlareStatusText("");
				DeployFlares();
			}
		}

		/// <summary>
		/// Called from a VRButton.
		/// </summary>
		public void DeployFlares()
		{
			canUseFlares = false;
			StartCoroutine(SpawnFlares());
		}

		private IEnumerator SpawnFlares()
		{
			AudioManager.instance.Play("Cockpit Flare Deploy");

			for (int i = 0; i < amountOfFlares; i++)
			{
				GameObject f = Instantiate(flare, flareSpawn.position, Quaternion.identity) as GameObject;
				Destroy(f, 5f);
				AudioManager.instance.Play("Jet Flare");
				yield return new WaitForSeconds(flareDeploySpeed);
			}

			StartCoroutine(ReloadFlares());
		}

		private IEnumerator ReloadFlares()
		{
			yield return new WaitForSeconds(flareReloadRate);
			Hud.instance.SetFlareStatusText(HudConstants.READY);
			AudioManager.instance.Play("Cockpit Beep");
			canUseFlares = true;
		}
	}
}