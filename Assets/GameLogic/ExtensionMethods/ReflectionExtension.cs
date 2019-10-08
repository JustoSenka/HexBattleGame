﻿using System;
using System.Reflection;
using UnityEngine;

namespace Assets.GameLogic.ExtensionMethods
{
    public static class ReflectionExtension
    {
        private const BindingFlags k_BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        public static void SetPropertyIfExist<T>(this T dest, string name, object value) where T : class
        {
            var destProp = dest.GetType().GetProperty(name, k_BindingFlags);
            destProp?.SetValue(dest, value);
        }

        public static object GetPropertyIfExist<T>(this T source, string name) where T : class
        {
            var prop = source.GetType().GetProperty(name, k_BindingFlags);
            return prop != null ? prop.GetValue(source) : null;
        }

        public static void SetFieldIfExist<T>(this T dest, string name, object value) where T : class
        {
            var destField = dest.GetType().GetField(name, k_BindingFlags);
            destField?.SetValue(dest, value);
        }

        public static object GetFieldIfExist<T>(this T source, string name) where T : class
        {
            var field = source.GetType().GetField(name, k_BindingFlags);
            return field != null ? field.GetValue(source) : null;
        }

        public static T Clone<T>(this T obj)
        {
            object newInstance = null;
            try
            {
                newInstance = Activator.CreateInstance(obj.GetType());

                var fields = obj.GetType().GetFields(k_BindingFlags);
                foreach (var field in fields)
                    field.SetValue(newInstance, field.GetValue(obj));
            }
            catch (Exception)
            {
                Debug.LogError("Command throws exception when cloning. Cloning method must be incorrect: " + obj.GetType());
            }
            return (T)newInstance;
        }
    }
}
