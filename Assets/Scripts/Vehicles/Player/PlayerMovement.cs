using UnityEngine;
using UnityEngine.UI;
using UI;
using Utilities;

namespace Player
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private Text speedText;

		[SerializeField] private float maxSpeed;
		[SerializeField] private float minSpeed;
		[SerializeField] private float cruisingSpeed;
		[SerializeField] private float accelerationSpeed;
		[SerializeField] private float brakeSpeed;
		[SerializeField] private float pitchSpeed;
		[SerializeField] private float rollSpeed;
		[SerializeField] private float yawSpeed;

		private Rigidbody rb;

		private float currentSpeed;
		private float x;
		private float z;

		private void Start()
		{
			currentSpeed = cruisingSpeed;
			rb = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			transform.position += transform.forward * currentSpeed * Time.deltaTime;

			Hud.instance.SetSpeedText(currentSpeed.ToString(HudConstants.SPEED_FORMAT));

			AlterPitchAndRoll();

			SetSpeedOnXboxTriggerValues();

			AdjustBackToCruisingSpeed();
		}

		private void SetSpeedOnXboxTriggerValues()
		{
			if (Input.GetAxis(ControllerConstants.XBOX_LEFT_TRIGGER) > 0.5f && currentSpeed > minSpeed)
			{
				currentSpeed -= Time.deltaTime * brakeSpeed;
			}

			if (Input.GetAxis(ControllerConstants.XBOX_RIGHT_TRIGGER) > 0.5f && currentSpeed < maxSpeed)
			{
				currentSpeed += Time.deltaTime * accelerationSpeed;
			}
		}

		private void AdjustBackToCruisingSpeed()
		{
			if (currentSpeed <= cruisingSpeed && Input.GetAxis(ControllerConstants.XBOX_RIGHT_TRIGGER) == 0 && Input.GetAxis(ControllerConstants.XBOX_LEFT_TRIGGER) == 0)
			{
				currentSpeed += Time.deltaTime * 5f;
			}

			if (currentSpeed >= cruisingSpeed && Input.GetAxis(ControllerConstants.XBOX_RIGHT_TRIGGER) == 0 && Input.GetAxis(ControllerConstants.XBOX_LEFT_TRIGGER) == 0)
			{
				currentSpeed -= Time.deltaTime * 5f;
			}
		}

		private void AlterPitchAndRoll()
		{
			// Rotate using Physics
			rb.AddTorque(Input.GetAxis(ControllerConstants.VERTICAL) * transform.right * Time.deltaTime * pitchSpeed);
			rb.AddTorque(Input.GetAxis(ControllerConstants.HORIZONTAL) * transform.forward * Time.deltaTime * -rollSpeed);

			rb.AddTorque(-Input.GetAxis(ControllerConstants.XBOX_LEFT_JOYSTICK_Y) * transform.right * Time.deltaTime * pitchSpeed / 2f);
			rb.AddTorque(-Input.GetAxis(ControllerConstants.XBOX_LEFT_JOYSTICK_X) * transform.forward * Time.deltaTime * rollSpeed / 2f);

			// Rotate the joystick model
			x = Input.GetAxis(ControllerConstants.VERTICAL) * 20;
			x = Mathf.Clamp(x, -45f, 45f);

			z = Input.GetAxis(ControllerConstants.HORIZONTAL) * -20;
			z = Mathf.Clamp(z, -45f, 45f);
		}
	}
}