using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace AnimExpress
{
	public static class AnimationExpressCreatorTool
	{
		private const string SUBFODLER = "Animations";

		[MenuItem("CONTEXT/TextureImporter/Create Animations", priority = 1000)]
		private static void CopySettings(MenuCommand command)
		{
			TextureImporter textureImporter = (TextureImporter)command.context;
			string path = AssetDatabase.GetAssetPath(textureImporter);
			string folderPath = Path.GetDirectoryName(path);
			string fileName = Path.GetFileName(path).Replace(".png", "");

			string newFodlerPath = Path.Combine(folderPath, SUBFODLER);
			if (!Directory.Exists(newFodlerPath))
			{
				Directory.CreateDirectory(newFodlerPath);
			}

			string newAssetName = Path.Combine(newFodlerPath, $"{fileName}.asset");
			var existingAsset = AssetDatabase.LoadAssetAtPath<AnimationExpress>(newAssetName);
			if (existingAsset is not null) return;

			UpdateSpriteName(command);

			Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
			sprites = sprites.OrderBy(x => x.name).ToArray();

			List<Frame> frames = new List<Frame>();
			for (int i = 0; i < sprites.Length; i++)
			{
				Frame frame = new Frame(sprites[i]);
				frames.Add(frame);
			}
			AnimationExpress animClip = new AnimationExpress(frames);

			AssetDatabase.CreateAsset(animClip, newAssetName);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		[MenuItem("CONTEXT/TextureImporter/Update Sprite Name", priority = 999)]
		private static void UpdateSpriteName(MenuCommand command)
		{
			TextureImporter textureImporter = (TextureImporter)command.context;
			string path = AssetDatabase.GetAssetPath(textureImporter);
			string fileName = Path.GetFileName(path).Replace(".png", "");

			SpriteMetaData[] spritesheet = textureImporter.spritesheet;
			for (int i = 0; i < spritesheet.Length; i++)
			{
				spritesheet[i].name = $"{fileName}_{i}";
			}
			textureImporter.spritesheet = spritesheet;

			EditorUtility.SetDirty(textureImporter);
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
			textureImporter.SaveAndReimport();
		}
	}
}

#endif