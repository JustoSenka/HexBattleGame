using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class BuildSkillTypeEnum
    {
        public const string k_ScriptPath = "Assets/GameLogic/Entities/SkillType-gen.cs";
        public const string k_ScriptTemplate = @"
// Generated file by: Tools/Build/Skill Enum
// Maps enum for highlighters to correct material index in PublicReferences component in Managers scene

using System;

namespace Assets
{{
    [Flags]
    public enum SkillType
    {{
{0}
    }}
}}";
        [MenuItem("Tools/Build/Build Skill Enum")]
        public static void BuildSkillTypeEnumMap()
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
            b.AppendLine($"        None = 0,");
            b.AppendLine($"        Attack = 1,");

            var i = 1;
            foreach (var skill in components[0].SkillDB.Skills)
            {
                var enumName = skill.Name.Replace("-", "_").Replace(" ", "_");
                b.AppendLine($"        {enumName} = 1 << {i++},");
            }

            File.WriteAllText(k_ScriptPath, string.Format(k_ScriptTemplate, b.ToString()));
            AssetDatabase.Refresh();
        }
    }
}
