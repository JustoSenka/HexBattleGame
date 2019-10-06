using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class BuildDatabase
    {
        private const string k_AssetDatabaseDataFile = "Assets/Editor/{0}_DB.asset";

        public static IDictionary<Type, HexType> TypeToHexTypeMap = new Dictionary<Type, HexType>()
        {
            { typeof(HexObstacle), HexType.Obstacle },
            { typeof(MovableBehaviour), HexType.Unit },
            { typeof(SelectableBehaviour), HexType.Unit },
            { typeof(UnitBehaviour), HexType.Unit },
        };

        [MenuItem("Scenes/Build Hex Database")]
        public static void BuildHexDatabase()
        {
            foreach (var scene in GetAllScenes())
            {
                var path = string.Format(k_AssetDatabaseDataFile, scene.name);
                var db = HexDatabaseData.Load(path);
                db.ClearAllData();

                var gos = scene.GetRootGameObjects();

                foreach (var snap in gos.SelectMany(g => g.GetComponentsInChildren<SnapToGrid>()))
                {
                    // Mark tile type which determine if they are walkable or not
                    var pos = HexUtility.WorldPointToHex(snap.transform.position, 1);
                    db.HexTypeData.Add(new HexDatabaseData.HexTypeElement(pos, GetHexType(snap)));


                    // Add objects from map to the database
                    if (snap is UnitBehaviour unit)
                    {
                        unit.Unit.Cell = pos;
                        db.UnitData.Add(unit.Unit);
                    }
                    else if (snap is MovableBehaviour move)
                    {
                        move.Movable.Cell = pos;
                        db.MovableData.Add(move.Movable);
                    }
                    else if (snap is SelectableBehaviour sel)
                    {
                        sel.Selectable.Cell = pos;
                        db.SelectableData.Add(sel.Selectable);
                    }
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
