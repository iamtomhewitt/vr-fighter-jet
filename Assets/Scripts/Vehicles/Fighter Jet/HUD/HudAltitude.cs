using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The altitude component of the heads up display.
/// </summary>
public class HudAltitude : MonoBehaviour
{
	[SerializeField] private Text altitudeText;
	[SerializeField] private Renderer altitudeRenderer;
	[SerializeField] private float altitudeSensitivity;
	[SerializeField] private float dampener = 100f;

	private float altitudeOffset;

	public void SetAltitudeOffset(Transform fighterJet)
	{
		altitudeOffset = fighterJet.position.y * (altitudeSensitivity / dampener);
	}

	public void UpdateAltitudeMaterial()
	{
		altitudeRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, altitudeOffset));
	}

	public void SetAltitudeText()
	{
		altitudeText.text = transform.position.y.ToString(HudConstants.ALTITUDE_FORMAT);
	}
}
