using UnityEngine;

namespace Utilities
{
	public class Rotate : MonoBehaviour
	{
		[SerializeField] private Vector3 speed;

		private void Update()
		{
			transform.Rotate(speed);
		}
	}
}