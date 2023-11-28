using UnityEditor;
using UnityEngine;

namespace WarriorAnims
{
	internal class SetupInputLayers:AssetPostprocessor
	{
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			SetupMessageWindow window = ( SetupMessageWindow )EditorWindow.GetWindow(typeof(SetupMessageWindow), true, "Load Input and TagLayer Presets");
			window.maxSize = new Vector2(315f, 130f);
			window.minSize = window.maxSize;
		}
	}

	public class SetupMessageWindow:EditorWindow
	{
		private void OnGUI()
		{
			EditorGUILayout.LabelField("Before attempting to use the Warrior Animation Mecanim Animation Packs, you must first ensure that the layers and inputs are correctly defined.  There are included Input.Presets and TagLayer.Presets which contain all the settings that you can load in.\n \nYou can remove this message by deleting the SetupInputLayers.cs script in the Editor folder.", EditorStyles.wordWrappedLabel);
		}
	}
}