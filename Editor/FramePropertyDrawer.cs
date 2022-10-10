using UnityEditor;
using UnityEngine;

namespace AnimExpress
{
	[CustomPropertyDrawer(typeof(Frame))]
	public class FramePropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var sprite = property.FindPropertyRelative("sprite");
			var duration = property.FindPropertyRelative("duration");
			Rect p1 = position;
			p1.width = p1.width * 0.8f;
			Rect p2 = position;
			p2.width = p2.width * 0.2f;
			p2.x += p1.width;
			EditorGUI.PropertyField(p1, sprite, GUIContent.none);
			EditorGUI.PropertyField(p2, duration, GUIContent.none);
		}
	}
}