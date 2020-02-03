using UnityEngine;

namespace Vehicle
{
	/// <summary>
	/// A health system that can be attached to any object that requires health that can be decreased.
	/// </summary>
	public abstract class HealthSystem : MonoBehaviour
	{
		[SerializeField] private int health;
		[SerializeField] private bool godMode;

		/// <summary>
		/// Removes health from the health variable.
		/// </summary>
		public void RemoveHealth(int amount)
		{
			if (godMode)
				return;

			health -= amount;

			if (health <= 0)
			{
				print(this.gameObject.name + " has died");
				this.Die();
			}
		}

		public abstract void Die();
	}
}