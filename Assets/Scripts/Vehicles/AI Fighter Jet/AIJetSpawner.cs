using UnityEngine;
using System.Collections;

namespace AIFighterJet
{
public class AIJetSpawner : MonoBehaviour 
{
	public GameObject jet;
	public Vector3 boundary;
	public int maxJets;
	public float spawnDelay;
    public float spawnRate;
	public int numberOfJets;
	
	void Start()
	{
        InvokeRepeating("SpawnJet", spawnDelay, spawnRate);
        InvokeRepeating("CountNumberOfJets", 0f, 0.2f);
    }
	
	void CountNumberOfJets()
	{
		numberOfJets = GameObject.FindGameObjectsWithTag("AI Jet").Length;
	}
	
	void SpawnJet()
	{
        if (numberOfJets < maxJets)
		{
			GameObject j = Instantiate( jet,
                                        new Vector3(Random.Range(-boundary.x, boundary.x),
										            Random.Range(4000f, boundary.y),
										            Random.Range(-boundary.z, boundary.z)),
                                        Quaternion.identity) as GameObject;		
            
			j.transform.parent = GameObject.Find ("Jets").transform;
		}
	}
}
}
