using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Vehicle
{
    namespace FighterJet
    {
        public class JetTakeOffSystem : MonoBehaviour
        {
            public Text informationText;

            [Header("Activated After Takeoff")]
            public MonoBehaviour[] jetScripts;
            public BoxCollider boxCollider;
            public bool quickTakeOff;

            float accelerationRate;

            Rigidbody rb;

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
                    StartCoroutine(TakeOff());
                }
            }

            void FixedUpdate()
            {
                rb.AddForce(transform.forward * accelerationRate, ForceMode.Acceleration);
            }

            IEnumerator TakeOff()
            {
                JetHUDSystem.instance.gameObject.SetActive(false);

                yield return new WaitForSeconds(3f);

                AudioManager.instance.Play("Jet Startup");

                yield return new WaitForSeconds(1f);

                AudioManager.instance.Play("Cockpit Beep");

                JetHUDSystem.instance.gameObject.SetActive(true);

                yield return new WaitForSeconds(8.5f);

                AudioManager.instance.Play("Jet Engines");
                AudioManager.instance.Play("Jet Engine Kick");
                yield return JetHUDSystem.instance.AnimateText(informationText, "INITIATING TAKE OFF SEQUENCE \n\nENGINES   /INIT \nWEAPONS /INIT");
                yield return JetHUDSystem.instance.AnimateText(informationText, "\n\nENGINES   /OK \nWEAPONS /OK");
                yield return JetHUDSystem.instance.AnimateText(informationText, "\n\nSYSTEMS NOMINAL");

                yield return new WaitForSeconds(1f);

                JetHUDSystem.instance.UpdateText(informationText, "INITIATING TAKE OFF SEQUENCE");
                yield return JetHUDSystem.instance.AnimateText(informationText, "\n\nLAUNCHING...");

                AudioManager.instance.Play("Cockpit Takeoff Countdown");

                yield return new WaitForSeconds(AudioManager.instance.GetSound("Cockpit Takeoff Countdown").clip.length);

                accelerationRate = 200;

                AudioManager.instance.Play("Jet Takeoff Blast");
                JetHUDSystem.instance.UpdateText(informationText, "");
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
