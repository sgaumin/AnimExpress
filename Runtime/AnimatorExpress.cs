using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimExpress
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class AnimatorExpress : MonoBehaviour
	{
		[SerializeField] private List<AnimationExpress> animations = new List<AnimationExpress>();

		[Header("Settings")]
		[SerializeField] private bool playDefaultOnEnabled = true;

		[Header("References")]
		[SerializeField] protected SpriteRenderer spriteRenderer;

		private bool hasBeenInitialized;
		private Coroutine animationRoutine;
		private AnimationExpress currentAnimation;
		private Dictionary<string, AnimationExpress> declaredAnimations;
		private Dictionary<string, Action> declaredAnimationEvents;

		public List<AnimationExpress> Animations => animations;
		public AnimationExpress Current => currentAnimation;
		public bool IsBeingTested { get; set; }

		private void Reset()
		{
			try
			{
				if (!Application.isPlaying)
				{
					if (spriteRenderer != null)
						spriteRenderer = GetComponent<SpriteRenderer>();

					spriteRenderer.sprite = animations[0].Frames[0].Sprite;
				}
			}
			catch (System.Exception) { }
		}

		private void OnEnable()
		{
			if (playDefaultOnEnabled)
				PlayDefault();
		}

		private void OnDisable()
		{
			Stop();
		}

		private void Init()
		{
			hasBeenInitialized = true;
			declaredAnimations = new Dictionary<string, AnimationExpress>(animations.Count);
			declaredAnimationEvents = new Dictionary<string, Action>();
			foreach (AnimationExpress animation in animations)
			{
				declaredAnimations.Add(animation.name, animation);
			}
		}

		private void PlayDefault()
		{
			CheckInitialization();

			if (animations.Count == 0 || currentAnimation == animations[0]) return;

			currentAnimation = animations[0]; // First animation is default

			PlayRoutine();
		}

		public void PlayTesting(string animationKey = "")
		{
			IsBeingTested = true;
			DoPlay(animationKey);
		}

		private void DoPlay(string animationKey, bool reversed = false)
		{
			CheckInitialization();

			if (string.IsNullOrEmpty(animationKey))
			{
				Debug.LogError($"Empty animation detected for {gameObject.name}");
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

			PlayRoutine(reversed);
		}

		public void Play(string animationKey = "")
		{
			if (IsBeingTested) return;
			DoPlay(animationKey);
		}

		public void PlayReversed(string animationKey = "")
		{
			if (IsBeingTested) return;
			DoPlay(animationKey, true);
		}

		public void Stop()
		{
			CheckInitialization();

			StopRoutine();
			currentAnimation = null;
		}

		public void AddListener(string frameName, Action action)
		{
			CheckInitialization();

			Action frameEvent;
			if (declaredAnimationEvents.TryGetValue(frameName, out frameEvent))
			{
				frameEvent += action;
			}
			else
			{
				frameEvent += action;
				declaredAnimationEvents.Add(frameName, frameEvent);
			}
		}

		public void RemoveListener(string frameName, Action action)
		{
			CheckInitialization();

			if (declaredAnimationEvents.TryGetValue(frameName, out Action frameEvent))
			{
				frameEvent -= action;
			}
		}

		private void PlayRoutine(bool reversed = false)
		{
			StopRoutine();
			animationRoutine = StartCoroutine(PlayCore(reversed));
		}

		private void StopRoutine()
		{
			if (animationRoutine != null)
			{
				StopCoroutine(animationRoutine);
			}
			animationRoutine = null;
		}

		private void CheckInitialization()
		{
			if (!hasBeenInitialized)
			{
				Init();
			}
		}

		private IEnumerator PlayCore(bool reversed = false)
		{
			int currentIndex = 0;
			float currentDuration = 0f;
			float currentStartTime = Time.time;
			Frame currentFrame = null;
			List<Frame> frames = currentAnimation.Frames;
			bool hasTriggeredEvent = false;

			if (reversed)
				frames.Reverse();

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
					}

					currentFrame = frames[currentIndex++];
					spriteRenderer.sprite = currentFrame.Sprite;
					currentDuration = Time.time + currentFrame.Duration / currentAnimation.SpeedFactor;
					hasTriggeredEvent = false;
				}

				// Triggering event attached to this frame
				if (!hasTriggeredEvent)
				{
					hasTriggeredEvent = true;
					if (declaredAnimationEvents.TryGetValue(currentFrame.Sprite.name, out Action frameEvent))
					{
						frameEvent?.Invoke();
					}
				}

				yield return null;
			}

			switch (currentAnimation.OnCompletionOption)
			{
				case AnimationExpressCompletionOptions.PlayDefaultAnimation:
					PlayDefault();
					break;

				case AnimationExpressCompletionOptions.DestroyGameObject:
					Destroy(gameObject);
					break;

				default:
					break;
			}

			// In case we were testing an animation here, we reset its testing state.
			if (IsBeingTested)
				IsBeingTested = false;
		}
	}
}