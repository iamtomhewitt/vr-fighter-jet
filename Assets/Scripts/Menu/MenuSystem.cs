using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

namespace Menu
{
public class MenuSystem : MonoBehaviour 
{
	public 	GameObject currentLookedButton;
	public 	GameObject lastLookedObject;
	private GameObject lookedObject;
	private bool isLoadingScene;

	void Update()
	{
		lookedObject = GazeRaycaster.GetRaycastedGameObject();

        if (lookedObject != null && lookedObject.tag == "Button") 
		{
			// Then the current button and the last button we looked at is this button
			currentLookedButton = lookedObject;
			lastLookedObject = currentLookedButton;

			MenuButton button = currentLookedButton.GetComponent<MenuButton> ();

			if (!isLoadingScene) 
			{
                button.HighlightButton(true);
                button.IncreaseLoadingBar();

                if (button.IncreaseLoadingBar() >= 1f) // fire i sused for the piuase menu in the game
                {
                    button.DoAction();
                    isLoadingScene = true;
                }
			}
		} 
		else
		{
			// If we are looking at a different button or nothing at all, the last button we looked
			// at should have stuff done to it
            if (lastLookedObject != null) 
			{
                MenuButton lastButton = lastLookedObject.GetComponent<MenuButton>();

                lastButton.HighlightButton(false);                
                lastButton.ResetLoadingBar();
			}
		}
	}
}
}