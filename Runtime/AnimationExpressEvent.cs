using System;
using UnityEngine;

namespace AnimExpress
{
	[Serializable]
	public class AnimationExpressEvent
	{
		[SerializeField] private string eventName;
		[SerializeField, Range(0f, 1f)] private float triggerTime;
		[SerializeField] private Action action;

		public string Name => eventName;
		public float TriggerTime => triggerTime;

		public void AddListener(Action action)
		{
			this.action += action;
		}

		public void RemoveListener(Action action)
		{
			this.action -= action;
		}

		public void RemoveAllListeners()
		{
			this.action = null;
		}

		public void Invoke()
		{
			this.action?.Invoke();
		}
	}
}