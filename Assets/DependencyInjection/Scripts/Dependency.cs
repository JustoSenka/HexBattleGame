using System;

namespace Assets
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class Dependency : Attribute
    {
        /// <summary>
        /// Interface or base class of depencency this field should be set upon initialization.
        /// This type will be created from UnityContainer and given to any field or property marked with PassStaticDependencyAttribute
        /// </summary>
        public Type DependencyType { get; }

        public Dependency(Type Type) : base()
        {
            this.DependencyType = Type;
        }
    }
}
