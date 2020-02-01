using UnityEngine;
using Utilities;

namespace Weapons
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private float speed;
		[SerializeField] private int damage = 40;

		private void Start()
		{
			Destroy(this.gameObject, 5f);
			transform.parent = GameObject.Find(Tags.BULLETS).transform;
		}

		private void Update()
		{
			transform.position += transform.forward * speed * Time.deltaTime;
		}

		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.tag.Equals(Tags.TARGET))
			{
				other.gameObject.GetComponent<HealthSystem>().RemoveHealth(damage);
				Destroy(this.gameObject);
			}
		}
	}
}