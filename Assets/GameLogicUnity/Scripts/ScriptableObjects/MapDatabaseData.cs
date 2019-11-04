using System;
using System.IO;

namespace Assets
{
    [Serializable]
    public class MapDatabaseData : SaveableScriptableObject
    {
        public override object JsonObjectToSerialize => Map;

        public Map Map = new Map();

        public override void Save(string path)
        {
            Map.Name = SceneName;
            base.Save(path);
        }
    }
}
