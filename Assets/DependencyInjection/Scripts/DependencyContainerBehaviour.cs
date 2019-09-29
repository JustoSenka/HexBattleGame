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

            // Pass dependencies to all singletons in our dependency map. Some of them were created few lines above. Some of them are mono behaviours
            foreach (var singleton in Container.SingletonDependencyMap)
                Container.InjectDependenciesInto(singleton.Value);


            // Pass dependencies to all components in loaded scene
            var roots = gameObject.scene.GetRootGameObjects();
            var components = roots.SelectMany(go => go.GetComponentsInChildren<MonoBehaviour>());
            foreach (var c in components)
                Container.InjectDependenciesInto(c);

            // Log info
            LogInfo();
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
