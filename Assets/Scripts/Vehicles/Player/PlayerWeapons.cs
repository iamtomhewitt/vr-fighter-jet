using UnityEngine;
using Weapons;
using Enemy;
using Utilities;
using UI;

namespace Player
{
	/// <summary>
	/// Class that controls the weapons for the player.
	/// </summary>
	public class PlayerWeapons : MonoBehaviour
	{
		[Header("Cannon")]
		[SerializeField] private GameObject bullet;
		[SerializeField] private GameObject[] bulletSpawns;
		[SerializeField] private float bulletFireRate;

		[Header("Homing Missile")]
		[SerializeField] private GameObject homingMissile;
		[SerializeField] private Transform[] homingMissileSpawns;
		[SerializeField] private float homingMissileFireRate;
		[SerializeField] private float lockOnDistance;
		[SerializeField] private float lockOnRadius;

		private GameObject target;
		private GameObject lastLookedTarget;
		private float homingMissileCooldown;
		private float bulletCooldown;
		private bool missileLock;
		private int missileSpawnIndex = 0;

		private void Update()
		{
			bulletCooldown -= Time.deltaTime;
			homingMissileCooldown -= Time.deltaTime;

			ControlBulletAudio();
			CheckForTargets();

			// If we press bullet fire
			if ((Input.GetButton(ControllerConstants.XBOX_LB) || Input.GetButton("Fire1")) && bulletCooldown <= 0)
			{
				// Fire a bullet
				FireBullet();

				// Reset the cooldown
				bulletCooldown = bulletFireRate;
			}

			// If we press missile fire
			if (((Input.GetButtonDown(ControllerConstants.XBOX_RB) || Input.GetButton("Fire2")) && missileLock) && homingMissileCooldown <= 0f)
			{
				// Fire a missile
				FireHomingMissile();

				// Reset the cooldown
				homingMissileCooldown = homingMissileFireRate;
			}
		}

		/// <summary>
		/// TODO - Use a bullet pool instead of instantiation.
		/// </summary>
		private void FireBullet()
		{
			Transform spawn = bulletSpawns[Random.Range(0, bulletSpawns.Length)].transform;
			Instantiate(bullet, spawn.position, spawn.rotation);
		}

		/// <summary>
		/// Have to do the audio separate as cooldown was giving jittery effects
		/// </summary>
		private void ControlBulletAudio()
		{
			// Have to do the audio separate as cooldown was giving jittery effects
			if ((Input.GetButton(ControllerConstants.XBOX_LB) || Input.GetButton("Fire1")))
			{
				AudioManager.instance.Play(SoundNames.CANNON);
			}
			else
			{
				AudioManager.instance.Pause(SoundNames.CANNON);
			}
		}

		private void CheckForTargets()
		{
			GameObject lookedObject = GazeRaycaster.GetSphereCastedGameObject(lockOnDistance, lockOnRadius);

			if (lookedObject != null && lookedObject.tag.Equals(Tags.TARGET))
			{
				// Work out the distance between us
				float distance = Mathf.Round(Vector3.Distance(transform.position, lookedObject.transform.position));

				Hud.instance.SetTargetInformationText(distance.ToString(HudConstants.DISTANCE_FORMAT), lookedObject.gameObject.tag);

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

		private void FireHomingMissile()
		{
			Transform spawn = homingMissileSpawns[missileSpawnIndex];

			missileSpawnIndex++;
			if (missileSpawnIndex.Equals(homingMissileSpawns.Length))
			{
				missileSpawnIndex = 0;
			}

			// Instantiate a new homing missile and set its target
			Instantiate(homingMissile, spawn.position, spawn.rotation).GetComponent<HomingMissile>().SetTarget(target.transform);
		}

		private void SetLockColour(GameObject t, bool locked)
		{
			if (t == null)
			{
				return;
			}

			// For every child transform in the target
			foreach (Transform g in t.transform)
			{
				// If its a lock on graphic
				if (g.tag.Equals(Tags.LOCK_ON))
				{
					EnemyLockOnUi graphic = g.GetComponent<EnemyLockOnUi>();
					if (locked)
					{
						graphic.SetLockColour();
					}
					else
					{
						graphic.ResetLockColour();
					}
					return;
				}
			}

			print("Couldn't find lock on graphic!");
		}
	}
}