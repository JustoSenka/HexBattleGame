using System.Collections.Generic;

namespace Assets
{
    public interface IObjectPool
    {
        IEnumerable<PoolItem> ReserveItems(int amount);
        void DestroyPool();
    }
}
