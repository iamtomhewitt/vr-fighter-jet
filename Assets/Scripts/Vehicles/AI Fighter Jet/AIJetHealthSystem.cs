using UnityEngine;
using System.Collections;
using AIFighterJet;

namespace AIFighterJet
{
    public class AIJetHealthSystem : HealthSystem
    {
        public GameObject explosion;
        public GameObject wreckage;

		public override void Destroy()
		{
			GameObject e = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
			Destroy(e, 15f);

			//GameObject w = Instantiate(wreckage, transform.position, transform.rotation) as GameObject;
			//w.GetComponent<AIJetWreckage>().ApplyForces(transform.forward);

			Destroy(this.gameObject);
		}

		void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag.Equals("Environment"))
            {
				this.RemoveHealth(200);
            }
        }
    }
}
