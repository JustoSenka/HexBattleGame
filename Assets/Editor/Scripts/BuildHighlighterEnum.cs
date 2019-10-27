using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class BuildHighlighterEnum
    {
        public const string k_ScriptPath = "Assets/GameLogicUnity/Scripts/Grid/Highlighter-gen.cs";
        public const string k_ScriptTemplate = @"
// Generated file by: Scenes/Build Highlighter Material Mappings
// Maps enum for highlighters to correct material index in PublicReferences component in Managers scene

namespace Assets
{{
    public enum Highlighter
    {{
{0}
    }}
}}";
        [MenuItem("Tools/Build/Build Highlighter Material Mappings")]
        public static void BuildHighlighterEnumMaps()
        {
            var components = GameObject.FindObjectsOfType<PublicReferences>();

            if (components.Length > 1)
                Debug.LogWarning("There are multiple PublicReferences components in loaded scenes. Generated 'enum Highlighter' might be incorrect");

            if (components.Length == 0)
            {
                Debug.LogWarning("There are no PublicReferences components in loaded scenes. Cannot generate 'enum Highlighter'");
                return;
            }


            var b = new StringBuilder();
            var i = 0;
            foreach (var mat in components[0].HighlightMaterials)
            {
                var enumName = mat.name.Replace("-", "_").Replace("hex_", "").Replace("outline2_", "");
                b.AppendLine($"        {enumName} = {i++},");
            }

            File.WriteAllText(k_ScriptPath, string.Format(k_ScriptTemplate, b.ToString()));
            AssetDatabase.Refresh();
        }
    }
}
