using UnityEngine;

namespace Utilities
{
	public class RotateAround : MonoBehaviour
	{
		[SerializeField] private Transform jet;
		[SerializeField] private float speed;
		[SerializeField] private float amplitude;
		[SerializeField] private float frequency;

		private void Update()
		{
			transform.LookAt(jet);
			transform.Translate(Vector3.right * Time.deltaTime * speed);

			transform.position += amplitude * (Mathf.Sin(2 * Mathf.PI * frequency * Time.time) - Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - Time.deltaTime))) * transform.up;
		}
	}
}