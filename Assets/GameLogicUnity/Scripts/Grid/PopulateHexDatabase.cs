﻿using Assets.GameLogic.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    [RegisterDependency(typeof(IPopulateHexDatabase), true)]
    public class PopulateHexDatabase : IPopulateHexDatabase
    {
        [Dependency(typeof(PublicReferences))]
        private PublicReferences PublicReferences;

        private readonly IHexDatabase HexDatabase;

        public PopulateHexDatabase(IHexDatabase HexDatabase)
        {
            this.HexDatabase = HexDatabase;
        }

        public void Start()
        {
            if (PublicReferences == null)
            {
                Debug.LogWarning(this.GetType() + ": PublicReferences == null");
                return;
            }

            PopulateDatabaseWithHexData();
        }

        public void PopulateDatabaseWithHexData()
        {
            var allLoadedSceneNames = GetAllSceneNames().ToArray();
            foreach (var db in PublicReferences.MapHexDB.Where(d => allLoadedSceneNames.Contains(d.SceneName)))
            {
                foreach (var el in db.HexTypeData)
                {
                    var newCell = new HexCell(el.Hex, el.HexType);
                    HexDatabase.UpdateHexCell(newCell);
                }

                foreach (var el in db.SelectableData)
                    HexDatabase.AddNewSelectable(el.Clone());

                foreach (var el in db.MovableData)
                    HexDatabase.AddNewSelectable(el.Clone());

                foreach (var el in db.UnitData)
                    HexDatabase.AddNewSelectable(el.Clone());
            }
        }

        private static IEnumerable<string> GetAllSceneNames()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
                yield return SceneManager.GetSceneAt(i).name;
        }
    }
}
