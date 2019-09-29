using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class ObjectPool : IObjectPool
    {
        private readonly GameObject m_Parent;
        private readonly GameObject m_ItemPrefab;
        private readonly IList<PoolItem> m_Pool;

        public ObjectPool(GameObject Parent, GameObject ItemPrefab, int initialSize)
        {
            this.m_Parent = Parent;
            this.m_ItemPrefab = ItemPrefab;

            m_Pool = new List<PoolItem>();

            CreateNewItems(initialSize).ToArray();
        }

        public IEnumerable<PoolItem> ReserveItems(int amount)
        {
            // Find free items in current pool and return those
            var reserved = 0;
            for (int i = 0; i < m_Pool.Count; i++)
            {
                var item = m_Pool[i];
                if (!item.IsReserved)
                {
                    item.Reserve();
                    reserved++;
                    yield return m_Pool[i];
                }

                if (reserved == amount)
                    break;
            }

            // Create new items if pool is missing any
            if (reserved < amount)
            {
                var itemsToCreate = amount - reserved;
                var items = CreateNewItems(itemsToCreate).ToArray();
                for (int i = 0; i < items.Count(); i++)
                {
                    var item = items[i];
                    item.Reserve();
                    yield return item;
                }
            }
        }

        public void DestroyPool()
        {
            m_Pool.Destroy();
            m_Pool.Clear();
        }

        // Private ---

        private IEnumerable<PoolItem> CreateNewItems(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var go = GameObject.Instantiate(m_ItemPrefab, Vector3.zero, Quaternion.identity, m_Parent.transform);
                var newItem = new PoolItem(false, go);
                m_Pool.Add(newItem);
                yield return newItem;
            }
        }
    }
}
