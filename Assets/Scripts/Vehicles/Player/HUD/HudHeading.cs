using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	/// <summary>
	/// The heading component of the heads up display.
	/// </summary>
	public class HudHeading : MonoBehaviour
	{
		[SerializeField] private Text headingText;
		[SerializeField] private Renderer headingRenderer;
		[SerializeField] private float headingSensitivity;
		[SerializeField] private float dampener = 100f;
		private float headingDirection;
		private float headingOffset;

		public void SetDirection(Transform fighterJet)
		{
			headingDirection = Mathf.Atan2(fighterJet.forward.z, fighterJet.forward.x) * Mathf.Rad2Deg;
		}

		public void SetHeadingOffset()
		{
			headingOffset = headingDirection * (headingSensitivity / dampener);
		}

		public void UpdateHeadingMaterial()
		{
			headingRenderer.material.SetTextureOffset("_MainTex", new Vector2(headingOffset, 0f));
		}

		public void SetHeadingText()
		{
			headingText.text = headingDirection.ToString(HudConstants.HEADING_FORMAT);
		}

		public float GetDirection()
		{
			return headingDirection;
		}
	}
}