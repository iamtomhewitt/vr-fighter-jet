using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
	/// <summary>
	/// The lock on graphic present on each enemy that the player sees.
	/// </summary>
	public class EnemyLockOnUi : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer sprite;
		[SerializeField] private Color colour;
		[SerializeField] private Color lockedColour;

		[SerializeField] private float objectScale = 0.025f;

		private Camera cam;
		private Vector3 initialScale;

		private void Start()
		{
			// Record initial scale, use this as a basis
			initialScale = transform.localScale;
			cam = Camera.main;
			ResetLockColour();
		}

		private void Update()
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