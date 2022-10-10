using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnimExpress
{
	[CreateAssetMenu(fileName = "AnimationExpress", menuName = "AnimationExpress", order = 1)]
	public class AnimationExpress : ScriptableObject
	{
		[SerializeField] private bool isLooping = true;
		[SerializeField] private bool playDefaultOnCompletion = true;
		[SerializeField] private float speedFactor = 1f;
		[SerializeField] private List<Frame> frames;
		[SerializeField] private List<AnimationExpressEvent> events;

		public bool IsLooping => isLooping;
		public bool PlayDefaultOnCompletion => playDefaultOnCompletion;
		public float SpeedFactor => speedFactor;
		public List<Frame> Frames => frames;
		public List<AnimationExpressEvent> Events => events;
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