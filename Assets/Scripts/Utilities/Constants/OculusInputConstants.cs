using UnityEngine;

namespace Utilities
{
	/// <summary>
	/// A class to quickly access the triggers on an Oculus Touch controller.
	/// </summary>
	public class OculusInputConstants
	{
		private static string LEFT_INDEX = "Oculus_CrossPlatform_PrimaryIndexTrigger";
		private static string RIGHT_INDEX = "Oculus_CrossPlatform_SecondaryIndexTrigger";
		private static string LEFT_HAND = "Oculus_CrossPlatform_PrimaryHandTrigger";
		private static string RIGHT_HAND = "Oculus_CrossPlatform_SecondaryHandTrigger";

		public static bool IsLeftIndexTriggered()
		{
			return Input.GetAxis(LEFT_INDEX) > 0.7f;
		}

		public static bool IsLeftHandTriggered()
		{
			return Input.GetAxis(LEFT_HAND) > 0.7f;
		}

		public static bool IsRightIndexTriggered()
		{
			return Input.GetAxis(RIGHT_INDEX) > 0.7f;
		}

		public static bool IsRightHandTriggered()
		{
			return Input.GetAxis(RIGHT_HAND) > 0.7f;
		}

		public static bool IsLeftFistTriggered()
		{
			return (IsLeftIndexTriggered() && IsLeftHandTriggered());
		}

		public static bool IsRightFistTriggered()
		{
			return (IsRightIndexTriggered() && IsRightHandTriggered());
		}
	}
}