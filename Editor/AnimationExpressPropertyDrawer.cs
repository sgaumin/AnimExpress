using AnimExpress;
using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace AnimExpressEditor
{
	[CustomPropertyDrawer(typeof(AnimationExpress))]
	public class AnimationExpressPropertyDrawer : PropertyDrawer
	{
		private const float COPY_BUTTON_WIDTH = 30f;

		private static string currrentAnimationPLaying;

		private AnimFloat progress;
		private float startTime;
		private float endTime;
		private bool initizalized;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public void RepaintInspector(SerializedObject BaseObject)
		{
			foreach (var item in ActiveEditorTracker.sharedTracker.activeEditors)
				if (item.serializedObject == BaseObject)
				{ item.Repaint(); return; }
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (progress == null)
			{
				progress = new AnimFloat(0f);
				progress.valueChanged.AddListener(() => RepaintInspector(property.serializedObject));
			}

			AnimatorExpress animator = (AnimatorExpress)property.serializedObject.targetObject;
			AnimationExpress playingAnimation = animator.Current;

			int index = Convert.ToInt32(property.displayName.Replace("Element ", ""));
			AnimationExpress animation = animator.Animations[index];

			// Check if this animation is playing
			if (playingAnimation == animation)
			{
				if (!initizalized || progress.value == 1f)
				{
					if (!initizalized) initizalized = true;

					startTime = Time.time;
					endTime = animation.TotalDuration;
				}

				progress.value = Mathf.Clamp01((Time.time - startTime) / endTime);
			}
			else
			{
				if (initizalized) initizalized = false;
				progress.value = 0f;
			}

			Rect animationRect = position;
			animationRect.width = position.width - 2 * COPY_BUTTON_WIDTH;

			Rect animationProgressRect = position;
			animationProgressRect.width = animationRect.width * progress.value;

			Rect playButtonRect = position;
			playButtonRect.width = COPY_BUTTON_WIDTH;
			playButtonRect.x += animationRect.width;

			Rect copyButtonRect = position;
			copyButtonRect.width = COPY_BUTTON_WIDTH;
			copyButtonRect.x += animationRect.width + COPY_BUTTON_WIDTH;

			EditorGUI.PropertyField(animationRect, property, GUIContent.none);

			Color progressColor = new Color(0f, 1f, 1f, 0.2f);
			EditorGUI.DrawRect(animationProgressRect, progressColor);

			// Checking if we have cached testing info after testing is completed
			if (!animator.IsBeingTested && !string.IsNullOrEmpty(currrentAnimationPLaying))
			{
				currrentAnimationPLaying = "";
			}

			EditorGUI.BeginDisabledGroup(!Application.isPlaying);
			if (animation.name == currrentAnimationPLaying)
			{
				if (GUI.Button(playButtonRect, EditorGUIUtility.IconContent("d_PauseButton@2x")))
				{
					animator.IsBeingTested = false;
					currrentAnimationPLaying = "";
				}
			}
			else
			{
				if (GUI.Button(playButtonRect, EditorGUIUtility.IconContent("d_PlayButton@2x")))
				{
					animator.PlayTesting(animation.name);
					currrentAnimationPLaying = animation.name;
				}
			}
			EditorGUI.EndDisabledGroup();

			if (GUI.Button(copyButtonRect, EditorGUIUtility.IconContent("Clipboard")))
			{
				GUIUtility.systemCopyBuffer = $"\"{property.objectReferenceValue.name}\"";
			}
		}
	}
}