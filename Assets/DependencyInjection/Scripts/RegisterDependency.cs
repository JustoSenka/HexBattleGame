using System;

namespace Assets
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RegisterDependency : Attribute
    {
        public Type InterfaceType { get; }
        public bool Singleton { get; }

        public RegisterDependency(Type InterfaceType, bool Singleton) : base()
        {
            this.InterfaceType = InterfaceType;
            this.Singleton = Singleton;
        }
    }
}
