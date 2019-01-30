using UnityEngine;
using System.Collections;
using AIFighterJet;

namespace Vehicle
{
    public class VehicleHealthSystem : MonoBehaviour
    {
        public GameObject explosion;
        [Tooltip("Only used for AI Jets")]
        public GameObject wreckage;
        public float health = 100;
        public bool isAIJet;

        public void DecreaseHealth(float amount)
        {
            health -= amount;

            if (health <= 0)
            {
                GameObject e = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                Destroy(e, 15f);

                GameObject w = Instantiate(wreckage, transform.position, transform.rotation) as GameObject;
                w.GetComponent<AIJetWreckage>().ApplyForces(transform.forward);

                Destroy(this.gameObject);
            }
        }

        void OnCollisionEnter(Collision other)
        {
            if (!isAIJet)
                return;

            if (other.gameObject.tag == "Environment")
            {
                DecreaseHealth(200f);
            }
        }
    }
}
