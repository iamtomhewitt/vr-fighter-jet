using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapons;
using AIFighterJet;
using Utilities;

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
    public GameObject muzzleFlash;

    public 	float bulletFireRate;
    private float bulletCooldown;

    public GameObject[] bulletSpawns;

    [Header("Homing Missile Settings")]
    public GameObject homingMissile;
    public Transform  homingMissileSpawn;
    public Text  statusText;
    public Text  distanceText;
    public float lockOnDistance;
    public float homingMissileFireRate;

	private GameObject target;
	private GameObject lastLookedTarget;
	private bool  missileLock;
	private float homingMissileCooldown;

    void Update()
    {
        // 'Cool' the cooldowns
        bulletCooldown -= Time.deltaTime;
        homingMissileCooldown -= Time.deltaTime;

		// Control checks
        ControlBulletAudio();
		CheckForMissileLock ();

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
        // Instantiate a physical bullet spawn and a muzzleflash
        Instantiate(bullet,
            bulletSpawns[Random.Range(0, bulletSpawns.Length)].transform.position,
            bulletSpawns[Random.Range(0, bulletSpawns.Length)].transform.rotation);

        Instantiate(muzzleFlash,
            bulletSpawns[0].transform.position,
            bulletSpawns[0].transform.rotation);
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

    void CheckForMissileLock()
    {
		GameObject lookedObject = GazeRaycaster.GetSphereCastedGameObject (lockOnDistance, 75f);

		if (lookedObject != null && (lookedObject.tag == "AI Jet" || lookedObject.tag == "Warship")) 
		{
			// Work out the distance between us
			float distance = Mathf.Round (Vector3.Distance (transform.position, lookedObject.transform.position));

			// Update the HUD
            JetHUDSystem.instance.UpdateText (statusText, "TARGET ACQUIRED");

			// Set the distance and target
            JetHUDSystem.instance.UpdateText (distanceText, distance.ToString ("F000"));
			target = lookedObject;

			// The last thing we looked at is our target
			lastLookedTarget = target;

			// Activate the HUD lock on, and we now have missile lock
			SetLockColour (target, true);
			missileLock = true;
		} 
		else 
		{
			// We are not hitting anything
			target = null;

			// Turn off the HUD target of the last thing we looked at
			SetLockColour(lastLookedTarget, false);

			// Update the HUD texts, and we no longer have a missile lock
            JetHUDSystem.instance.UpdateText (statusText, "");
            JetHUDSystem.instance.UpdateText (distanceText, "");
			missileLock = false;
		}
    }

    void FireHomingMissile()
    {
        // Instantiate a new homing missile and set its target
        GameObject m = Instantiate(homingMissile, homingMissileSpawn.position, Quaternion.identity) as GameObject;
        m.GetComponent<HomingMissileSystem>().target = target.transform;
    }

	void SetLockColour(GameObject t, bool locked)
	{
		if (t == null)
			return;

        if(locked)
            t.GetComponent<AIJetCounterMeasuresSystem>().lockOnGraphic.SetLockColour();
        else
            t.GetComponent<AIJetCounterMeasuresSystem>().lockOnGraphic.ResetLockColour();

//		// For every child transform in the target
//		foreach (Transform g in t.transform) 
//		{
//			// If its a lock on graphic
//			if (g.tag == "Lock On") 
//			{
//				g.gameObject.SetActive (active);
//				return;
//			}
//		}
//
//		print ("Couldn't find lock on graphic!");
	}
}
}
}