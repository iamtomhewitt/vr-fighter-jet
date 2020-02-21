using UnityEngine;
using Utilities;
using Vehicle;

namespace Enemy
{
    public class EnemyHealth : HealthSystem
    {
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject wreckage;

		public override void Die()
		{
			GameObject e = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
			Destroy(e, 15f);

			GameObject w = Instantiate(wreckage, transform.position, transform.rotation) as GameObject;
			w.GetComponent<EnemyWreckage>().ApplyForces(transform.forward);

			Destroy(this.gameObject);
		}

		private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag.Equals(Tags.ENVIRONMENT))
            {
				this.RemoveHealth(200);
            }
        }
    }
}