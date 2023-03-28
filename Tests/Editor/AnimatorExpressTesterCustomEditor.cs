using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AnimExpress
{
	[CustomEditor(typeof(AnimatorExpressTester))]
	public class AnimatorExpressTesterCustomEditor : Editor
	{
		private AnimatorExpressTester context;

		public void OnEnable()
		{
			context = (AnimatorExpressTester)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (context is not null && context.Animator is not null && context.Animator.Animations is not null)
			{
				EditorGUI.BeginDisabledGroup(!Application.isPlaying);
				foreach (AnimationExpress item in context.Animator.Animations)
				{
					try
					{
						if (item is null) continue;

						if (GUILayout.Button(item.name))
						{
							context.IsTakingControls = true;
							context.Animator.PlayTesting(item.name);
						}
					}
					catch (Exception)
					{
					}
				}

				if (context.Animator.Animations.Count > 0)
				{
					GUILayout.Space(16f);
					EditorGUI.BeginDisabledGroup(!context.IsTakingControls);
					if (GUILayout.Button("Release Control"))
					{
						context.IsTakingControls = false;
					}
					EditorGUI.EndDisabledGroup();
				}
				EditorGUI.EndDisabledGroup();

				if (context.Animator.Animations.Count == 0)
				{
					string filteredName = $"{context.gameObject.name}-";
					if (GUILayout.Button($"Import animations starting with \"{filteredName}\""))
					{
						string[] guids = AssetDatabase.FindAssets($"t:AnimationExpress {filteredName}");
						if (guids.Length == 0)
						{
							Debug.LogError($"No animations assets found starting with \"{filteredName}\"");
						}

						foreach (string guid in guids)
						{
							context.Animator.AddAnimation(AssetDatabase.LoadAssetAtPath<AnimationExpress>(AssetDatabase.GUIDToAssetPath(guid)));
						}
					}
				}
			}
		}
	}
}