using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// This class was previously empty. However, because we needed collision on Oculus fingers, it needed to be updated to detect collisions when making a fist or not.
/// 
/// Tom Hewitt
/// </summary>
public class OvrAvatarHand : MonoBehaviour
{
	public Rigidbody rb;
	public bool isLeftHand;

	private void Start()
	{
		isLeftHand = transform.name.Contains("left");
	}

	private void Update()
	{
		// We only want to detect collisions for the hand if it is not a fist.
		// Separate fist collision is detected in the TrackingSpace under the hand Anchors, using the Grababble scripts.
		rb.detectCollisions = isLeftHand ? !OculusInput.IsLeftFistTriggered() : !OculusInput.IsRightFistTriggered();
		//print(transform.name + " detecting collisions: " + rb.detectCollisions);
	}
}
