using UnityEngine;
using System.Collections;

using UnityStandardAssets.Utility;

namespace AIFighterJet
{
// This script finds a random circuit and assigns it to the WaypointProgressTracker that Unity has made. The AI Jet
// then uses this circuit as its flight path.
public class FindACircuit : MonoBehaviour 
{
    public GameObject[] circuits;

    void Start()
    {
        //script is now redundant, needs deleting

//        circuits = GameObject.FindGameObjectsWithTag("Circuit");
//
//        GetComponent<WaypointProgressTracker>().circuit = circuits[Random.Range(0, circuits.Length)].GetComponent<WaypointCircuit>();
    }
}
}
