using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIFighterJet
{
	public class LockOnGraphic : MonoBehaviour
	{
		public Color colour;
		public Color lockedColour;

		public float objectScale = 0.025f;

		public SpriteRenderer sprite;

		private Vector3 initialScale;
		private Camera cam;

		void Start()
		{
			// Record initial scale, use this as a basis
			initialScale = transform.localScale;

			cam = Camera.main;

			ResetLockColour();
		}

		void Update()
		{
			// Scale object relative to distance from camera plane
			Plane plane = new Plane(cam.transform.forward, cam.transform.position);
			float dist = plane.GetDistanceToPoint(transform.position);

			transform.localScale = initialScale * Mathf.Abs(dist) / objectScale;
			transform.LookAt(cam.transform);
		}

		public void SetLockColour()
		{
			sprite.color = lockedColour;
		}

		public void ResetLockColour()
		{
			sprite.color = colour;
		}
	}
}
