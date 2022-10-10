using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace AnimExpress
{
	[CustomPropertyDrawer(typeof(AnimationExpress))]
	public class AnimationExpressPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Rect p1 = position;
			p1.width = position.width * 0.9f;
			Rect p2 = position;
			p2.width = position.width * 0.1f;
			p2.x += position.width * 0.9f;
			EditorGUI.PropertyField(p1, property, GUIContent.none);
			if (GUI.Button(p2, "Copy"))
			{
				GUIUtility.systemCopyBuffer = $"\"{property.objectReferenceValue.name}\"";
			}
		}
	}
}