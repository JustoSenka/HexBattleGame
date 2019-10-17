﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    [Serializable]
    [CreateAssetMenu(fileName = "UnitDatabaseData", menuName = "ScriptableObjects/UnitDatabaseData", order = 1)]
    public class UnitDatabaseData : SaveableScriptableObject
    {
        public List<Unit> Units = new List<Unit>();

        public void ClearAllData()
        {
            Units.Clear();
        }
    }
}
