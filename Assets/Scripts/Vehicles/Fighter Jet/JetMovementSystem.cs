using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vehicle
{
    namespace FighterJet
    {
public class JetMovementSystem : MonoBehaviour 
{
    [Header("Speed Settings")]
    public float maxSpeed;
    public float minSpeed;
    public float cruisingSpeed;
    public float accelerationSpeed;
    public float brakeSpeed;

    [HideInInspector]
    public float currentSpeed;  

    [Header("Rotation Settings")]
    public float pitchSpeed;
    public float rollSpeed;
    public float yawSpeed;

	[Space()]
	public Text speedText;
	public Text altitudeText;

    [Space()]
    public Transform joystick;
    public Transform throttle;
    public Transform throttleStartPoint;

    private Rigidbody rb;

    float x,z;

    void Start ()
    {
        // Set the current speed here. Allows for inspector change without having to alter the rest of the code.
        currentSpeed = cruisingSpeed;
        rb = GetComponent<Rigidbody>();
	}
	
    void FixedUpdate()
    {
        // Move constantly forward
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

		// Update the HUD
        JetHUDSystem.instance.UpdateText(speedText, currentSpeed.ToString("0000"));
        JetHUDSystem.instance.UpdateText(altitudeText, Mathf.Round(transform.position.y).ToString("00000"));

        AlterPitchAndRoll();

        ThrottleControl();

        AdjustBackToCruisingSpeed();
    }

    void ThrottleControl()
    {
        if (Input.GetAxis("Xbox Controller LT") > 0.5f && currentSpeed > minSpeed)
            currentSpeed -= Time.deltaTime * brakeSpeed;

        if (Input.GetAxis("Xbox Controller RT") > 0.5f && currentSpeed < maxSpeed)
            currentSpeed += Time.deltaTime * accelerationSpeed;

        if (Input.GetAxis("Joystick Up/Down") < 0 && currentSpeed > minSpeed)
            currentSpeed -= Time.deltaTime;

        if (Input.GetAxis("Joystick Up/Down") > 0 && currentSpeed < maxSpeed)
            currentSpeed += Time.deltaTime;

        //throttle.forward = new Vector3(0f, 1f, (currentSpeed / 10000f));
        throttle.position = throttleStartPoint.position + (throttle.forward * (currentSpeed / 1000f));
    }

    void AdjustBackToCruisingSpeed()
    {
        if (currentSpeed <= cruisingSpeed && Input.GetAxis("Xbox Controller RT") == 0 && Input.GetAxis("Xbox Controller LT") == 0)
            currentSpeed += Time.deltaTime * 5f;

        if (currentSpeed >= cruisingSpeed && Input.GetAxis("Xbox Controller RT") == 0 && Input.GetAxis("Xbox Controller LT") == 0)
            currentSpeed -= Time.deltaTime * 5f;
    }

    void AlterPitchAndRoll()
    {
        // Rotate using Physics
        rb.AddTorque(Input.GetAxis("Vertical") * transform.right * Time.deltaTime * pitchSpeed);
        rb.AddTorque(Input.GetAxis("Horizontal") * transform.forward * Time.deltaTime * -rollSpeed);

        rb.AddTorque(-Input.GetAxis("Xbox Controller Left Joystick Y") * transform.right * Time.deltaTime * pitchSpeed/2f);
        rb.AddTorque(-Input.GetAxis("Xbox Controller Left Joystick X") * transform.forward * Time.deltaTime * rollSpeed/2f);
    
        // Rotate the joystick model
        x = Input.GetAxis("Vertical") * 20;
        x = Mathf.Clamp(x, -45f, 45f);

        z = Input.GetAxis("Horizontal") * -20;
        z = Mathf.Clamp(z, -45f, 45f);

        joystick.localEulerAngles = new Vector3(x, 0f, z);
    }
        }
}
}
