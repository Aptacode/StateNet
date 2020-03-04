using System.Reflection;

namespace Aptacode.StateNet.Extensions
{
    public static class ReflectionHelper
    {
        /// <summary>
        ///     If a member has a SetValue method, it will be invoked
        ///     using the given arguments.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="obj">The object whose value will be set if possible</param>
        /// <param name="value">The new value</param>
        /// <returns></returns>
        public static bool TrySetValue(this MemberInfo memberInfo, object obj, object value)
        {
            if (memberInfo.GetType().IsSubclassOf(typeof(PropertyInfo)))
            {
                ((PropertyInfo) memberInfo).SetValue(obj, value);
                return true;
            }

            if (memberInfo.GetType().IsSubclassOf(typeof(FieldInfo)))
            {
                ((FieldInfo) memberInfo).SetValue(obj, value);
                return true;
            }

            return false;
        }

        public static bool TryGetValue<T>(this MemberInfo memberInfo, object obj, out T output)
        {
            if (memberInfo.GetType().IsSubclassOf(typeof(PropertyInfo)))
            {
                output = (T) ((PropertyInfo) memberInfo).GetValue(obj);
                return true;
            }

            if (memberInfo.GetType().IsSubclassOf(typeof(FieldInfo)))
            {
                output = (T) ((FieldInfo) memberInfo).GetValue(obj);
                return true;
            }

            output = default;
            return false;
        }
    }
}