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

		[SerializeField] private Color hudColour;
		[SerializeField] private Color warningColour;
		[SerializeField] private Text flaresText;
		[SerializeField] private Text targetInformationText;
		[SerializeField] private Text speedText;
		[SerializeField] private Text targetDistanceText;
		[SerializeField] private Text healthText;
		[SerializeField] private Text takeoffText;
		[SerializeField] private Text warningText;
		[SerializeField] private Text statusText;

		[SerializeField] private HudHeading heading;
		[SerializeField] private PitchLadder pitchLadder;

		[SerializeField] private GameObject[] hudComponents;

		private Rigidbody rb;
		private bool active = false;
		
		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			rb = GetComponent<Rigidbody>();
			statusText.text = "";
			ColourHUD(hudColour);
		}

		private void Update()
		{
			pitchLadder.SetPitch(transform);
			pitchLadder.SetPitchOffset();
			pitchLadder.UpdatePitchLadderMaterial();
			pitchLadder.AlignWithHorizon(transform);

			heading.SetDirection(transform);
			heading.SetHeadingOffset();
			heading.UpdateHeadingMaterial();
			heading.SetHeadingText();
		}

		private void OnGUI()
		{
			GUI.Label(new Rect(20, 0, 200, 40), "Heading: " + heading.GetDirection().ToString());
			GUI.Label(new Rect(20, 20, 200, 40), "Pitch: " + pitchLadder.GetPitch().ToString());
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
				GameObject c = hudComponents[i];
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

		public void SetStatusText(string text)
		{
			statusText.text = text;
		}

		public void SetStatusTextColour(Color colour)
		{
			statusText.color = colour;
		}

		public void ResetStatusTextColour()
		{
			statusText.color = hudColour;
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

