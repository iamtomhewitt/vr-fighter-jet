using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Utilities;
using Vehicle;

namespace Player
{
	public class PlayerHealth : HealthSystem
	{
		[SerializeField] private Text healthText;

		void OnTriggerEnter(Collider other)
		{
			if (other.tag.Equals(Tags.ENVIRONMENT))
			{
				AudioListener.pause = true;
				StartCoroutine(TriggerGameOver());
			}
		}

		public override void Die()
		{
			StartCoroutine(TriggerGameOver());
		}

		private IEnumerator TriggerGameOver()
		{
			PlayerPrefs.SetInt(PlayerPrefConstants.FIRST_PLAY_KEY, 1);
			FadeHelper.instance.BlackOut();
			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}