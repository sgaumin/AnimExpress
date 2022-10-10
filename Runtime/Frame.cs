using System;
using UnityEngine;

namespace AnimExpress
{
	[Serializable]
	public class Frame
	{
		[SerializeField] private Sprite sprite;
		[SerializeField] private float duration;

		public Sprite Sprite => sprite;
		public float Duration => duration;

		public Frame(Sprite sprite)
		{
			this.sprite = sprite;
			this.duration = 0.1f;
		}
	}
}