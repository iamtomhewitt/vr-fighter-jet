using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthSystem : MonoBehaviour
{
	public int health;
	public bool godMode;

	/// <summary>
	/// Removes health from the health variable.
	/// All other things done when health is removed is done in DecreaseHealth();
	/// </summary>
	public void RemoveHealth(int amount)
	{
		if (godMode)
			return;

		health -= amount;

		if (health <= 0)
		{
			print(this.gameObject.name + " has been killed.");
			this.Destroy();
		}
	}

	public abstract void Destroy();
}
