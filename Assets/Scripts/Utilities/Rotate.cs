using UnityEngine;
using System.Collections;
namespace Utilities
{
public class Rotate : MonoBehaviour 
{
	public Vector3 speed;
	
	void Update()
	{
		transform.Rotate(speed);
	}
}
}
