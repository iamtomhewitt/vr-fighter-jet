using UnityEngine;
using System.Collections;

public class screenshots : MonoBehaviour 
{
    bool paused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;

            if (paused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}
