using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AnimExpress
{
	[RequireComponent(typeof(SpriteRenderer), typeof(AnimatorExpressTester))]
	public class AnimatorExpress : MonoBehaviour
	{
		[SerializeField] private List<AnimationExpress> animations = new List<AnimationExpress>();

		[Header("References")]
		[SerializeField] protected SpriteRenderer spriteRenderer;
		[SerializeField] protected AnimatorExpressTester animatorExpressTester;

		private bool hasBeenInitialized;
		private Coroutine animationRoutine;
		private AnimationExpress currentAnimation;
		private Dictionary<string, AnimationExpress> declaredAnimations;
		private Dictionary<string, Dictionary<string, AnimationExpressEvent>> declaredAnimationEvents;

		public List<AnimationExpress> Animations => animations;

		private void OnValidate()
		{
			try
			{
				if (!Application.isPlaying)
				{
					if (spriteRenderer is null)
					{
						spriteRenderer = GetComponent<SpriteRenderer>();
					}
					if (animatorExpressTester is null)
					{
						animatorExpressTester = GetComponent<AnimatorExpressTester>();
					}
					spriteRenderer.sprite = animations[0].Frames[0].Sprite;
				}
			}
			catch (System.Exception) { }
		}

		private void Awake()
		{
			CheckInitialization();
		}

		private void Start()
		{
			PlayDefault();
		}

		private void Init()
		{
			hasBeenInitialized = true;
			declaredAnimations = new Dictionary<string, AnimationExpress>(animations.Count);
			declaredAnimationEvents = new Dictionary<string, Dictionary<string, AnimationExpressEvent>>();
			foreach (AnimationExpress animation in animations)
			{
				declaredAnimations.Add(animation.name, animation);

				var d = new Dictionary<string, AnimationExpressEvent>();
				foreach (AnimationExpressEvent e in animation.Events)
				{
					d.Add(e.Name, e);
				}
				declaredAnimationEvents.Add(animation.name, d);
			}
		}

#if UNITY_EDITOR

		public void AddAnimation(AnimationExpress animation)
		{
			animations.Add(animation);
		}

#endif

		private void PlayDefault()
		{
			CheckInitialization();

			if (animations.Count == 0 || currentAnimation == animations[0]) return;

			currentAnimation = animations[0]; // First animation is default

			PlayRoutine();
		}

		public void PlayTesting(string animationKey = "")
		{
			DoPlay(animationKey);
		}

		public void Play(string animationKey = "")
		{
			if (animatorExpressTester.IsTakingControls) return;
			DoPlay(animationKey);
		}

		private void DoPlay(string animationKey)
		{
			CheckInitialization();

			if (string.IsNullOrEmpty(animationKey))
			{
				Debug.LogError($"Animation empty detected for {gameObject.name}");
				PlayDefault();
			}
			else if (declaredAnimations.TryGetValue(animationKey, out AnimationExpress animation))
			{
				if (currentAnimation == animation) return;
				currentAnimation = animation;
			}
			else
			{
				Debug.LogError($"Animation {animationKey} not found for {gameObject.name}");
			}

			PlayRoutine();
		}

		private void PlayRoutine()
		{
			if (animationRoutine is not null)
			{
				StopCoroutine(animationRoutine);
			}
			animationRoutine = StartCoroutine(PlayCore());
		}

		private void CheckInitialization()
		{
			if (!hasBeenInitialized)
			{
				Init();
			}
		}

		private IEnumerator PlayCore()
		{
			int currentIndex = 0;
			float currentDuration = 0f;
			float currentStartTime = 0f;
			List<Frame> frames = currentAnimation.Frames;
			List<AnimationEventChecker> events = new List<AnimationEventChecker>();
			foreach (var e in currentAnimation.Events)
			{
				var a = new AnimationEventChecker(e);
				a.SetupTrigger(currentAnimation);
				events.Add(a);
			}

			while (true)
			{
				if (Time.time >= currentDuration)
				{
					if (currentIndex >= frames.Count)
					{
						if (currentAnimation.IsLooping)
						{
							currentIndex = 0;
							currentStartTime = Time.time;
						}
						else
						{
							break;
						}

						events.ForEach(x => x.hasBeenTriggered = false);
					}

					Frame currentFrame = frames[currentIndex++];
					spriteRenderer.sprite = currentFrame.Sprite;
					currentDuration = Time.time + currentFrame.Duration / currentAnimation.SpeedFactor;
				}

				foreach (var e in events)
				{
					float triggerTime = e.animationEvent.TriggerTime + currentStartTime;
					if (triggerTime <= Time.time && !e.hasBeenTriggered)
					{
						e.hasBeenTriggered = true;
						e.animationEvent.Invoke();
					}
				}

				yield return null;
			}

			if (currentAnimation.PlayDefaultOnCompletion)
			{
				PlayDefault();
			}
		}

		public void AddListener(string animationName, string eventName, Action action)
		{
			if (declaredAnimationEvents.TryGetValue(animationName, out var events))
			{
				if (events.TryGetValue(eventName, out var e))
				{
					e.AddListener(action);
				}
				else
				{
					Debug.LogError($"Event {eventName} on animation {animationName} not found for {gameObject.name}");
				}
			}
			else
			{
				Debug.LogError($"Animation {animationName} not found for {gameObject.name}");
			}
		}

		public void RemoveListener(string animationName, string eventName, Action action)
		{
			if (declaredAnimationEvents.TryGetValue(animationName, out var events))
			{
				if (events.TryGetValue(eventName, out var e))
				{
					e.RemoveListener(action);
				}
			}
		}
	}
}