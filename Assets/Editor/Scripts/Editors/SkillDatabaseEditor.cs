using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(SkillDatabaseData))]
    public class SkillDatabaseEditor : DatabaseEditor
    {
        private bool m_ShowEffects = true;

        void OnEnable() { }

        public (string Label, int Width, Type type)[] SkillDetails = new[]
        {
            ("ID", 0, typeof(void)),
            ("Name", 100, typeof(string)),
            ("MagicReq", 0, typeof(int)),
            ("Cooldown", 0, typeof(int)),
            ("Duration", 0, typeof(int)),
            ("AOE", 0, typeof(int)),
            ("RangeMin", 0, typeof(int)),
            ("RangeMax", 0, typeof(int)),
        };

        public (string Label, int Width, Type type)[] SkillEffectDetails = new[]
        {
            ("ID", 0, typeof(void)),
            ("PropertyName", 0, typeof(Enum)),
            ("Amount", 0, typeof(int)),
            ("ShouldSet", 0, typeof(bool)),
            ("IsPermament", 0, typeof(bool)),
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            var DB = (SkillDatabaseData)serializedObject.targetObject;
            PrintLabelGUI(SkillDetails);

            var skillArray = DB.Skills.ToArray();
            for (int i = 0; i < skillArray.Length; i++)
            {
                var skill = MakeSureSkillHasCorrectIDAndNotNullEffectList(DB, skillArray[i], i);

                var (arrayModified, _) = PrintObjectGUI(SkillDetails, DB.Skills, skill, i, true);
                if (arrayModified)
                    return;

                var effectsArray = skill.Effects.ToArray();
                if (!m_ShowEffects)
                {
                    for (int j = 0; j < effectsArray.Length; j++)
                    {
                        var effect = effectsArray[j];
                        effect.ID = j;

                        PrintObjectGUI(SkillEffectDetails, skill.Effects, effect, j, false, true);
                    }

                    GUILayout.Space(20);
                }
            }

            if (GUILayout.Button("New Skill"))
                DB.Skills.Add(new Skill());

            m_ShowEffects = GUILayout.Toggle(m_ShowEffects, "Show Effects");

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save & Generate Scripts"))
            {
                BuildSkillTypeEnumMap(DB);
                DB.Save();
            }
        }

        private static Skill MakeSureSkillHasCorrectIDAndNotNullEffectList(SkillDatabaseData DB, Skill skill, int i)
        {
            skill.ID = i;

            if (skill.Effects == null)
                skill.Effects = new List<SkillEffect>();

            if (skill.Effects.Count == 0)
                skill.Effects.Add(new SkillEffect());

            DB.Skills[i] = skill;

            return skill;
        }

        public const string k_ScriptPath = "Assets/GameLogic/Entities/SkillType-gen.cs";
        public const string k_ScriptTemplate = @"
// Generated file by: Tools/Build/Skill Enum
// Maps enum for highlighters to correct material index in PublicReferences component in Managers scene

using System;
using System.Collections.Generic;

namespace Assets
{{
    [Flags]
    public enum SkillType
    {{
{0}
    }}

    public static class SkillTypeIDs
    {{
        public static Dictionary<int, SkillType> IdToSkillType = new Dictionary<int, SkillType>
        {{
{1}
        }};

        public static Dictionary<SkillType, int> SkillTypeToId = new Dictionary<SkillType, int>
        {{
{2}
        }};
    }}
}}";
        public static void BuildSkillTypeEnumMap(SkillDatabaseData DB)
        {
            var b0 = new StringBuilder();
            b0.AppendLine($"        None = 0,");
            b0.AppendLine($"        Attack = 1,");

            var b1 = new StringBuilder();
            b1.AppendLine($"            {{ 0, SkillType.None }},");
            b1.AppendLine($"            {{ 1, SkillType.Attack }},");

            var b2 = new StringBuilder();
            b2.AppendLine($"            {{ SkillType.None, 0 }},");
            b2.AppendLine($"            {{ SkillType.Attack, 1 }},");

            var i = 1;
            foreach (var skill in DB.Skills)
            {
                var enumName = skill.Name.Replace("-", "_").Replace(" ", "_");
                b0.AppendLine($"        {enumName} = 1 << {i},");

                b1.AppendLine($"            {{ {i + 1}, SkillType.{enumName} }},");
                b2.AppendLine($"            {{ SkillType.{enumName}, {i + 1} }},");

                i++;
            }

            File.WriteAllText(k_ScriptPath, string.Format(k_ScriptTemplate, b0.ToString(), b1.ToString(), b2.ToString()));
            AssetDatabase.Refresh();
        }
    }
}
