using UnityEngine;
using System.Collections;

namespace Utilities
{
public class RotateAround : MonoBehaviour 
{
    public Transform jet;
    public float speed, amplitude, frequency;

    void Update()
    {
        transform.LookAt(jet);
        transform.Translate(Vector3.right * Time.deltaTime * speed);

        transform.position += amplitude*(Mathf.Sin(2*Mathf.PI*frequency*Time.time) - Mathf.Sin(2*Mathf.PI*frequency*(Time.time - Time.deltaTime)))*transform.up;
    }

}
}
