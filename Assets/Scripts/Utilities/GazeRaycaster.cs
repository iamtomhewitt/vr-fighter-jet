using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
/// <summary>
/// GazeRaycaster class is used to return GameObjects, their strings, or their tags based off of
/// where/what the player is looking at.
/// 
/// For example, using GetRaycastedGameObject() would return the GameObject that the player is 
/// looking at.
/// 
/// Raycasts are blue, Spherecasts are yellow.
/// </summary>
public static class GazeRaycaster 
{
	/// <summary>
	/// Returns the GameObject that we look at, 1000f forward.
	/// </summary>
	public static GameObject GetRaycastedGameObject()
	{
		RaycastHit hit;
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000f, Color.blue);

		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f)) 
			return hit.collider.gameObject;
		else 
			return null;
	}

	/// <summary>
	/// Returns the Tag of the GameObject that we look at, 1000f forward.
	/// </summary>
	public static string GetRaycastedTag()
	{
		RaycastHit hit;
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000f, Color.blue);

		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f)) 
			return hit.collider.tag;
		else 
			return null;
	}

	/// <summary>
	/// Returns the Name of the GameObject that we look at, 1000f forward.
	/// </summary>
	public static string GetRaycastedName()
	{
		RaycastHit hit;
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000f, Color.blue);

		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f)) 
			return hit.collider.name;
		else 
			return null;
	}


	/// <summary>
	/// Creates a Sphere Raycast of size 'radius' and of length 'distance'.
	/// Returns the GameObject that comes into contact with the created Sphere Raycast.
	/// </summary>
	public static GameObject GetSphereCastedGameObject(float distance, float radius)
	{
		RaycastHit hit;
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * distance, Color.yellow);
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

		if (Physics.SphereCast (ray, radius, out hit, distance)) 
			return hit.collider.gameObject;
		else
			return null;
	}


    /// <summary>
    /// Creates a Raycast of length 'rayDistance'.
    /// Returns the distance of the object hit. If no object is hit, a value of 1 is returned.
    /// </summary>
    public static float GetRaycastHitDistance(float rayDistance)
    {
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.cyan);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance))
        {
            //Debug.Log(hit.collider.name);
            return hit.distance;
        }
        else
            return 1f;
    }
}
}
