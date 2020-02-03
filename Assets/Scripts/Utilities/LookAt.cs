using UnityEngine;

namespace Utilities
{
	public class LookAt : MonoBehaviour
	{
		[SerializeField] private Transform target;

		private void Update()
		{
			// Use value of 2 to make it not inverted
			transform.LookAt(2 * transform.position - target.position);
		}
	}
}