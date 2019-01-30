using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Menu
{
public class ControlsMenuHelper : MonoBehaviour 
{
    public GameObject xboxControls;
    public GameObject pcControls;

    public KeyCode nextButton;
    public KeyCode menuButton;

    public string xboxNextButton;
    public string xboxBackButton;

    public string menuScene;

    void Start()
    {
        xboxControls.SetActive(true);
        pcControls.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(nextButton) || Input.GetButton(xboxNextButton))
        {
            Switch();
        }

        if (Input.GetKeyDown(menuButton) || Input.GetButton(xboxBackButton))
        {
            FadeHelper.instance.FadeOutAndLoadScene(menuScene);
        }
    }

    void Switch()
    {
        xboxControls.SetActive(!xboxControls.activeSelf);
        pcControls.SetActive(!pcControls.activeSelf);
    }
}
}
