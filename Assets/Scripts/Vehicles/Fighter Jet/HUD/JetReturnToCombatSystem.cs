using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utilities;

namespace Vehicle
{
	namespace FighterJet
	{
		public class JetReturnToCombatSystem : MonoBehaviour
		{
			public GameObject warning;
			public Text warningText;
			public int warningTime;
			public bool insideCombatArea;

			void OnTriggerEnter(Collider other)
			{
				if (other.tag == "Combat Area")
				{
					insideCombatArea = true;
					warning.SetActive(false);
					StopCoroutine(Countdown());
				}
			}

			void OnTriggerExit(Collider other)
			{
				if (other.tag == "Combat Area")
				{
					insideCombatArea = false;
					warning.SetActive(true);
					StartCoroutine(Countdown());
				}
			}

			IEnumerator Countdown()
			{
				for (int i = warningTime; i >= 0; i--)
				{
					if (insideCombatArea)
						break;

					warningText.text = "RETURN TO COMBAT\n00:" + i.ToString("00");
					yield return new WaitForSeconds(1f);

					if (i == 0)
					{
						FadeHelper.instance.BlackOut();
						yield return new WaitForSeconds(1f);
						SceneManager.LoadScene(SceneManager.GetActiveScene().name);
						FadeHelper.instance.FadeIn();
						print("Countdown finished.");
					}
				}
			}
		}
	}
}
