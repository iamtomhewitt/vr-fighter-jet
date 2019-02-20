using UnityEngine;
using System.Collections;
using Weapons;

namespace AIFighterJet
{
	public class AIJetWeaponsSystem : MonoBehaviour
	{
		public GameObject homingMissile;
		private GameObject target;

		public Transform spawn;

		public float lockOnDistance;
		public float lockOnRadius;

		private bool isInvoking = false;

		void Start()
		{
			// Better than doing it every frame every second, do it over time
			InvokeRepeating("HandleRaycastLockOn", 0f, .3f);
		}

		void HandleRaycastLockOn()
		{
			// Cast a ray out from the transform position
			RaycastHit hit;

			Debug.DrawRay(spawn.position, transform.forward * lockOnDistance, Color.red);

			Ray ray = new Ray(spawn.position, transform.forward);

			if (Physics.SphereCast(ray, lockOnRadius, out hit, lockOnDistance))
			{
				if (hit.collider.tag.Equals("Target"))
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

		void Fire()
		{
			GameObject g = Instantiate(homingMissile, spawn.position, spawn.rotation) as GameObject;
			g.GetComponent<HomingMissileSystem>().target = target.transform;
			print(this.gameObject.name + " has fired a missile at " + g.GetComponent<HomingMissileSystem>().target.name);
		}
	}
}
