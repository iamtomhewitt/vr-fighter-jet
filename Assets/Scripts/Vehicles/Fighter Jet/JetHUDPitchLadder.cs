using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vehicle
{
    namespace FighterJet
    {
public class JetHUDPitchLadder : MonoBehaviour 
{
	public  Rigidbody jetRigidbody;
	public  Transform jetTransform;
	private Renderer rend;
    public  float scrollDamp;
    private float offset;

	void Start()
	{
        rend = GetComponent<Renderer> ();
	}
	
	void Update () 
	{
        offset += jetRigidbody.angularVelocity.z / scrollDamp;
        rend.material.SetTextureOffset ("_MainTex", new Vector2 (0f, -offset));	

        // Have to do some funky stuff here as the rotation is off as it is a child of the Fighter jet.
        // This sets the pitch ladder to the opposite angle of the jet, always aligning with the horizon,
        // whilst maintaining facing the pilot.
        transform.localRotation = Quaternion.Euler(jetTransform.localEulerAngles.z + 90f, 90f, 90f);
	}
}
    }
}
