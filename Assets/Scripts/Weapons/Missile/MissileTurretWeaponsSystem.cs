using UnityEngine;

namespace Weapons
{
	public class MissileTurretWeaponsSystem : MonoBehaviour
	{
		[SerializeField] private GameObject homingMissile;
		[SerializeField] private Transform spawn;
		[SerializeField] private float fireRate;

		private MissileTurretTrackingSystem trackingSystem;

		private void Start()
		{
			trackingSystem = GetComponent<MissileTurretTrackingSystem>();
			InvokeRepeating("Fire", fireRate, fireRate);
		}

		private void Fire()
		{
			if (trackingSystem.IsLockedOn() && trackingSystem.GetTarget() != null)
			{
				Instantiate(homingMissile, spawn.position, spawn.transform.rotation).GetComponent<HomingMissile>().SetTarget(trackingSystem.GetTarget().transform);
			}
		}
	}
}
