using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleIoC.Bean
{
    public class BeanMetadataComparer : IComparer<BeanMetadata>
    {
        public int Compare(BeanMetadata x, BeanMetadata y)
        {
            return x.Parameters.Count - y.Parameters.Count;
        }
    }

    public class BeanMetadataEqualityComparer : IEqualityComparer<BeanMetadata>
    {
        public bool Equals(BeanMetadata x, BeanMetadata y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(BeanMetadata obj)
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(obj.Name);
        }
    }

    public class BeanMetadata : IComparable<BeanMetadata>, IEquatable<BeanMetadata>
    {
        public static IComparer<BeanMetadata> Comparer { get; } = new BeanMetadataComparer();
        public static IEqualityComparer<BeanMetadata> EqualityComparer { get; } = new BeanMetadataEqualityComparer();

        public string Name { get; set; }
        public Type ParentType {
            get
            {
                return Constructor.DeclaringType;
            }
        }

        public Type BeanType { get; protected set; }

        public MethodBase Constructor { get; protected set; }

        public object Bean { get; set; }

        public bool IsCreated
        {
            get
            {
                return Bean != null;
            }
        }

        public bool IsMethodConstructor { get; protected set; }

        public bool IsClassConstructor { get; protected set; }
       

        public List<ParameterInfo> Parameters
        {
            get
            {
                return Constructor.GetParameters().ToList();
            }
        }

        public BeanMetadata(MethodBase constructor) : this(constructor, null)
        {

        }


        public BeanMetadata(MethodBase constructor, string name) : this(constructor, name, null)
        {

        }

        public BeanMetadata(MethodBase constructor, string name, object beanObject)
        {
            Constructor = constructor;
            Bean = beanObject;
            switch (constructor)
            {
                case MethodInfo m:
                    BeanType = m.ReturnType;
                    IsClassConstructor = false;
                    IsMethodConstructor = true;
                    break;
                case ConstructorInfo c:
                    BeanType = c.DeclaringType;
                    IsClassConstructor = true;
                    IsMethodConstructor = false;
                    break;
                default:
                    throw new NotImplementedException();

            }
            if (string.IsNullOrEmpty(name))
            {
                Name = BeanType.FullName;
            }
            else
            {
                Name = name;
            }
        }

        public int CompareTo(BeanMetadata other)
        {
            return Comparer.Compare(this, other);
        }

        public override int GetHashCode()
        {
            return EqualityComparer.GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            var metadata = obj as BeanMetadata;
            return metadata != null &&
                   EqualityComparer.Equals(this, metadata);
        }

        public bool Equals(BeanMetadata other)
        {
            return EqualityComparer.Equals(this, other);
        }

        public static bool operator ==(BeanMetadata metadata1, BeanMetadata metadata2)
        {
            return EqualityComparer.Equals(metadata1, metadata2);
        }

        public static bool operator !=(BeanMetadata metadata1, BeanMetadata metadata2)
        {
            return !(metadata1 == metadata2);
        }

        public override string ToString()
        {
            return $"BeanMeta name={Name}, Type={BeanType}, ParaCount={Parameters.Count()}";
        }
    }
}
