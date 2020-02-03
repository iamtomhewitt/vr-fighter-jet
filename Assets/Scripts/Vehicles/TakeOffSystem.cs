using UnityEngine;
using System.Collections;
using UI;

namespace Player
{
	public class TakeOffSystem : MonoBehaviour
	{
		[SerializeField] private bool quickTakeOff;
		[SerializeField] private bool takeOffInitiated;

		[Header("Activated After Takeoff")]
		[SerializeField] private MonoBehaviour[] jetScripts;
		[SerializeField] private BoxCollider boxCollider;

		private float accelerationRate;

		private Rigidbody rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody>();

			if (quickTakeOff)
			{
				AudioManager.instance.Play("Cockpit Beep");
				AudioManager.instance.Play("Jet Engines");
			}
			else
			{
				SetComponents(false);
			}
		}

		private void FixedUpdate()
		{
			rb.AddForce(transform.forward * accelerationRate, ForceMode.Acceleration);
		}

		/// <summary>
		/// Called from the VRButton in the cockpit, so that the engines are started via the player.
		/// </summary>
		public void InitiateTakeOffSequence()
		{
			if (!takeOffInitiated)
			{
				takeOffInitiated = true;
				StartCoroutine(TakeOff());
			}
		}

		private IEnumerator TakeOff()
		{
			Hud.instance.SetHud(false);

			AudioManager.instance.Play("Jet Startup");

			yield return new WaitForSeconds(1f);

			AudioManager.instance.Play("Cockpit Beep");

			Hud.instance.SetHud(true);

			yield return new WaitForSeconds(8.5f);

			AudioManager.instance.Play("Jet Engines");
			AudioManager.instance.Play("Jet Engine Kick");

			yield return Hud.instance.AnimateTakeOffText(HudConstants.TAKE_OFF_SEQUENCE_1);
			yield return Hud.instance.AnimateTakeOffText(HudConstants.TAKE_OFF_SEQUENCE_2);
			yield return Hud.instance.AnimateTakeOffText(HudConstants.TAKE_OFF_SEQUENCE_3);

			yield return new WaitForSeconds(1f);

			yield return Hud.instance.AnimateTakeOffText(HudConstants.TAKE_OFF_SEQUENCE_4);

			AudioManager.instance.Play("Cockpit Takeoff Countdown");

			yield return new WaitForSeconds(AudioManager.instance.GetSound("Takeoff Countdown").clip.length);

			accelerationRate = 200;

			AudioManager.instance.Play("Jet Takeoff Blast");

			Hud.instance.SetTakeOffText("");

			yield return new WaitForSeconds(3f);

			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;

			SetComponents(true);
			Destroy(this);
		}

		private void SetComponents(bool isActive)
		{
			for (int i = 0; i < jetScripts.Length; i++)
			{
				jetScripts[i].enabled = isActive;
			}

			boxCollider.enabled = isActive;
		}
	}
}
