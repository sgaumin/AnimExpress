using AnimExpress;
using UnityEditor;
using UnityEngine;

namespace AnimExpressEditor
{
	[CustomEditor(typeof(AnimationExpress))]
	public class AnimationExpressCustomEditor : Editor
	{
		private AnimationExpress context;

		public void OnEnable()
		{
			context = (AnimationExpress)target;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SerializedProperty isLooping = serializedObject.FindProperty("isLooping");
			EditorGUILayout.PropertyField(isLooping);
			if (!isLooping.boolValue)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onCompletionOption"));
			}

			EditorGUILayout.Space(16f);

			EditorGUILayout.LabelField($"Total Duration: {context.TotalDuration.ToString("0.00")}s", EditorStyles.boldLabel);
			SerializedProperty speedFactor = serializedObject.FindProperty("speedFactor");
			EditorGUILayout.PropertyField(speedFactor);
			if (speedFactor.floatValue == 0f)
			{
				speedFactor.floatValue = 1f;
			}
			else
			{
				speedFactor.floatValue = Mathf.Max(speedFactor.floatValue, 0.01f);
			}
			EditorGUILayout.PropertyField(serializedObject.FindProperty("frames"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("events"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}