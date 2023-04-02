using AnimExpress;
using System;
using UnityEditor;
using UnityEngine;

namespace AnimExpressEditor
{
	[CustomPropertyDrawer(typeof(AnimationExpress))]
	public class AnimationExpressPropertyDrawer : PropertyDrawer
	{
		private const float COPY_BUTTON_WIDTH = 30f;

		private static string currrentAnimationPLaying;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			AnimatorExpress animator = (AnimatorExpress)property.serializedObject.targetObject;

			int index = Convert.ToInt32(property.displayName.Replace("Element ", ""));
			AnimationExpress animation = animator.Animations[index];

			Rect animationRect = position;
			animationRect.width = position.width - 2 * COPY_BUTTON_WIDTH;

			Rect playButtonRect = position;
			playButtonRect.width = COPY_BUTTON_WIDTH;
			playButtonRect.x += animationRect.width;

			Rect copyButtonRect = position;
			copyButtonRect.width = COPY_BUTTON_WIDTH;
			copyButtonRect.x += animationRect.width + COPY_BUTTON_WIDTH;

			EditorGUI.PropertyField(animationRect, property, GUIContent.none);

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