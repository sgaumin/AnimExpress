﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnimExpress
{
	[CreateAssetMenu(fileName = "AnimationExpress", menuName = "AnimationExpress", order = 1)]
	public class AnimationExpress : ScriptableObject
	{
		[SerializeField] private bool isLooping = true;
		[SerializeField] private bool canBeRestarted;
		[SerializeField] private AnimationExpressCompletionOptions onCompletionOption = AnimationExpressCompletionOptions.PlayDefaultAnimation;
		[SerializeField] private string methodName;
		[SerializeField] private float speedFactor = 1f;
		[SerializeField] private List<Frame> frames;

		public bool IsLooping => isLooping;
		public bool CanBeRestarted => canBeRestarted;
		internal AnimationExpressCompletionOptions OnCompletionOption => onCompletionOption;
		public string MethodName => methodName;
		public float SpeedFactor => speedFactor;
		public List<Frame> Frames => frames;
		public float TotalDuration => frames.Sum(x => x.Duration) / speedFactor;

		public AnimationExpress(List<Frame> frames)
		{
			this.frames = frames;
		}

#if UNITY_EDITOR

		public void AddFrame(Sprite sprite)
		{
			var frame = new Frame(sprite);
			frames.Add(frame);
		}

#endif
	}
}