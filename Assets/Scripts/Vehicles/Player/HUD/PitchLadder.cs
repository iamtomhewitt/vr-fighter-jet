using UnityEngine;

namespace UI
{
	/// <summary>
	/// The pitch ladder component of the heads up display.
	/// </summary>
	public class PitchLadder : MonoBehaviour
	{
		[SerializeField] private Transform target;
		[SerializeField] private Renderer pitchLadderRenderer;
		[SerializeField] private float pitchLadderSensitivity;
		[SerializeField] private float dampener = 100f;
		private float pitchLadderOffset;
		private float pitch;

		public void SetPitch(Transform fighterJet)
		{
			pitch = SignedAngle(fighterJet.forward, ProjectPointOnPlane(Vector3.up, Vector3.zero, fighterJet.forward), fighterJet.right);
		}

		public void SetPitchOffset()
		{
			pitchLadderOffset = pitch * (pitchLadderSensitivity / dampener);
		}

		public void UpdatePitchLadderMaterial()
		{
			pitchLadderRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, pitchLadderOffset));
		}

		public float GetPitch()
		{
			return pitch;
		}

		private float SignedAngle(Vector3 v1, Vector3 v2, Vector3 normal)
		{
			Vector3 perp = Vector3.Cross(normal, v1);
			float angle = Vector3.Angle(v1, v2);
			angle *= Mathf.Sign(Vector3.Dot(perp, v2));
			return angle;
		}

		private Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
		{
			planeNormal.Normalize();
			float distance = -Vector3.Dot(planeNormal.normalized, (point - planePoint));
			return point + planeNormal * distance;
		}

		public void AlignWithHorizon(Transform fighterJet)
		{
			target.localRotation = Quaternion.Euler(fighterJet.localEulerAngles.z + 90f, 0f, 90f);
			pitchLadderRenderer.transform.localRotation = Quaternion.Euler(fighterJet.localEulerAngles.z + 90f, 0f, 90f);
		}
	}
}