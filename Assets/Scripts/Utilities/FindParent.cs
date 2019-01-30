using UnityEngine;
using System.Collections;

namespace Utilities
{
public class FindParent : MonoBehaviour
{
    public string parentName;

    void Start()
    {
        transform.parent = GameObject.Find(parentName).transform;
    }
}
}
