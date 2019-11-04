using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(UnitDatabaseData))]
    public class UnitDatabaseEditor : DatabaseEditor
    {
        void OnEnable() { }

        public (string Label, int Width, Type type)[] UnitPropDetails = new[]
        {
            ("ID", 0, typeof(void)),
            ("Name", 100, typeof(string)),
            ("Tier", 0, typeof(int)),
            ("Attack", 0, typeof(int)),
            ("Defense", 0, typeof(int)),
            ("Health", 0, typeof(int)),
            ("Magic", 0, typeof(int)),
            ("RangeMin", 0, typeof(int)),
            ("RangeMax", 0, typeof(int)),
            ("SkillFlags", 80, typeof(FlagsAttribute)),
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var DB = (UnitDatabaseData)serializedObject.targetObject;
            var array = DB.Units.ToArray();

            PrintLabelGUI(UnitPropDetails);

            for (int i = 0; i < array.Length; i++)
            {
                var unit = array[i];
                unit.SkillFlags = SkillTypeExtension.SkillTypeFromIDs(unit.SkillIDs);

                var (arrayModified, elementsModified) = PrintObjectGUI(UnitPropDetails, DB.Units, unit, i, true);
                if (arrayModified)
                    return;

                if (elementsModified)
                    unit.SkillIDs = SkillTypeExtension.IDsFromSkillType(unit.SkillFlags);
            }

            if (GUILayout.Button("New Unit"))
                DB.Units.Add(new Unit());

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save"))
                DB.Save();
        }
    }
}
