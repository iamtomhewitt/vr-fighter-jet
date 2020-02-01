using UnityEngine;

namespace Utilities
{
	/// <summary>
	/// A singleton that controls pausing of the game.
	/// </summary>
	public class PauseSystem: MonoBehaviour
	{
		[SerializeField] private GameObject pauseMenu;

		private bool isPaused;

		public static PauseSystem instance;

		private void Start()
		{
			instance = this;

			int firstPlay = PlayerPrefs.GetInt(PlayerPrefConstants.FIRST_PLAY_KEY);

			if (firstPlay == 0)
			{
				print("This is the very first time playing");
				FadeHelper.instance.FadeInAndPause();
			}
			else
			{
				print("This is NOT the very first time playing");
				pauseMenu.SetActive(false);
				AudioListener.pause = false;
				FadeHelper.instance.FadeIn();
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				if (isPaused)
				{
					UnPauseGame();
				}
				else
				{
					PauseGame();
				}
			} 
		}

		public void PauseGame()
		{
			isPaused = true;
			Time.timeScale = 0f;
			pauseMenu.SetActive(isPaused);

			AudioListener.pause = isPaused;
		}

		public void UnPauseGame()
		{
			isPaused = false;
			Time.timeScale = 1f;
			pauseMenu.SetActive(isPaused);

			AudioListener.pause = isPaused;
		}
	}
}