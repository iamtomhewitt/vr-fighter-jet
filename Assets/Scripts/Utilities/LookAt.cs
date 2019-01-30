using UnityEngine;
using System.Collections;

namespace Utilities
{
public class LookAt : MonoBehaviour 
{
    public Transform target;

	void Update () 
    {
		transform.LookAt(2 * transform.position - target.position);
	}
}
}
