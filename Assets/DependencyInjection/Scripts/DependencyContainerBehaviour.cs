using System.Linq;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// This component will populate DependencyContainer on awake and inject dependencies into every single component in the scene.
    /// Best to keep int a scene where managers/controlers live.
    /// Assigned game objects in array MonoBehaviourDependencies will be registered as singleton dependencies and can
    /// as well be injected in other objects/components which have [Dependency(MyMonoBehaviourType)]
    /// 
    /// Mono Behaviours are always injected as singletons, since they are created by unity itself.
    /// </summary>
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
