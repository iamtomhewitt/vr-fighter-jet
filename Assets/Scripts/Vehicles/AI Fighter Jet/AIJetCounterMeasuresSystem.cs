using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIFighterJet
{
public class AIJetCounterMeasuresSystem : MonoBehaviour 
{
    public GameObject countermeasure;
    public LockOnGraphic lockOnGraphic;
    public Transform spawn;
    public int countermeasureCount;
	public int repeatRate;
	public float deploySpeed;

	void Start () 
    {
		InvokeRepeating("FireCounterMeasure", 0f, Random.Range(10f, repeatRate));
	}

    void FireCounterMeasure()
    {
        StartCoroutine(SpawnCountermeasure());
    }
	
    IEnumerator SpawnCountermeasure()
    {
        for (int i = 0; i <= countermeasureCount; i++)
        {
            GameObject c = Instantiate(countermeasure, spawn.position, Quaternion.identity) as GameObject;
            Destroy(c, 6f);
            yield return new WaitForSeconds(deploySpeed);
        }
    }
}
}
