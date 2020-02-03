using UnityEngine;

namespace Enemy
{
	public class EnemyWreckage : MonoBehaviour
	{
		[SerializeField] private Rigidbody[] parts;
		[SerializeField] private ParticleSystem[] smokes;

		[SerializeField] private float force;
		[SerializeField] private float torque;

		private void Start()
		{
			Size();
		}

		private void Size()
		{
			for (int i = 0; i < parts.Length; i++)
			{
				float scale = Random.Range(3, 10);
				parts[i].transform.localScale = new Vector3(scale, scale, scale);
				smokes[i].transform.localScale = new Vector3(scale, scale, scale);
			}
		}

		public void ApplyForces(Vector3 direction)
		{
			for (int i = 0; i < parts.Length; i++)
			{
				float x = Random.Range(-torque, torque);
				float y = Random.Range(-torque, torque);
				float z = Random.Range(-torque, torque);

				float f = Random.Range(force - 50f, force + 50f);

				float offsetX = Random.Range(-.1f, .1f);
				float offsetY = Random.Range(-.1f, .1f);
				float offsetZ = Random.Range(-.1f, .1f);

				direction += new Vector3(offsetX, offsetY, offsetZ);

				parts[i].AddForce(direction * f, ForceMode.Impulse);
				parts[i].AddTorque(x, y, z, ForceMode.Impulse);
			}
		}

		public void SetSmokeEmissionRate(float emissionRate, int i)
		{
			ParticleSystem.EmissionModule emission = smokes[i].emission;
			ParticleSystem.MinMaxCurve rate = emission.rateOverTime;
			rate.constantMax = emissionRate;
			emission.rateOverTime = rate;
		}
	}
}