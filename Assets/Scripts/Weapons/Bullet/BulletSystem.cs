using AIFighterJet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vehicle;

namespace Weapons
{
	public class BulletSystem : MonoBehaviour
	{
		public float speed;

		void Start()
		{
			Destroy(this.gameObject, 5f);
			transform.parent = GameObject.Find("Bullets").transform;
		}

		void Update()
		{
			transform.position += transform.forward * speed * Time.deltaTime;
		}

		void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.tag == "AI Jet" || other.gameObject.tag == "Warship")
			{
				other.gameObject.GetComponent<HealthSystem>().RemoveHealth(40);
				Destroy(this.gameObject);
			}
		}
	}
}
