using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Utilities;
using UI;

namespace Manager
{
	public class CombatAreaManager : MonoBehaviour
	{
		[SerializeField] private int warningTime;
		[SerializeField] private bool insideCombatArea;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag.Equals(Tags.COMBAT_AREA))
			{
				insideCombatArea = true;
				StopCoroutine(Countdown());
				Hud.instance.SetStatusText("");
				Hud.instance.ResetStatusTextColour();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.tag.Equals(Tags.COMBAT_AREA))
			{
				insideCombatArea = false;
				StartCoroutine(Countdown());
			}
		}

		private IEnumerator Countdown()
		{
			for (int i = warningTime; i >= 0; i--)
			{
				if (insideCombatArea)
				{
					break;
				}

				Hud.instance.SetStatusText("RETURN TO COMBAT\n00:" + i.ToString("00"));
				Hud.instance.SetStatusTextColour(Color.red);
				yield return new WaitForSeconds(1f);

				if (i == 0)
				{
					FadeHelper.instance.BlackOut();
					yield return new WaitForSeconds(1f);
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					FadeHelper.instance.FadeIn();
				}
			}
		}
	}
}
