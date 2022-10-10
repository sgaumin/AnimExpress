using UnityEngine;

namespace AnimExpress
{
	public class AnimatorExpressTester : MonoBehaviour
	{
		public AnimatorExpress Animator => GetComponent<AnimatorExpress>();
		public bool IsTakingControls { get; set; }
	}
}