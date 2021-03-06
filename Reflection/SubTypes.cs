using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Reflection
{
    public static class SubTypes
    {
        private static readonly Dictionary<Type, Type[]> subTypes = new Dictionary<Type, Type[]>();

        public static Type[] GetsubTypes(this Type type)
        {
            if (subTypes.TryGetValue(type, out Type[] types)) 
                return types;

            types = type.GetImplements().ToArray();
            subTypes.Add(type, types);
            return types;
        }

        public static IEnumerable<Type> GetImplements(this Type type)
        {
            var assembly = type.Assembly;
            return assembly.GetTypes().Where(t =>
                !t.IsInterface && !t.IsAbstract
                && (t == type || t.Implements(type)));
        }

        public static bool Implements<T>(this Type type)
            => Implements(type, typeof(T));

        public static bool Implements(this Type type, Type otherType)
            => otherType.IsInterface ? otherType.IsAssignableFrom(type) : type.IsSubclassOf(otherType);
    }
}
