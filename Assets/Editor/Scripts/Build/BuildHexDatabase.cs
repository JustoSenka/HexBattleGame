using Assets.GameLogic.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class BuildDatabase
    {
        private const string k_AssetDatabaseDataFile = "Assets/GameLogicUnity/SO/Maps/{0}_DB.asset";

        public static IDictionary<Type, HexType> TypeToHexTypeMap = new Dictionary<Type, HexType>()
        {
            { typeof(HexObstacle), HexType.Obstacle },
            /*{ typeof(MovableBehaviour), HexType.Unit },
            { typeof(SelectableBehaviour), HexType.Unit },
            { typeof(UnitBehaviour), HexType.Unit },*/
        };

        [MenuItem("Tools/Build/Build Hex Database")]
        public static void BuildHexDatabase()
        {
            foreach (var scene in GetAllScenes())
            {
                var path = string.Format(k_AssetDatabaseDataFile, scene.name);
                var db = SaveableScriptableObject.Load<HexDatabaseData>(path);
                db.Map.ClearMapData();

                var gos = scene.GetRootGameObjects();

                foreach (var snap in gos.SelectMany(g => g.GetComponentsInChildren<SnapToGrid>()))
                {
                    // Mark tile type which determine if they are walkable or not
                    var pos = HexUtility.WorldPointToHex(snap.transform.position, 1);
                    db.Map.HexTypeData.Add(new Map.HexTypeElement(pos, GetHexType(snap)));


                    // Add objects from map to the database
                    if (snap is UnitBehaviour unit)
                    {
                        unit.Cell = pos;
                        db.Map.UnitData.Add(unit.GetFieldIfExist("m_Unit") as Unit);
                    }
                    else if (snap is MovableBehaviour move)
                    {
                        move.Cell = pos;
                        db.Map.MovableData.Add(move.GetFieldIfExist("m_Movable") as Movable);
                    }
                    else if (snap is SelectableBehaviour sel)
                    {
                        sel.Cell = pos;
                        db.Map.SelectableData.Add(sel.GetFieldIfExist("m_Selectable") as Selectable);
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
            {
                var s = SceneManager.GetSceneAt(i);
                if (s.name.Contains("Map"))
                    yield return s;
            }
        }
    }
}
