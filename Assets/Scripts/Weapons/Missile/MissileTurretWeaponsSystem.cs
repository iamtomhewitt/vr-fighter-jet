using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace MissileTurret
{
public class MissileTurretWeaponsSystem : MonoBehaviour
{
	public GameObject homingMissile;
	public Transform spawn;

	MissileTurretTrackingSystem trackingSystem;

	void Start()
	{
		trackingSystem = GetComponent<MissileTurretTrackingSystem>();
		InvokeRepeating("Fire", 5f, 10f);
	}

	void Fire()
	{
        if (trackingSystem.lockedOn && trackingSystem.target != null)
        {
            GameObject g = Instantiate(homingMissile, spawn.position, spawn.transform.rotation) as GameObject;
            g.GetComponent<HomingMissile>().SetTarget(trackingSystem.target.transform);
        }
	}
}
}
