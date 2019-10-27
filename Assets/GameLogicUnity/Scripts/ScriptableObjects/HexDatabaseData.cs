using System;
using System.IO;

namespace Assets
{
    [Serializable]
    public class HexDatabaseData : SaveableScriptableObject
    {
        public Map Map = new Map();

        public override void Save(string path)
        {
            Map.Name = SceneName;
            base.Save(path);
        }
    }
}
