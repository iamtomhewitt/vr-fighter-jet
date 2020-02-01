using UnityEngine;
using System.Collections;
using UI;

namespace Vehicle
{
    namespace FighterJet
    {
        public class JetTakeOffSystem : MonoBehaviour
        {
			public bool quickTakeOff;
			public bool takeOffInitiated;

            [Header("Activated After Takeoff")]
            public MonoBehaviour[] jetScripts;
            public BoxCollider boxCollider;

            private float accelerationRate;

            private Rigidbody rb;

            void Start()
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

            void FixedUpdate()
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

            IEnumerator TakeOff()
            {
				Hud.instance.SetHud(false);

                AudioManager.instance.Play("Jet Startup");

                yield return new WaitForSeconds(1f);

                AudioManager.instance.Play("Cockpit Beep");

				Hud.instance.SetHud(true);

				yield return new WaitForSeconds(8.5f);

                AudioManager.instance.Play("Jet Engines");
                AudioManager.instance.Play("Jet Engine Kick");

                yield return Hud.instance.AnimateTakeOffText("INITIATING TAKE OFF SEQUENCE \n\nENGINES   /INIT \nWEAPONS /INIT");
                yield return Hud.instance.AnimateTakeOffText("\n\nENGINES   /OK \nWEAPONS /OK");
                yield return Hud.instance.AnimateTakeOffText("\n\nSYSTEMS NOMINAL");

                yield return new WaitForSeconds(1f);

				yield return Hud.instance.AnimateTakeOffText("\n\nLAUNCHING...");

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

            void SetComponents(bool isActive)
            {
                for (int i = 0; i < jetScripts.Length; i++)
                {
                    jetScripts[i].enabled = isActive;
                }

                boxCollider.enabled = isActive;
            }
        }
    }
}
