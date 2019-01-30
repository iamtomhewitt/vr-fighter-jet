using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Utilities;

namespace Vehicle
{
    namespace FighterJet
    {
public class JetHealthSystem : MonoBehaviour 
{
    public int health = 100;
    public Text healthText;

    public void DecreaseHealth(int amount)
    {
        health -= amount;

        JetHUDSystem.instance.UpdateText(healthText, health.ToString() + "%");

        if (health <= 0)
        {
            StartCoroutine(TriggerGameOver());
        }
    }

	IEnumerator TriggerGameOver()
    {
        PlayerPrefs.SetInt("First Play", 1);

        FadeHelper.instance.BlackOut ();
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Environment")
        {
            AudioListener.pause = true;
			StartCoroutine(TriggerGameOver());
        }
    }
}
    }
}
