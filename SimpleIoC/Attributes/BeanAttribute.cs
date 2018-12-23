using System;

namespace SimpleIoC.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class BeanAttribute : Attribute
    {
        public string Name { get; set; }

        public BeanAttribute() : this(string.Empty)
        {

        }
        public BeanAttribute(string name)
        {
            Name = name;
        }
    }
}
