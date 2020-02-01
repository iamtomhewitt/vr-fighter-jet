using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapons;
using AIFighterJet;
using Utilities;
using UI;

namespace Vehicle
{
	namespace FighterJet
	{
		/// <summary>
		/// Class that controls the weapons for the fighter jet. Consists of Cannon control,
		/// and Homing Missile control.
		/// </summary>
		public class JetWeaponSystem : MonoBehaviour
		{
			[Header("Cannon/Bullet Settings")]
			public GameObject bullet;
			public float bulletFireRate;
			private float bulletCooldown;
			public GameObject[] bulletSpawns;

			[Header("Homing Missile Settings")]
			public GameObject homingMissile;
			public Transform[] homingMissileSpawns;
			public float homingMissileFireRate;
			private float homingMissileCooldown;
			private bool missileLock;
			private int missileSpawnIndex = 0;

			[Header("Missile Lock Settings")]
			public float lockOnDistance;
			public float lockOnRadius;

			private GameObject target;
			private GameObject lastLookedTarget;

			void Update()
			{
				bulletCooldown -= Time.deltaTime;
				homingMissileCooldown -= Time.deltaTime;

				// Control checks
				ControlBulletAudio();
				GetMissileLock();

				// If we press bullet fire
				if ((Input.GetButton("Xbox Controller LB") || Input.GetButton("Fire1")) && bulletCooldown <= 0)
				{
					// Fire a bullet
					FireBullet();

					// Reset the cooldown
					bulletCooldown = bulletFireRate;
				}

				// If we press missile fire
				if (((Input.GetButtonDown("Xbox Controller RB") || Input.GetButton("Fire2")) && missileLock == true) && homingMissileCooldown <= 0f)
				{
					// Fire a missile
					FireHomingMissile();

					// Reset the cooldown
					homingMissileCooldown = homingMissileFireRate;
				}
			}

			void FireBullet()
			{
				Transform spawn = bulletSpawns[Random.Range(0, bulletSpawns.Length)].transform;
				Instantiate(bullet, spawn.position, spawn.rotation);
			}

			void ControlBulletAudio()
			{
				// Have to do the audio separate as cooldown was giving jittery effects
				if ((Input.GetButton("Xbox Controller LB") || Input.GetButton("Fire1")))
					AudioManager.instance.Play("Weapon Cannon");
				//bulletSound.Play();
				else
					AudioManager.instance.Pause("Weapon Cannon");
				//bulletSound.Pause();
			}

			void GetMissileLock()
			{
				GameObject lookedObject = GazeRaycaster.GetSphereCastedGameObject(lockOnDistance, lockOnRadius);

				if (lookedObject != null && lookedObject.tag.Equals("Target"))
				{
					// Work out the distance between us
					float distance = Mathf.Round(Vector3.Distance(transform.position, lookedObject.transform.position));

					Hud.instance.SetTargetInformationText(distance.ToString("F000"), lookedObject.gameObject.tag);

					target = lookedObject;

					// The last thing we looked at is our target
					lastLookedTarget = target;

					// Activate the HUD lock on, and we now have missile lock
					SetLockColour(target, true);
					missileLock = true;
				}
				else
				{
					// We are not hitting anything
					target = null;

					// Turn off the HUD target of the last thing we looked at
					SetLockColour(lastLookedTarget, false);

					// Update the HUD texts, and we no longer have a missile lock
					Hud.instance.SetTargetInformationText("", "");
					missileLock = false;
				}
			}

			void FireHomingMissile()
			{
				Transform spawn = homingMissileSpawns[missileSpawnIndex];
				missileSpawnIndex++;
				if (missileSpawnIndex == homingMissileSpawns.Length)
					missileSpawnIndex = 0;

				// Instantiate a new homing missile and set its target
				GameObject m = Instantiate(homingMissile, spawn.position, spawn.rotation) as GameObject;
				m.GetComponent<HomingMissileSystem>().target = target.transform;
			}

			void SetLockColour(GameObject t, bool locked)
			{
				if (t == null)
					return;

				// For every child transform in the target
				foreach (Transform g in t.transform)
				{
					// If its a lock on graphic
					if (g.tag == "Lock On")
					{
						LockOnGraphic graphic = g.GetComponent<LockOnGraphic>();
						if (locked) graphic.SetLockColour(); else graphic.ResetLockColour();
						return;
					}
				}

				print("Couldn't find lock on graphic!");
			}
		}
	}
}