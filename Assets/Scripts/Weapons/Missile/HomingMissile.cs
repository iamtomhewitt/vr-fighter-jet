using UnityEngine;
using System.Collections;
using Player;
using UI;
using Utilities;

namespace Weapons
{
	public class HomingMissile : MonoBehaviour
	{
		[Tooltip("Can be set if you want to, but doesn't matter if not")]
		[SerializeField] private Transform target;
		[SerializeField] private GameObject missileModel;
		[SerializeField] private GameObject trailSmoke;
		[SerializeField] private ParticleSystem smoke;
		[SerializeField] private float speed = 5f;
		[SerializeField] private float turningSpeed;
		
		private GameObject smokeParent;
		private Transform fighterJet;
		private Rigidbody rb;
		private PlayerCounterMeasures jetCounterMeasuresSystem;
		private Collider missileCollider;
		private float fuseDelay;
		private float speedMultiplier = 50f;
	
		private void Start()
		{
			smokeParent = GameObject.Find(Tags.SMOKES);
			fighterJet = GameObject.FindGameObjectWithTag(Tags.FIGHTER_JET).transform;

			jetCounterMeasuresSystem = FindObjectOfType<PlayerCounterMeasures>();

			rb = GetComponent<Rigidbody>();
			missileCollider = GetComponent<Collider>();

			StartCoroutine(DelayCollider());
			StartCoroutine(DestroyAfterLifetime(7.5f));

			PlayRandomLaunchSound();
		}

		private void FixedUpdate()
		{
			// A target may be null if another homing missile has already shot it down
			if (target == null)
			{
				StartCoroutine(StopSmokeAndDestroy());
				return;
			}

			// If the target is the player, then update the hud text
			if (target == fighterJet)
			{
				//Hud.instance.UpdateText (jetCounterMeasuresSystem.lockWarningText, "WARN");
				print("TODO: Warning text");
				Hud.instance.ShowHostileLock();

				if (!AudioManager.instance.GetSound("Cockpit Warning Lock").source.isPlaying)
				{
					AudioManager.instance.Play("Cockpit Warning Lock");
				}
			}

			HomeIn();
		}

		private IEnumerator DelayCollider()
		{
			missileCollider.enabled = false;
			yield return new WaitForSeconds(1f);
			missileCollider.enabled = true;
		}

		/// <summary>
		/// Moves towards a target.
		/// </summary>
		private void HomeIn()
		{
			// Move using rigidbody for betting looking particle smoke
			rb.velocity = transform.forward * speed * speedMultiplier * Time.deltaTime;

			// Look at our target
			Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

			// Rotate towards our target
			rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed));
		}

		private void PlayRandomLaunchSound()
		{
			int i = Random.Range(1, 4);
			AudioManager.instance.AttachSoundTo(SoundNames.MISSILE_1, this.gameObject).Play();
		}

		private void OnCollisionEnter(Collision other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.TARGET:
					other.gameObject.GetComponent<HealthSystem>().RemoveHealth(150);
					StartCoroutine(StopSmokeAndDestroy());
					break;

				case Tags.ENVIRONMENT:
					StartCoroutine(StopSmokeAndDestroy());
					break;

				default:
					print("UNRECOGNISED TAG: " + other.gameObject.tag);
					StartCoroutine(StopSmokeAndDestroy());
					break;
			}
		}

		void OnTriggerEnter(Collider other)
		{
			switch (other.tag)
			{
				case Tags.FLARE:
					//Hud.instance.UpdateText(jetCounterMeasuresSystem.lockWarningText, "");
					print("TODO: Turn off warning");

					if (target != null && target.gameObject == fighterJet)
					{
						AudioManager.instance.Pause("Cockpit Warning Lock");
					}
					//jetCounterMeasuresSystem.warningLockSound.Pause();

					Destroy(missileCollider);
					StartCoroutine(StopSmokeAndDestroy());
					break;

				case Tags.FIGHTER_JET:
					other.gameObject.GetComponent<HealthSystem>().RemoveHealth(20);

					AudioManager.instance.Play("Vehicle Damage");

					// Stops creating explosions over and over
					Destroy(missileCollider);

					StartCoroutine(StopSmokeAndDestroy());
					break;

				default:
					print("UNRECOGNISED TAG: " + other.gameObject.tag);
					break;
			}
		}

		private IEnumerator StopSmokeAndDestroy()
		{
			if (target == fighterJet)
			{
				AudioManager.instance.Pause("Cockpit Warning Lock");
				Hud.instance.ChangeColourWarning();
				//Hud.instance.UpdateText(jetCounterMeasuresSystem.lockWarningText, "");
				print("TODO: Update text");
			}

			target = null;
			SetSmokeEmissionRate(0f);
			smoke.transform.parent = smokeParent.transform;
			trailSmoke.transform.parent = smokeParent.transform;
			Destroy(trailSmoke, 7f);
			Destroy(missileModel);
			yield return new WaitForSeconds(.5f);
			Destroy(this.gameObject);
		}

		private void SetSmokeEmissionRate(float emissionRate)
		{
			ParticleSystem.EmissionModule emission = smoke.emission;
			ParticleSystem.MinMaxCurve rate = emission.rateOverTime;
			rate.constantMax = emissionRate;
			emission.rateOverTime = rate;
		}

		private IEnumerator DestroyAfterLifetime(float lifetime)
		{
			yield return new WaitForSeconds(lifetime);
			StartCoroutine(StopSmokeAndDestroy());
		}

		public void SetTarget(Transform target)
		{
			this.target = target;
		}

		public Transform GetTarget()
		{
			return target;
		}
	}
}