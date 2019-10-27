using Assets.GameLogic.ExtensionMethods;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class DatabaseEditor : UnityEditor.Editor
    {
        public static void PrintLabelGUI((string Label, int Width, Type type)[] LabelDetails)
        {
            EditorGUILayout.BeginHorizontal();

            foreach (var (Label, Width, Type) in LabelDetails)
            {
                int widthToUse = GetLabelWidth(Label, Width);
                EditorGUILayout.LabelField(Label, GUILayout.Width(widthToUse));
            }

            EditorGUILayout.EndHorizontal();
        }

        public static void PrintObjectGUI<T>((string Label, int Width, Type type)[] LabelDetails, List<T> list, T obj, int index, bool printIndex = false, bool labelsInFront = false)
        {
            EditorGUILayout.BeginHorizontal();

            foreach (var (Label, Width, Type) in LabelDetails)
            {
                var updated = false;
                if (Label == "ID")
                    updated = ShowField(ref obj, index.ToString(), Width, Type, printIndex, labelsInFront);
                else
                    updated = ShowField(ref obj, Label, Width, Type, printIndex, labelsInFront);

                // If object is struct, we need to assign back, since it is not reference type
                if (updated)
                    list[index] = obj;
            }

            if (GUILayout.Button("-", GUILayout.Width(20)))
                list.RemoveAt(index);

            if (GUILayout.Button("+", GUILayout.Width(20)))
                list.Insert(index + 1, Activator.CreateInstance<T>());

            EditorGUILayout.EndHorizontal();
        }

        public static bool ShowField<T>(ref T obj, string Label, int width, Type Type, bool printIndex = false, bool labelsInFront = false)
        {
            var currValue = obj.GetFieldIfExist(Label);
            var newVal = GetNewValueBasedOnType(Label, Type, width, currValue, printIndex, labelsInFront);

            if (newVal != null && !newVal.Equals(currValue))
            {
                ReflectionExtension.SetField(ref obj, Label, newVal);
                return true;
            }

            return false;
        }

        private static object GetNewValueBasedOnType(string Label, Type Type, int width, object currValue, bool printIndex = false, bool labelsInFront = false)
        {
            var widthToUse = GetLabelWidth(Label, width);
            var widthOptions = GUILayout.Width(widthToUse);

            object newVal = null;

            if (Type == typeof(void) && printIndex)
                EditorGUILayout.LabelField(Label, widthOptions);

            else if (labelsInFront)
            {
                EditorGUILayout.LabelField(Label, widthOptions);
                widthOptions = GUILayout.Width(GetFieldWidth(Type));
            }

            if (Type == typeof(string))
                newVal = EditorGUILayout.TextField((string)currValue, widthOptions);

            else if (Type == typeof(int))
                newVal = EditorGUILayout.IntField((int)currValue, widthOptions);

            else if (Type == typeof(bool))
                newVal = EditorGUILayout.Toggle((bool)currValue, widthOptions);

            else if (Type == typeof(Enum))
                newVal = EditorGUILayout.EnumPopup((Enum)currValue, widthOptions);

            else if (Type == typeof(FlagsAttribute))
                newVal = EditorGUILayout.EnumFlagsField((Enum)currValue, widthOptions);

            return newVal;
        }

        private static int GetFieldWidth(Type type) => type == typeof(Enum) ? 100 : type == typeof(string) ? 100 : type == typeof(bool) ? 30 : type == typeof(int) ? 40 : 60;
        private static int GetLabelWidth(string Label, int Width) => Width == 0 ? Label.Length * 8 + 16 : Width;
    }
}
