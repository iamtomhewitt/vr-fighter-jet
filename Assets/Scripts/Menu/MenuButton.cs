using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

namespace Menu
{
public class MenuButton : MonoBehaviour
{
    public GameObject selectedBackground;
    public RectTransform loadingBar;
    public string sceneToLoad;
    public float loadingBarSpeed;

    public enum ButtonType
    {
        Play, Load, Quit
    };
    public ButtonType type;


    public void DoAction()
    {
        switch (type)
        {
            case ButtonType.Load:
                LoadScene();
                break;

            case ButtonType.Play:
                PlayerPrefs.SetInt("First Play", 0);
                print("PlayerPrefs: First Play - " + PlayerPrefs.GetInt("First Play").ToString());
                LoadScene();
                break;

            case ButtonType.Quit:
                Application.Quit();
                break;
        }
    }


    public void HighlightButton(bool yes)
    {
        selectedBackground.SetActive(yes);
    }

	void LoadScene()
	{
        FadeHelper.instance.FadeOutAndLoadScene (sceneToLoad);
	}

    public float IncreaseLoadingBar()
    {
        loadingBar.localScale += new Vector3(loadingBarSpeed / 3f * Time.deltaTime, 0f, 0f);
        return loadingBar.localScale.x;
    }

    public void ResetLoadingBar()
    {
        loadingBar.localScale = new Vector3(0f, 1f, 1f);
    }
}
}
