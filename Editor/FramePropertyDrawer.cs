using AnimExpress;
using UnityEditor;
using UnityEngine;

namespace AnimExpressEditor
{
	[CustomPropertyDrawer(typeof(Frame))]
	public class FramePropertyDrawer : PropertyDrawer
	{
		private const float DURATION_FIELD_WIDTH = 40f;
		private const float TEXTURE_FIELD_WIDTH = 60f;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var sprite = property.FindPropertyRelative("sprite");
			var duration = property.FindPropertyRelative("duration");

			Rect spriteRect = position;
			spriteRect.width = position.width - TEXTURE_FIELD_WIDTH - AnimationExpressEditorUtils.COPY_BUTTON_WIDTH - DURATION_FIELD_WIDTH;

			Rect textureRect = position;
			textureRect.width = TEXTURE_FIELD_WIDTH;
			textureRect.x += spriteRect.width;

			Rect copyButtonRect = position;
			copyButtonRect.width = AnimationExpressEditorUtils.COPY_BUTTON_WIDTH;
			copyButtonRect.x += spriteRect.width + TEXTURE_FIELD_WIDTH;

			Rect durationRect = position;
			durationRect.width = DURATION_FIELD_WIDTH;
			durationRect.x += spriteRect.width + TEXTURE_FIELD_WIDTH + AnimationExpressEditorUtils.COPY_BUTTON_WIDTH;

			EditorGUI.PropertyField(spriteRect, sprite, GUIContent.none);

			if (sprite.objectReferenceValue)
			{
				Texture texture = AssetPreview.GetAssetPreview(sprite.objectReferenceValue);
				if (texture)
				{
					GUI.DrawTexture(textureRect, texture, ScaleMode.ScaleToFit);
				}
			}

			if (GUI.Button(copyButtonRect, EditorGUIUtility.IconContent("Clipboard")))
			{
				GUIUtility.systemCopyBuffer = $"\"{sprite.objectReferenceValue.name}\"";
			}

			EditorGUI.PropertyField(durationRect, duration, GUIContent.none);
		}
	}
}