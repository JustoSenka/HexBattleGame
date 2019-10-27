using System.Linq;
using UnityEngine.SceneManagement;

namespace Assets
{
    [RegisterDependency(typeof(SceneLoader), true)]
    public class SceneLoader
    {
#pragma warning disable CS0649
        [Dependency(typeof(PublicReferences))]
        private PublicReferences PublicReferences;
#pragma warning restore CS0649

        private bool m_IgnoreSceneManagerCallback = false;

        private readonly IHexDatabase HexDatabase;
        public SceneLoader(IHexDatabase HexDatabase)
        {
            this.HexDatabase = HexDatabase;

            HexDatabase.BeforeMapUnload += OnBeforeMapUnload;
            HexDatabase.AfterMapLoaded += OnAfterMapLoaded;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (m_IgnoreSceneManagerCallback)
                return;

            // Load map to HexDatabase if scene was loaded from main menu
            if (scene.name.Contains("Map"))
            {
                m_IgnoreSceneManagerCallback = true;
                HexDatabase.LoadMap(PublicReferences.MapHexDB.First(db => db.Map.Name == scene.name).Map);
                m_IgnoreSceneManagerCallback = false;
            }
        }

        private void OnBeforeMapUnload(Map map)
        {
            SceneManager.UnloadSceneAsync(map.Name);
        }

        private void OnAfterMapLoaded(Map map)
        {
            if (m_IgnoreSceneManagerCallback)
                return;

            m_IgnoreSceneManagerCallback = true;
            SceneManager.LoadSceneAsync(map.Name);
            m_IgnoreSceneManagerCallback = false;
        }
    }
}
