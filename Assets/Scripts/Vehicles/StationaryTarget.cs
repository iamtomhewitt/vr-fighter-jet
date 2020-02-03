namespace Vehicle
{
	/// <summary>
	/// A target that does not move.
	/// </summary>
	public class StationaryTarget : HealthSystem
	{
		public override void Die()
		{
			return;
		}
	}
}