using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Vehicle
{
    namespace FighterJet
    {
public class JetPauseSystem : MonoBehaviour
{
	public GameObject pauseMenu;
	public bool isPaused;

	public void PauseGame()
	{
		isPaused = true;
		Time.timeScale = 0f;
		pauseMenu.SetActive (true);

        AudioListener.pause = true;
	}

	public void UnPauseGame()
	{
		isPaused = false;
		Time.timeScale = 1f;
		pauseMenu.SetActive (false);

        AudioListener.pause = false;
	}

	void Start()
	{
        int firstPlay = PlayerPrefs.GetInt("First Play");

        if (firstPlay == 0)
        {
            print("This is the very first time playing.");
            FadeHelper.instance.FadeInAndPause();
        }
        else
        {
            print("This is NOT the very first time playing.");
            pauseMenu.SetActive(false);
            AudioListener.pause = false;
            FadeHelper.instance.FadeIn();
        }
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (isPaused) 
			{
				UnPauseGame ();
			} 
			else 
			{
				PauseGame ();
			}
		}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isPaused) 
            {
                UnPauseGame ();
                FadeHelper.instance.FadeOutAndLoadScene("Main Menu");
            }
        }
	}
}
    }
}
