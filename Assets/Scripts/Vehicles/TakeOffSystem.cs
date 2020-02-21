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
				AudioManager.instance.Play(SoundNames.COCKPIT_BEEP);
				AudioManager.instance.Play(SoundNames.JET_ENGINES);
			}
			else
			{
				SetComponents(false);
				InitiateTakeOffSequence();
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
			yield return new WaitForSeconds(3f);

			AudioManager.instance.Play(SoundNames.JET_STARTUP);

			yield return new WaitForSeconds(1f);

			AudioManager.instance.Play(SoundNames.COCKPIT_BEEP);

			Hud.instance.SetHud(true);

			yield return new WaitForSeconds(8.5f);

			AudioManager.instance.Play(SoundNames.JET_ENGINES);
			AudioManager.instance.Play(SoundNames.JET_ENGINE_KICK);

			yield return Hud.instance.AnimateTakeOffText(HudConstants.TAKE_OFF_SEQUENCE_1);
			yield return Hud.instance.AnimateTakeOffText(HudConstants.TAKE_OFF_SEQUENCE_2);
			yield return Hud.instance.AnimateTakeOffText(HudConstants.TAKE_OFF_SEQUENCE_3);

			yield return new WaitForSeconds(1f);

			yield return Hud.instance.AnimateTakeOffText(HudConstants.TAKE_OFF_SEQUENCE_4);

			AudioManager.instance.Play(SoundNames.COCKPIT_COUNTDOWN);

			yield return new WaitForSeconds(AudioManager.instance.GetSound(SoundNames.COCKPIT_COUNTDOWN).clip.length);

			accelerationRate = 200;

			AudioManager.instance.Play(SoundNames.JET_SONIC_BOOM);

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