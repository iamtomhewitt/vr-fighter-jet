using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UI
{
	public class Hud : MonoBehaviour
	{
		public static Hud instance;

		[SerializeField] private Color hudColour, warningColour;
		private Rigidbody rb;
		private bool active = false;

		[Header("Pitch Ladder Settings")]
		[SerializeField] private GameObject pitchLadder;
		[SerializeField] private float pitchLadderSensitivity;
		private Renderer pitchLadderRenderer;
		private float pitchLadderOffset;
		private float pitch;		

		[Header("Altitude Settings")]
		[SerializeField] private GameObject altitude;
		[SerializeField] private Text altitudeText;
		[SerializeField] private float altitudeSensitivity;
		private Renderer altitudeRenderer;
		private float altitudeOffset;

		[Space()]
		[SerializeField] private GameObject centralTarget;
		[SerializeField] private Text flaresText;
		[SerializeField] private Text targetInformationText;
		[SerializeField] private Text speedText;
		[SerializeField] private Text targetDistanceText;
		[SerializeField] private Text healthText;
		[SerializeField] private Text takeoffText;
		[SerializeField] private Text warningText;

		[SerializeField] private HudHeading heading;

		[SerializeField] private Component[] hudComponents;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			rb					= GetComponent<Rigidbody>();
			pitchLadderRenderer = pitchLadder.GetComponent<Renderer>();
			altitudeRenderer	= altitude.GetComponent<Renderer>();

			ColourHUD(hudColour);
		}

		private void Update()
		{
			Vector3 pos = ProjectPointOnPlane(Vector3.up, Vector3.zero, transform.forward);

			// Pitch
			pitch = SignedAngle(transform.forward, pos, transform.right);
			pitchLadderOffset = pitch * (pitchLadderSensitivity / 10);
			pitchLadderRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, pitchLadderOffset));

			// Altitude
			altitudeOffset = transform.position.y * (altitudeSensitivity / 10);
			altitudeRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, altitudeOffset));
			altitudeText.text = transform.position.y.ToString("00000");

			heading.SetDirection(transform);
			heading.SetHeadingOffset();
			heading.UpdateHeadingMaterial();
			heading.SetHeadingText();

			// Ladders
			pitchLadder.transform.localRotation = AlignWithHorizon();
			centralTarget.transform.localRotation = AlignWithHorizon();
		}

		private void OnGUI()
		{
			GUI.Label(new Rect(20, 0, 200, 40), "Heading: " + heading.GetDirection().ToString());
			GUI.Label(new Rect(20, 20, 200, 40), "Pitch: " + pitch.ToString());
		}

		private Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
		{
			planeNormal.Normalize();
			float distance = -Vector3.Dot(planeNormal.normalized, (point - planePoint));
			return point + planeNormal * distance;
		}

		private float SignedAngle(Vector3 v1, Vector3 v2, Vector3 normal)
		{
			Vector3 perp = Vector3.Cross(normal, v1);
			float angle = Vector3.Angle(v1, v2);
			angle *= Mathf.Sign(Vector3.Dot(perp, v2));
			return angle;
		}

		private Quaternion AlignWithHorizon()
		{
			return Quaternion.Euler(transform.localEulerAngles.z + 90f, 0, 90);
		}

		public void ChangeColourNormal()
		{
			ColourHUD(hudColour);
		}

		public void ChangeColourWarning()
		{
			ColourHUD(warningColour);
		}

		public void ToggleHUD()
		{
			active = !active;
			SetHud(active);
		}

		public IEnumerator AnimateTakeOffText(string msg)
		{
			yield return StartCoroutine(AnimateText(takeoffText, msg));
		}

		private IEnumerator AnimateText(Text t, string msg)
		{
			int i = 0;

			while (i < msg.Length)
			{
				t.text += msg[i++];
				yield return new WaitForSeconds(HudConstants.TEXT_ANIMATION_SPEED);
			}

			yield return new WaitForSeconds(HudConstants.HALF_SECOND);
		}

		/// <summary>
		/// Changes the Hud to a certain colour.
		/// </summary>
		private void ColourHUD(Color colour)
		{
			for (int i = 0; i < hudComponents.Length; i++)
			{
				Component c = hudComponents[i];
				Image image			= c.GetComponent<Image>();
				Text text			= c.GetComponent<Text>();
				Renderer renderer	= c.GetComponent<Renderer>();

				if (image)
				{
					image.color = colour;
				}

				if (text)
				{
					text.color = colour;
				}

				if (renderer)
				{
					MaterialPropertyBlock props = new MaterialPropertyBlock();
					props.SetColor("_Color", colour);
					renderer.SetPropertyBlock(props);
				}
			}
		}

		/// <summary>
		/// Updates the Hud to show the distance and information about a looked target.
		/// </summary>
		public void SetTargetInformationText(string distance, string name)
		{
			targetDistanceText.text = distance;
			targetInformationText.text = name;
		}

		public void SetFlareStatusText(string text)
		{
			flaresText.text = text;
		}

		public void SetSpeedText(string text)
		{
			speedText.text = text;
		}

		public void SetTakeOffText(string text)
		{
			takeoffText.text = text;
		}

		public void ShowHostileLock()
		{
			warningText.text = HudConstants.WARNING;
			ChangeColourWarning();
		}

		public void SetHud(bool on)
		{
			for (int i = 0; i < hudComponents.Length; i++)
			{
				hudComponents[i].gameObject.SetActive(on);
			}
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Hud))]
public class HUDEditor : Editor
{
	Hud hud = null;

	void OnEnable()
	{
		hud = (Hud)target;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Colour HUD"))
		{
			hud.ChangeColourNormal();
		}
	}
}
#endif

