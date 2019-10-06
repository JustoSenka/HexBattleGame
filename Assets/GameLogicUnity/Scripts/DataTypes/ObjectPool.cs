using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class ObjectPool : IObjectPool
    {
        private readonly Transform m_ParentTransform;
        private readonly GameObject m_ItemPrefab;
        private readonly IList<PoolItem> m_Pool;

        public ObjectPool(GameObject Parent, GameObject ItemPrefab, int initialSize)
        {
            this.m_ParentTransform = Parent.transform;
            this.m_ItemPrefab = ItemPrefab;

            m_Pool = new List<PoolItem>();

            CreateNewItems(initialSize).ToArray();
        }

        public PoolItem ReserveItem()
        {
            var item = m_Pool.FirstOrDefault(i => !i.IsReserved);

            if (item == default)
                item = CreateNewItem();

            item.Reserve();
            return item;
        }

        // Reserving item calls should always iterate the collection when call has been made to reserve all the pool items right away
        public PoolItem[] ReserveItems(int amount) => ReserveItemsEnumerable(amount).ToArray();
        private IEnumerable<PoolItem> ReserveItemsEnumerable(int amount)
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

        private PoolItem CreateNewItem()
        {
            var go = GameObject.Instantiate(m_ItemPrefab, Vector3.zero, Quaternion.identity, m_ParentTransform);
            go.SetActive(false);
            var newItem = new PoolItem(false, go);
            m_Pool.Add(newItem);
            return newItem;
        }

        private IEnumerable<PoolItem> CreateNewItems(int amount)
        {
            for (int i = 0; i < amount; i++)
                yield return CreateNewItem();
        }
    }
}
