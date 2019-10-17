using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    // [RegisterDependency(typeof(IBehaviourCollector<>), false)]
    public class BehaviourCollector<T> : IBehaviourCollector<T> where T : MonoBehaviour
    {
        public IDictionary<Selectable, T> ObjectMap { get; set; }

        private readonly IHexDatabase HexDatabase;
        public BehaviourCollector(IHexDatabase HexDatabase)
        {
            this.HexDatabase = HexDatabase;
        }

        public void Start()
        {
            ObjectMap = new Dictionary<Selectable, T>();

            var behaviours = GameObject.FindObjectsOfType<T>();
            foreach (var el in behaviours)
            {
                var cell = HexUtility.WorldPointToHex(el.gameObject.transform.position, 1);
                var selectable = HexDatabase.GetSelectable(cell);
                if (selectable == default)
                {
                    Debug.LogError("Selectable component was in scene but not in the database. Default selectable was assigned in BehaviourCollector. Did you forget to rebuild the database?");
                    continue;
                }

                ObjectMap[selectable] = el;
            }
        }

        public void AddBehaviour()
        {

        }

        public void RemoveBehaviour()
        {

        }
    }

    [RegisterDependency(typeof(IBehaviourCollector<HealthBar>), false)]
    public class HealthBarCollector : BehaviourCollector<HealthBar> { public HealthBarCollector(IHexDatabase HexDatabase) : base(HexDatabase) { } }
}
