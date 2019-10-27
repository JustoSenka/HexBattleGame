using System;
using System.Collections.Generic;
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
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            var DB = (SkillDatabaseData)serializedObject.targetObject;
            var skillArray = DB.Skills.ToArray();

            PrintLabelGUI(SkillDetails);

            for (int i = 0; i < skillArray.Length; i++)
            {
                var skill = skillArray[i];

                if (skill.Effects == null)
                    skill.Effects = new List<SkillEffect>();

                if (skill.Effects.Count == 0)
                    skill.Effects.Add(new SkillEffect());

                var effectsArray = skill.Effects.ToArray();
                PrintObjectGUI(SkillDetails, DB.Skills, skill, i, true);

                if (!m_ShowEffects)
                {
                    for (int j = 0; j < effectsArray.Length; j++)
                        PrintObjectGUI(SkillEffectDetails, skill.Effects, effectsArray[j], j, false, true);

                    GUILayout.Space(20);
                }
            }

            if (GUILayout.Button("New Skill"))
                DB.Skills.Add(new Skill());

            m_ShowEffects = GUILayout.Toggle(m_ShowEffects, "Show Effects");

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(DB);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
