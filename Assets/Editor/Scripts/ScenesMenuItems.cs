using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ScenesMenuItems
{
    public const string k_SetupAssetPath = "Assets/Editor/SceneSetup.asset";

    [MenuItem("Scenes/Save Scene Manager Setup")]
    public static void SavesSetup()
    {
        var asset = ScriptableObject.CreateInstance<SceneManagerSetupAsset>();
        asset.SceneSetup = EditorSceneManager.GetSceneManagerSetup();
        AssetDatabase.CreateAsset(asset, k_SetupAssetPath);
    }

    [MenuItem("Scenes/Restore Scene Manager Setup")]
    public static void RestoreSetup()
    {
        var asset = AssetDatabase.LoadAssetAtPath<SceneManagerSetupAsset>(k_SetupAssetPath);
        EditorSceneManager.RestoreSceneManagerSetup(asset.SceneSetup);
    }
}
