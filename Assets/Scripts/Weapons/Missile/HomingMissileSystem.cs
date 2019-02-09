using UnityEngine;
using System.Collections;
using Vehicle.FighterJet;
using Vehicle;

namespace Weapons
{
public class HomingMissileSystem : MonoBehaviour 
{
	public 	GameObject missileModel;
	private GameObject smokeParent;
    public  GameObject trailSmoke;
	public 	ParticleSystem smoke;
    public 	float speed = 5f;

    private Transform fighterJet;
    public float turningSpeed;
    private float fuseDelay;

    //[HideInInspector]
    [Tooltip("Can be set if you want to, but doesn't matter if not")]
    public Transform target;

	Rigidbody rb;
	JetCounterMeasuresSystem jetCounterMeasuresSystem;

    void Start () 
    {
        smokeParent 				= GameObject.Find("Smokes");
        fighterJet 					= GameObject.FindGameObjectWithTag ("Fighter Jet").transform;

		jetCounterMeasuresSystem 	= GameObject.FindObjectOfType<JetCounterMeasuresSystem> ();

		rb 							= GetComponent<Rigidbody> ();

        PlayRandomLaunchSound();

        StartCoroutine(DestroyAfterLifetime(15f));
    }


    void FixedUpdate ()
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
            JetHUDSystem.instance.UpdateText (jetCounterMeasuresSystem.lockWarningText, "WARN");
            JetHUDSystem.instance.ChangeColourNormal();

            if (!AudioManager.instance.GetSound("Cockpit Warning Lock").source.isPlaying)
                AudioManager.instance.Play("Cockpit Warning Lock");
		}

        HomeIn();
    }
        

    void HomeIn()
    {
        // Move forward
        //transform.position += transform.forward * speed * Time.deltaTime;
        rb.velocity = transform.forward * speed * 50 * Time.deltaTime; // use this for better looking particle system smoke

		// Look at our target
		Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

		// Rotate towards our target
		rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed));

		//transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
	}


    void PlayRandomLaunchSound()
    {
        int i = Random.Range(1, 4);
        AudioManager.instance.AttachSoundTo("Weapon Homing Missile " + i, this.gameObject);
        GetComponent<AudioSource>().Play();
    }


	void OnCollisionEnter(Collision other)
    {
		switch (other.gameObject.tag)
        {
			case "AI Jet": 
				other.gameObject.GetComponent<VehicleHealthSystem> ().DecreaseHealth (150f);
				StartCoroutine (StopSmokeAndDestroy ());
				break;

			case "Warship": 
				other.gameObject.GetComponent<VehicleHealthSystem> ().DecreaseHealth (150f);
				StartCoroutine (StopSmokeAndDestroy ());
				break;
        }
    }

	void OnTriggerEnter(Collider other)
	{
		switch (other.tag) 
		{
            case "Flare":
                JetHUDSystem.instance.UpdateText (jetCounterMeasuresSystem.lockWarningText, "");

                if (target != null && target.gameObject == fighterJet)
                    AudioManager.instance.Pause("Cockpit Warning Lock");
                    //jetCounterMeasuresSystem.warningLockSound.Pause();

                Destroy(GetComponent<CapsuleCollider>());                 
				StartCoroutine(StopSmokeAndDestroy());
				break;

            case "Fighter Jet":        
                other.gameObject.GetComponent<JetHealthSystem>().DecreaseHealth(20);

                AudioManager.instance.Play("Vehicle Damage");

                // Stops creating explosions over and over
                Destroy(GetComponent<CapsuleCollider>());   

                StartCoroutine(StopSmokeAndDestroy());
                break;

            case "Missile Turret":
                return;

            case "Warship":
                return;
		}
	}


    IEnumerator StopSmokeAndDestroy()
    {
        if (target == fighterJet)
        {
            AudioManager.instance.Pause("Cockpit Warning Lock");
            JetHUDSystem.instance.ChangeColourWarning();
            JetHUDSystem.instance.UpdateText (jetCounterMeasuresSystem.lockWarningText, "");
        }
        
		target = null;
        SetSmokeEmissionRate(0f);
        smoke.transform.parent = smokeParent.transform;
		trailSmoke.transform.parent = smokeParent.transform;
		Destroy (trailSmoke, 7f);
        Destroy(missileModel);
		yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }

    public void SetSmokeEmissionRate(float emissionRate)
    {
        ParticleSystem.EmissionModule emission = smoke.emission;
		ParticleSystem.MinMaxCurve rate = emission.rateOverTime;
        rate.constantMax = emissionRate;
		emission.rateOverTime = rate;
    }

    IEnumerator DestroyAfterLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        StartCoroutine(StopSmokeAndDestroy());
    }
}
}
