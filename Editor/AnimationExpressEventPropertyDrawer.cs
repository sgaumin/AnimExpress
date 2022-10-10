using UnityEditor;
using UnityEngine;

namespace AnimExpress
{
	[CustomPropertyDrawer(typeof(AnimationExpressEvent))]
	public class AnimationExpressEventPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var nameProperty = property.FindPropertyRelative("eventName");
			var triggerTime = property.FindPropertyRelative("triggerTime");
			Rect p1 = position;
			p1.width = position.width * 0.4f;
			Rect p2 = position;
			p2.width = position.width * 0.46f;
			p2.x += position.width * 0.42f;
			Rect p3 = position;
			p3.width = position.width * 0.1f;
			p3.x += position.width * 0.9f;
			EditorGUI.PropertyField(p1, nameProperty, GUIContent.none);
			EditorGUI.PropertyField(p2, triggerTime, GUIContent.none);
			if (GUI.Button(p3, "Copy"))
			{
				GUIUtility.systemCopyBuffer = $"\"{nameProperty.stringValue}\"";
			}
		}
	}
}