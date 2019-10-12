using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IMonoDatabase), true)]
    public class MonoDatabase : IMonoDatabase
    {
        public event Action<Selectable, SelectableBehaviour> SelectableAdded;
        public event Action<Selectable> SelectableRemoved;

        private IDictionary<Selectable, SelectableBehaviour> m_SelectableMap;

        private readonly IHexDatabase HexDatabase;
        public MonoDatabase(IHexDatabase HexDatabase)
        {
            this.HexDatabase = HexDatabase;

            m_SelectableMap = new Dictionary<Selectable, SelectableBehaviour>();
        }

        public void Start()
        {
            var selectableBehaviours = GameObject.FindObjectsOfType<SelectableBehaviour>();
            foreach (var el in selectableBehaviours)
            {
                var selectable = HexDatabase.GetSelectable(el.Cell);
                if (selectable == default)
                {
                    Debug.LogError("Selectable component was in scene but not in the database. Default selectable was assigned in MonoDatabase. Did you forget to rebuild the database?");
                    continue;
                }

                m_SelectableMap[selectable] = el;
            }
        }

        public IEnumerable<T> GetAllBehavioursOfType<T>() where T : class
        {
            return m_SelectableMap.Values.Where(v => v is T).Cast<T>();
        }

        public T GetBehaviourFor<T>(Selectable selectable) where T : class
        {
            if (m_SelectableMap.TryGetValue(selectable, out SelectableBehaviour behaviour))
            {
                return behaviour as T;
            }

            return default;
        }

        public void AddBehaviour(Selectable selectable, SelectableBehaviour behaviour)
        {
            m_SelectableMap.Add(selectable, behaviour);
            SelectableAdded?.Invoke(selectable, behaviour);
        }

        public void RemoveBehaviour(Selectable selectable)
        {
            m_SelectableMap.Remove(selectable);
            SelectableRemoved?.Invoke(selectable);
        }
    }
}
