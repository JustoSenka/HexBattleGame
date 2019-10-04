using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class BuildDatabase
    {
        private const string k_AssetDatabaseDataFile = "Assets/Editor/{0}_DB.asset";

        public static IDictionary<Type, HexType> TypeToHexTypeMap = new Dictionary<Type, HexType>()
        {
            { typeof(HexObstacle), HexType.Obstacle },
        };

        [MenuItem("Scenes/Build Hex Database")]
        public static void BuildHexDatabase()
        {
            foreach (var scene in GetAllScenes())
            {
                var path = string.Format(k_AssetDatabaseDataFile, scene.name);
                var db = HexDatabaseData.Load(path);

                foreach (var snap in scene.GetRootGameObjects().SelectMany(g => g.GetComponentsInChildren<SnapToGrid>()))
                {
                    var pos = HexUtility.WorldPointToHex(snap.transform.position, 1);
                    db.Data[pos] = GetHexType(snap);
                }

                db.SceneName = scene.name;
                db.Save(path);
            }
        }

        private static HexType GetHexType(SnapToGrid snap)
        {
            TypeToHexTypeMap.TryGetValue(snap.GetType(), out HexType hexType);
            return hexType == default ? HexType.Empty : hexType;
        }

        private static IEnumerable<Scene> GetAllScenes()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
                yield return SceneManager.GetSceneAt(i);
        }
    }
}
