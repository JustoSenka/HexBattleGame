using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [RegisterDependency(typeof(HealthBarManager), true)]
    public class HealthBarManager : IHealthBarManager
    {
        private readonly IBehaviourCollector<HealthBar> Collector;
        public HealthBarManager(IBehaviourCollector<HealthBar> Collector)
        {
            this.Collector = Collector;
        }

        public void Start()
        {
            Collector.Start();

            foreach (var pair in Collector.ObjectMap)
            {
                pair.Value.unit = pair.Key as Unit;
            }
        }

        public void Update()
        {
            foreach (var healthBar in Collector.ObjectMap)
            {
                // Update manually? or let behaviours update?
            }
        }
    }
}
