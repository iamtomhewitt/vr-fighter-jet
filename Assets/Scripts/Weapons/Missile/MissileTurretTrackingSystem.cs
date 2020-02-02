using UnityEngine;
using Utilities;

namespace Weapons
{
	public class MissileTurretTrackingSystem : MonoBehaviour
	{
		[SerializeField] private GameObject target;
		[SerializeField] private GameObject body;
		[SerializeField] private GameObject barrell;
		[SerializeField] private float turningSpeed;
		[SerializeField] private bool lockedOn;
		[SerializeField] private bool shootsAI;

		private Vector3 distance;
		private Quaternion barrellRotation, bodyRotation;

		private void Update()
		{
			if (target && lockedOn)
			{
				distance = target.transform.position - transform.position;

				barrellRotation = Quaternion.LookRotation(distance);
				bodyRotation = Quaternion.LookRotation(new Vector3(distance.x, 0f, distance.z));

				body.transform.rotation = Quaternion.Slerp(body.transform.rotation, bodyRotation, turningSpeed * Time.deltaTime);
				barrell.transform.rotation = Quaternion.Slerp(barrell.transform.rotation, barrellRotation, turningSpeed * Time.deltaTime);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag.Equals(Tags.HOMING_MISSILE))
			{
				return;
			}

			if (!shootsAI && other.tag.Equals(Tags.FIGHTER_JET))
			{
				LockOnto(other.gameObject);
			}

			if (shootsAI && other.tag.Equals(Tags.AI_JET))
			{
				LockOnto(other.gameObject);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			lockedOn = false;
			target = null;
		}

		private void LockOnto(GameObject g)
		{
			lockedOn = true;
			print(gameObject.name + " has locked onto: " + g.name);
			target = g;
		}

		public bool IsLockedOn()
		{
			return lockedOn;
		}

		public GameObject GetTarget()
		{
			return target;
		}
	}
}