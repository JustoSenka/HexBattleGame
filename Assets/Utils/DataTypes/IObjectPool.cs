namespace Assets
{
    public interface IObjectPool
    {
        PoolItem ReserveItem();
        PoolItem[] ReserveItems(int amount);

        void DestroyPool();
    }
}
