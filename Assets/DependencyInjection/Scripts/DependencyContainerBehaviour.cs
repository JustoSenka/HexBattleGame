using System.Linq;
using UnityEngine;

namespace Assets
{
    public class DependencyContainerBehaviour : MonoBehaviour
    {
        public GameObject[] MonoBehaviourDependencies;
        public DependencyContainer Container;

        public void Awake()
        {
            Container = new DependencyContainer();

            // Populate singleton dependency map with mono behaviours from the scene
            foreach (var dep in MonoBehaviourDependencies.SelectMany(go => go.GetComponents<MonoBehaviour>()))
                Container.RegisterSingleton(dep.GetType(), dep);

            // Populate dependency maps from types which have attribute on them (parses all loaded assemblies)
            Container.PopulateDependenciesFromAttributesInDomain();

            Container.InstantiateSingletons();

            // Log info
            LogInfo();

            /* Pass dependencies to all mono behaviours attached in public reference of this object. This is done by steps bellow
            foreach (var singleton in Container.SingletonDependencyMap.Where(s => s.Value.Instance is MonoBehaviour))
                Container.InjectDependenciesInto(singleton.Value.Instance); */

            // Pass dependencies to all components in loaded scene
            var roots = gameObject.scene.GetRootGameObjects();
            var components = roots.SelectMany(go => go.GetComponentsInChildren<MonoBehaviour>());
            foreach (var c in components)
                Container.InjectDependenciesInto(c);

        }

        /// <summary>
        /// Prints out everything about the collected dependencies.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private void LogInfo()
        {
            Debug.Log(Container.LogInformationAboutCollectedContainer());
        }
    }
}
