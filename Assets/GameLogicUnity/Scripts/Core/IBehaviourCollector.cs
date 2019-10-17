using System.Collections.Generic;

namespace Assets
{
    public interface IBehaviourCollector<T>
    {
        IDictionary<Selectable, T> ObjectMap { get; set; }

        void Start();
    }
}