using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A simple push button that relies on the Oculus Touch controllers as fingers to be pressed in.
/// A UnityEvent is used so that any function can be run on the button press.
/// </summary>
public class VRButton : MonoBehaviour
{
	public GameObject buttonModel;

	public Transform startPos;
	public Transform endPos;

	public UnityEvent buttonPressEvent;

	public bool pressed;

	private string lastCollidedTag = "";

	private void OnTriggerEnter(Collider collision)
	{
		// If the button has collided with our finger, and we have font collided with a finger before
		if (collision.tag == "VR Finger" && collision.tag != lastCollidedTag)
		{
			buttonModel.transform.position = endPos.position;
			pressed = true;
		}

		lastCollidedTag = collision.tag;
	}

	private void OnTriggerExit(Collider collision)
	{
		if (collision.tag == "VR Finger" && collision.tag != lastCollidedTag)
		{
			if (pressed)
				buttonPressEvent.Invoke();

			buttonModel.transform.position = startPos.position;
			pressed = false;
		}

		lastCollidedTag = "";
	}
}
