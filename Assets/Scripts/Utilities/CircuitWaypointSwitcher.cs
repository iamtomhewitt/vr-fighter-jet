using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace Utilities
{
	/// <summary>
	/// Class that randomises the positions of the Waypoints of the circuit every X seconds.
	/// Makes the jets look more like they are making their own flying decisions instead of flying around in a constant loop.
	/// </summary>
	public class CircuitWaypointSwitcher : MonoBehaviour
	{
		public Boundary xBoundary;
		public Boundary yBoundary;
		public Boundary zBoundary;

		private Transform[] waypoints;

		void Start()
		{
			waypoints = GetComponent<WaypointCircuit>().Waypoints;
			InvokeRepeating("SwitchWaypointPositions", 10f, 10f);
		}


		void SwitchWaypointPositions()
		{
			for (int i = 0; i < waypoints.Length; i++)
			{
				Transform waypoint = waypoints[i];

				float randX = Random.Range(xBoundary.min, xBoundary.max);
				float randY = Random.Range(yBoundary.min, yBoundary.max);
				float randZ = Random.Range(zBoundary.min, zBoundary.max);

				Vector3 newPosition = new Vector3(randX, randY, randZ);

				waypoint.position = newPosition;
			}
		}


		[System.Serializable]
		public class Boundary
		{
			public float min;
			public float max;
		}
	}
}
