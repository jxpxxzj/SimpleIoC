using System;

namespace SimpleIoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ComponentAttribute : Attribute
    {
        public string Name { get; set; }

        public ComponentAttribute() : this(string.Empty)
        {

        }
        public ComponentAttribute(string name)
        {
            Name = name;
        }
    }
}
