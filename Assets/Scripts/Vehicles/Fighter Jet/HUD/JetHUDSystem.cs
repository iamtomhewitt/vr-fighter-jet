using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Vehicle.FighterJet;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Vehicle
{
	namespace FighterJet
	{
		public class JetHUDSystem : MonoBehaviour
		{
			public Color HUDColour, warningColour;
			public static JetHUDSystem instance;
			private Rigidbody rb;

			[Header("Pitch Ladder Settings")]
			public GameObject pitchLadder;
			private Renderer pitchLadderRenderer;
			public float pitchLadderSensitivity;
			private float pitchLadderOffset;
			private float pitch;

			[Header("Heading Settings")]
			public GameObject heading;
			public Text headingText;
			private Renderer headingRenderer;
			public float headingSensitivity;
			private float headingDirection;
			private float headingOffset;

			[Header("Altitude Settings")]
			public GameObject altitude;
			private Renderer altitudeRenderer;
			public Text altitudeText;
			public float altitudeSensitivity;
			private float altitudeOffset;

			[Space()]
			public GameObject centralTarget;
			public Text flaresText;
			public Text targetInformationText;
			public Text targetDistanceText;
			public Text healthText;
			public Text takeoffText;

			[Space()]
			public Component[] HUDComponents;

			private void Awake()
			{
				instance = this;
			}


			private void Start()
			{
				rb = GetComponent<Rigidbody>();
				pitchLadderRenderer = pitchLadder.GetComponent<Renderer>();
				altitudeRenderer = altitude.GetComponent<Renderer>();
				headingRenderer = heading.GetComponent<Renderer>();

				ColourHUD(HUDColour);
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

				// Heading
				headingDirection = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;
				headingOffset = headingDirection * (headingSensitivity / 10);
				headingRenderer.material.SetTextureOffset("_MainTex", new Vector2(headingOffset, 0f));
				headingText.text = headingDirection.ToString("000");

				// Ladders
				pitchLadder.transform.localRotation = AlignWithHorizon();
				centralTarget.transform.localRotation = AlignWithHorizon();			
			}


			private void OnGUI()
			{
				GUI.Label(new Rect(20, 0, 200, 40), "Heading: "+headingDirection.ToString());
				GUI.Label(new Rect(20, 20, 200, 40), "Pitch: "+pitch.ToString());
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
				ColourHUD(HUDColour);
			}


			public void ChangeColourWarning()
			{
				ColourHUD(warningColour);
			}


			public void TurnHUD(bool on)
			{
				for (int i = 0; i < HUDComponents.Length; i++)
				{
					Component c = HUDComponents[i];
					c.gameObject.SetActive(on);
				}
			}

			/// <summary>
			/// Updates the UI Text to the Message supplied.
			/// </summary>
			//public void UpdateText(Text text, string message)
			//{
			//	text.text = message;
			//}


			public IEnumerator AnimateText(Text t, string msg)
			{
				int i = 0;
				//t.text = "";
				while (i < msg.Length)
				{
					t.text += msg[i++];
					yield return new WaitForSeconds(.00005f);
				}

				yield return new WaitForSeconds(.5f);
			}


			private void ColourHUD(Color colour)
			{
				for (int i = 0; i < HUDComponents.Length; i++)
				{
					Component c = HUDComponents[i];

					if (c.GetComponent<Image>())
						c.GetComponent<Image>().material.color = colour;

					if (c.GetComponent<Text>())
						c.GetComponent<Text>().color = colour;

					if (c.GetComponent<Renderer>())
						c.GetComponent<Renderer>().sharedMaterial.color = colour;
				}
			}

			public void SetTargetInformationText(string distance, string name)
			{
				targetDistanceText.text = distance;
				targetInformationText.text = name;
			}
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(JetHUDSystem))]
public class HUDEditor : Editor
{
	JetHUDSystem hud = null;

	void OnEnable()
	{
		hud = (JetHUDSystem)target;
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

