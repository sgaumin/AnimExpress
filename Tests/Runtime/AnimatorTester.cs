using UnityEngine;

namespace AnimExpress
{
	[RequireComponent(typeof(AnimatorExpress))]
	public class AnimatorTester : MonoBehaviour
	{
		private AnimatorExpress animator;

		private void Awake()
		{
			animator = GetComponent<AnimatorExpress>();

			animator.AddListener("Player-Idle_4", Logging);
			animator.AddListener("Player-DeathWater_1", Logging);
		}

		private void OnDestroy()
		{
			animator.RemoveListener("Player-Idle_4", Logging);
			animator.RemoveListener("Player-DeathWater_1", Logging);
		}

		private void Logging()
		{
			Debug.Log($"Houba");
		}
	}
}
