using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileTurret
{
public class MissileTurretTrackingSystem : MonoBehaviour 
{
	public GameObject target;
	public GameObject body;
	public GameObject barrell;

	public float turningSpeed;

	public bool lockedOn;
	public bool shootsAI;

	private Vector3 distance;
	private Quaternion barrellRotation, bodyRotation;

	void Update()
	{
		if (target && lockedOn)
		{
			distance = target.transform.position - transform.position;

			barrellRotation = Quaternion.LookRotation (distance);
			bodyRotation = Quaternion.LookRotation (new Vector3(distance.x, 0f, distance.z));

			body.transform.rotation = Quaternion.Slerp (body.transform.rotation, bodyRotation, turningSpeed * Time.deltaTime);
			barrell.transform.rotation = Quaternion.Slerp (barrell.transform.rotation, barrellRotation, turningSpeed * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Homing Missile")
            return;
        
        if (!shootsAI && other.tag == "Fighter Jet")
            LockOnto(other);

		if (shootsAI && other.tag == "AI Jet")
			LockOnto(other);

	}

	void OnTriggerExit()
	{
		lockedOn = false;
		target = null;
	}

	void LockOnto(Collider o)
	{
		lockedOn = true;
		//print(gameObject.name + " has locked onto: " + o.gameObject.name);
		target = o.gameObject;
	}
}
}
