using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Utilities;

namespace Vehicle
{
	namespace FighterJet
	{
		public class JetHealthSystem : HealthSystem
		{
			public Text healthText;

			IEnumerator TriggerGameOver()
			{
				PlayerPrefs.SetInt("First Play", 1);

				FadeHelper.instance.BlackOut();
				yield return new WaitForSeconds(1f);
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}

			void OnTriggerEnter(Collider other)
			{
				if (other.tag == "Environment")
				{
					AudioListener.pause = true;
					StartCoroutine(TriggerGameOver());
				}
			}

			public override void Die()
			{
				StartCoroutine(TriggerGameOver());
			}
		}
	}
}
