using System;

namespace HN.HNLib.ObjectPool
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PoolableAttribute : Attribute
    {
        public int Count { get; private set; }
        public string PoolName { get; private set; }

        public PoolableAttribute(int count, string poolName = null)
        {
            Count = count;
            PoolName = poolName;
        }
    }
}