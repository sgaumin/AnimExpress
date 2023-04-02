using AnimExpress;
using UnityEditor;
using UnityEngine;

namespace AnimExpressEditor
{
	[CustomPropertyDrawer(typeof(Frame))]
	public class FramePropertyDrawer : PropertyDrawer
	{
		private const float DURATION_FIELD_WIDTH = 40f;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var sprite = property.FindPropertyRelative("sprite");
			var duration = property.FindPropertyRelative("duration");

			Rect spriteRect = position;
			spriteRect.width = position.width - DURATION_FIELD_WIDTH;
			Rect durationRect = position;
			durationRect.width = DURATION_FIELD_WIDTH;
			durationRect.x += spriteRect.width;
			EditorGUI.PropertyField(spriteRect, sprite, GUIContent.none);
			EditorGUI.PropertyField(durationRect, duration, GUIContent.none);
		}
	}
}