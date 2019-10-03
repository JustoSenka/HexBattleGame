using System;

namespace Assets
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class Dependency : Attribute
    {
        public Type DependencyType { get; }
        public Dependency(Type Type) : base()
        {
            this.DependencyType = Type;
        }
    }
}
