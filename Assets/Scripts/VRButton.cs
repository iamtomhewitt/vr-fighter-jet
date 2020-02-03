using UnityEngine;
using UnityEngine.Events;
using Utilities;

/// <summary>
/// A simple push button that relies on the Oculus Touch controllers as fingers to be pressed in.
/// A UnityEvent is used so that any function can be run on the button press.
/// </summary>
public class VRButton : MonoBehaviour
{
	[SerializeField] private GameObject buttonModel;
	[SerializeField] private Transform startPos;
	[SerializeField] private Transform endPos;
	[SerializeField] private UnityEvent buttonPressEvent;

	[SerializeField] private bool pressed;

	private void OnTriggerEnter(Collider collision)
	{
		//print("VRButton " + transform.name + " has Trigger ENTER collided with " + collision.transform.name);

		if (collision.tag.Equals(Tags.VR_FINGER))
		{
			buttonModel.transform.position = endPos.position;
			pressed = true;
		}
	}

	private void OnTriggerExit(Collider collision)
	{
		//print("VRButton " + transform.name + " has Trigger EXIT collided with " + collision.transform.name);

		if (collision.tag.Equals(Tags.VR_FINGER))
		{
			if (pressed)
			{
				buttonPressEvent.Invoke();
			}

			buttonModel.transform.position = startPos.position;
			pressed = false;
		}
	}
}