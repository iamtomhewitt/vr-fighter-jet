using UnityEngine;
using Utilities;
using Weapons;

namespace Enemy
{
	public class EnemyWeapons : MonoBehaviour
	{
		[SerializeField] private GameObject homingMissile;
		[SerializeField] private Transform spawn;
		[SerializeField] private float lockOnDistance;
		[SerializeField] private float lockOnRadius;

		private GameObject target;
		private bool isInvoking = false;

		private void Start()
		{
			// Better than doing it every frame every second, do it over time
			InvokeRepeating("HandleRaycastLockOn", 0f, .3f);
		}

		private void HandleRaycastLockOn()
		{
			// Cast a ray out from the transform position
			RaycastHit hit;

			Debug.DrawRay(spawn.position, transform.forward * lockOnDistance, Color.red);

			Ray ray = new Ray(spawn.position, transform.forward);

			if (Physics.SphereCast(ray, lockOnRadius, out hit, lockOnDistance))
			{
				if (hit.collider.tag.Equals(Tags.TARGET))
				{
					print(this.gameObject.name + " has locked onto " + hit.collider.name + ", FIRING");
					target = hit.collider.gameObject;

					if (!isInvoking)
					{
						InvokeRepeating("Fire", 0f, 3f);
						isInvoking = true;
					}
				}
			}
			else
			{
				// We are not hitting anything
				isInvoking = false;
				CancelInvoke("Fire");
			}
		}

		private void Fire()
		{
			Instantiate(homingMissile, spawn.position, spawn.rotation).GetComponent<HomingMissile>().SetTarget(target.transform);
			print(gameObject.name + " has fired a missile at " + target.name);
		}
	}
}