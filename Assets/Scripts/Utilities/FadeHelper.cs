using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vehicle.FighterJet;

namespace Utilities
{
public class FadeHelper : MonoBehaviour 
{
	public float fadeSpeed = 0.75f;
	public CanvasGroup canvasGroup;

	public static FadeHelper instance;

    /// <summary>
    /// Check if there is another FadeHelper present in the scene. If there is, destroy it and use this one instead.
    /// Useful for testing, you can have as many FadeHelpers as you want but they will be destroyed when
    /// ran in play mode.
    /// </summary>
	void Awake()
	{
		if (instance) 
		{
			DestroyImmediate (gameObject);
		} 
		else 
		{
			DontDestroyOnLoad (this.gameObject);
			instance = this;
		}
	}	


    /// <summary>
    /// When enabled, subscribe to delegate.
    /// </summary>
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnNewLevelLoaded;
    }


    /// <summary>
    /// When disabled, unsubscribe to delegate.
    /// </summary>
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnNewLevelLoaded;
    }


    /// <summary>
    /// Whenever a new level/scene is loaded, it should be faded in.
    /// </summary>
    void OnNewLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeIn();
    }


    /// <summary>
    /// Public method to call Fade Coroutine.
    /// </summary>
	public void FadeIn()
	{
		StartCoroutine (FadeInFromBlack ());
	}


    /// <summary>
    /// Public method to call Fade Coroutine.
    /// </summary>
	public void FadeInAndPause()
	{
		StartCoroutine (FadeInFromBlackAndPause ());
	}

    /// <summary>
    /// Public method to call Fade Coroutine.
    /// </summary>
	public void FadeOut()
	{
		StartCoroutine (FadeOutToBlack ());
	}


    /// <summary>
    /// Public method to call Fade Coroutine.
    /// </summary>
	public void FadeOutAndLoadScene(string sceneName)
	{
		StartCoroutine (FadeOutToBlackAndLoadScene (sceneName));
	}

    /// <summary>
    /// Turns the screen instant pitch black.
    /// </summary>
	public void BlackOut()
	{
		canvasGroup.alpha = 1f;
	}

    /// <summary>
    /// Coroutines for fading listed here.
    /// </summary>
	IEnumerator FadeInFromBlack()
	{
		canvasGroup.alpha = 1f;

		while (canvasGroup.alpha > 0) 
		{
			canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
			yield return null;
		}

		canvasGroup.interactable = false;
		yield return null;
	}


	IEnumerator FadeInFromBlackAndPause()
	{
		canvasGroup.alpha = 1f;

		while (canvasGroup.alpha > 0) 
		{
			canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
			yield return null;
		}

		canvasGroup.interactable = false;
		GameObject.FindObjectOfType<JetPauseSystem> ().PauseGame ();
		yield return null;
	}


	IEnumerator FadeOutToBlack()
	{
		canvasGroup.alpha = 0f;

		while (canvasGroup.alpha < 1)
		{
			canvasGroup.alpha += Time.deltaTime * fadeSpeed;
			yield return null;
		}

		canvasGroup.interactable = false;
		yield return null;
	}


	IEnumerator FadeOutToBlackAndLoadScene(string sceneName)
	{
		canvasGroup.alpha = 0f;

		while (canvasGroup.alpha < 1)
		{
			canvasGroup.alpha += Time.deltaTime * fadeSpeed;
			yield return null;
		}

		canvasGroup.interactable = false;
		yield return null;
		SceneManager.LoadScene (sceneName);
	}
}
}
