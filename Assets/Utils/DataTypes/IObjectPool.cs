using System.Collections.Generic;

namespace Assets
{
    public interface IObjectPool
    {
        PoolItem ReserveItem();
        IEnumerable<PoolItem> ReserveItems(int amount);

        void DestroyPool();
    }
}
