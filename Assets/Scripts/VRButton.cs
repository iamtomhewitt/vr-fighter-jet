using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VRButton : MonoBehaviour
{
	public GameObject buttonModel;

	public Transform startPos;
	public Transform endPos;

	public bool pressed;

	public UnityEvent buttonPressEvent;

	string hand = "RightHandAnchor";

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.transform.name == hand)
		{
			buttonModel.transform.position = endPos.position;
			pressed = true;
		}
	}

	private void OnTriggerExit(Collider collision)
	{
		if (collision.transform.name == hand)
		{
			if (pressed)
				buttonPressEvent.Invoke();

			buttonModel.transform.position = startPos.position;
			pressed = false;
		}
	}
}
