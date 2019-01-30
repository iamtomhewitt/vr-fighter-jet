using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vehicle
{
namespace FighterJet
{
public class JetCounterMeasuresSystem : MonoBehaviour 
{
    [Header("Flare Settings")]
    public GameObject flare;
    public Text flareStatusText;
    public Transform flareSpawn;
    public KeyCode flareButton;
    public int amountOfFlares;
    public float flareReloadRate;
    public float flareDeploySpeed;

    private float cooldown;

    [Header("Lock Warning Settings")]
    public Text lockWarningText;

	void Start () 
    {
        JetHUDSystem.instance.UpdateText(flareStatusText, "READY");
	}

    void Update()
    {
        cooldown -= Time.deltaTime;

        // this could maybe done with an invoke repeating or couroutine bool control.
        // invoke method that sets bool to allowed to fire 10 seconds after pressing flare button
        if (Input.GetKeyDown(flareButton) && cooldown < 0)
        {
            cooldown = flareReloadRate;
            JetHUDSystem.instance.UpdateText(flareStatusText, "");
            StartCoroutine(SpawnFlares());
        }
    }

    IEnumerator SpawnFlares()
    {
        AudioManager.instance.Play("Cockpit Flare Deploy");

        for (int i = 0; i < amountOfFlares; i++)
        {
            GameObject f = Instantiate(flare, flareSpawn.position, Quaternion.identity) as GameObject;
            Destroy(f, 5f);
            AudioManager.instance.Play("Jet Flare");
            yield return new WaitForSeconds(flareDeploySpeed);
        }

        yield return new WaitForSeconds(flareReloadRate);

        AudioManager.instance.Play("Cockpit Beep");
        JetHUDSystem.instance.UpdateText(flareStatusText, "READY");
    }
}
    }
}
