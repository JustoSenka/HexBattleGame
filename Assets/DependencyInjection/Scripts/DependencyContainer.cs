using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets
{
    public class DependencyContainer
    {
        private const BindingFlags StaticNonPublic = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public Dictionary<Type, object> SingletonDependencyMap;
        public Dictionary<Type, Type> OtherDependencies;

        public DependencyContainer()
        {
            SingletonDependencyMap = new Dictionary<Type, object>();
            OtherDependencies = new Dictionary<Type, Type>();
        }

        public void Register<T, K>() => Register(typeof(T), typeof(K));
        public void Register(Type interfaceType, Type instanceType) => OtherDependencies.Add(interfaceType, instanceType);

        public void RegisterSingleton<T, K>() => RegisterSingleton(typeof(T), typeof(K));
        public void RegisterSingleton(Type interfaceType, object instance) => SingletonDependencyMap.Add(interfaceType, instance);


        /// <summary>
        /// Will Instantiate object of type T and pass all dependencies to the object if it requests for any.
        /// </summary>
        public T Resolve<T>() => (T)Resolve(typeof(T));
        public object Resolve(Type type)
        {
            var isSingleton = SingletonDependencyMap.ContainsKey(type);
            var isNotSingleton = OtherDependencies.ContainsKey(type);

            if (!isSingleton && !isNotSingleton)
            {
                Debug.LogError("Dependency of type: " + type + " was not registered in any of the maps.");
                return null;
            }
            // If dependency is a Singleton, just pass the same instance which is already created
            else if (isSingleton)
            {
                return SingletonDependencyMap[type];
            }
            // If not singleton, create new instance and set the field with it. Pass dependencies recursivelly to newly created class instance as well
            else if (isNotSingleton)
            {
                var actualInstanceType = OtherDependencies[type];
                var instance = Activator.CreateInstance(actualInstanceType);

                // Recursive calls to pass dependencies to all newly created types by the container
                InjectDependenciesInto(instance);

                return instance;
            }
            else
            {
                Debug.LogError("Dependency of type: " + type + " was both singleton and non singleton. This should not normally happen.");
                return null;
            }
        }

        /// <summary>
        /// Passes dependencies to object if object is requesting any with Dependency attribute.
        /// If dependency is not singleton, new instance of class will be created.
        /// That instance will also receive dependencies from current maps.
        /// </summary>
        public void InjectDependenciesInto(object obj)
        {
            var type = obj.GetType();
            var attributeType = typeof(Dependency);

            var allPropsWithAttribute = type.GetProperties(StaticNonPublic)
                .Where(p => p.CustomAttributes.Any(a => a.AttributeType == attributeType));
            var allFieldsWithAttribute = type.GetFields(StaticNonPublic)
                .Where(f => f.CustomAttributes.Any(a => a.AttributeType == attributeType));

            var propAttributeMap = allPropsWithAttribute
                .SelectMany(p => p.GetCustomAttributes(attributeType)
                .Select(a => (Prop: p, Attribute: (Dependency)a)));

            var fieldAttributeMap = allFieldsWithAttribute
                .SelectMany(f => f.GetCustomAttributes(attributeType)
                .Select(a => (Field: f, Attribute: (Dependency)a)));


            // Pass dependencies to properties
            foreach (var (Prop, Attribute) in propAttributeMap)
            {
                var dependency = Resolve(Attribute.DependencyType);
                Prop.SetValue(obj, dependency);
            }

            // Pass dependencies to fields
            foreach (var (Field, Attribute) in fieldAttributeMap)
            {
                var dependency = Resolve(Attribute.DependencyType);
                Field.SetValue(obj, dependency);
            }
        }

        /// <summary>
        /// Will parse all loaded assemblies in the domain in search for all types which have RegisterDependency attribute.
        /// If Singleton is set to true, will instantiate the type and store it in sigleton map.
        /// If Singleton is set to false, will store in map of (InterfaceType -> ActualType).
        /// Asumes that all dependencies are created with empty constructor.
        /// </summary>
        public void PopulateDependenciesFromAttributesInDomain()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
            var allTypesWithAttribute = allTypes.Where(t => t.CustomAttributes.Any(a => a.AttributeType == typeof(RegisterDependency)));
            var typeAttributeMap = allTypesWithAttribute
                .SelectMany(t => t.GetCustomAttributes(typeof(RegisterDependency))
                .Select(a => (Type: t, Attribute: (RegisterDependency)a)));

            var typeMap = typeAttributeMap.Select(t => (t.Type, t.Attribute.InterfaceType, t.Attribute.Singleton));

            foreach (var (Type, InterfaceType, IsSingleton) in typeMap)
            {
                if (IsSingleton)
                {
                    if (SingletonDependencyMap.ContainsKey(InterfaceType))
                    {
                        Debug.LogError("SingletonMap already contains type: " + InterfaceType + ". Did you register it twice? Is it a mono behaviour? Mono behaviours should be registered via public references since they are initialized by unity.");
                        continue;
                    }

                    var obj = Activator.CreateInstance(Type);
                    SingletonDependencyMap.Add(InterfaceType, obj);
                }
                else
                {
                    if (OtherDependencies.ContainsKey(InterfaceType))
                    {
                        Debug.LogError("OtherDependenciesMap already contains type: " + InterfaceType + ". Did you register it twice? Is it a mono behaviour? Mono behaviours should be registered via public references since they are initialized by unity.");
                        continue;
                    }

                    OtherDependencies.Add(InterfaceType, Type);
                }
            }
        }

        /// <summary>
        /// Returns a string of all registered dependencies.
        /// </summary>
        public string LogInformationAboutCollectedContainer()
        {
            var str = "Registered mono behaviours: \n";
            foreach (var pair in SingletonDependencyMap.Where(p => p.Value is MonoBehaviour))
                str += $"Type '{pair.Key}' to object of type '{pair.Value.GetType()}'\n";

            str += "\nRegistered singletons: \n";
            foreach (var pair in SingletonDependencyMap.Where(p => !(p.Value is MonoBehaviour)))
                str += $"Type '{pair.Key}' to object of type '{pair.Value.GetType()}'\n";


            str += "\nRegistered other dependencies: \n";
            foreach (var pair in OtherDependencies)
                str += $"Type '{pair.Key}' to type '{pair.Value}'\n";

            str += "\n-------------------------------------------------------------------------------------";
            return str;
        }
    }
}
