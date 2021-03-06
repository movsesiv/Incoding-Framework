namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Management.Instrumentation;
    using System.Reflection;
    using System.Web.Mvc;
    using Incoding.Maybe;

    #endregion

    public static class ReflectionExtensions
    {
        #region Factory constructors

        /// <summary>
        ///     Try find first attribute by T (type attribute)
        /// </summary>
        /// <typeparam name="TAttribute"> Type attribute for found </typeparam>
        /// <param name="methodInfo"> The method for search </param>
        /// <returns>
        ///     If method has attribute will be return attribute else <c>null</c>
        /// </returns>
        public static TAttribute FirstOrDefaultAttribute<TAttribute>(this MemberInfo methodInfo)
                where TAttribute : Attribute
        {
            var customAttributes = methodInfo.GetCustomAttributes(typeof(TAttribute), false);
            return customAttributes.Length == 0
                           ? null
                           : customAttributes[0] as TAttribute;
        }

        public static string GetMemberName(this LambdaExpression lambdaExpression)
        {
            string result = ExpressionHelper.GetExpressionText(lambdaExpression);
            if (!string.IsNullOrWhiteSpace(result))
                return result;

            string toDebugString = ((UnaryExpression)lambdaExpression.Body).Operand.ToString();
            int firstDot = toDebugString.IndexOf(".", StringComparison.InvariantCultureIgnoreCase) + 1;
            return toDebugString.Substring(firstDot, toDebugString.Length - firstDot);
        }

        public static string GetMemberNameAsHtmlId(this LambdaExpression lambdaExpression)
        {
            return lambdaExpression.GetMemberName().Replace(".", "_");
        }

        public static bool HasAttribute<TAttribute>(this MemberInfo methodInfo)
                where TAttribute : Attribute
        {
            return methodInfo.FirstOrDefaultAttribute<TAttribute>() != null;
        }

        public static bool IsImplement<TImplement>(this Type targetType) where TImplement : class
        {
            return targetType.IsImplement(typeof(TImplement));
        }

        public static bool IsImplement(this Type targetType, Type implementType)
        {
            var baseTargetType = targetType;
            while (baseTargetType != null)
            {
                if (baseTargetType == typeof(object) && implementType != typeof(object))
                    return false;

                if (baseTargetType == typeof(IDisposable) && implementType != typeof(IDisposable))
                    return false;

                if (baseTargetType == implementType)
                    return true;

                if (baseTargetType.IsAssignableFrom(implementType))
                    return true;

                if (baseTargetType.GetInterfaces().Any(@interface => IsImplement(@interface, implementType)))
                    return true;

                baseTargetType = baseTargetType.BaseType;
            }

            return false;
        }

        public static bool IsTypicalType(this Type type)
        {
            bool isPrimitive = type.IsValueType || type.IsEnum || type.IsPrimitive || type.IsAnyEquals(typeof(string), typeof(DateTime), typeof(TimeSpan));
            return isPrimitive;
        }

        public static TObject SetValue<TObject>(this TObject ob, string property, object value)
        {
            var memberInfo = GetMemberInfo(ob, property, BindingFlags.SetField | BindingFlags.SetProperty);
            if (memberInfo == null)
                throw new InstanceNotFoundException("Not found property or field {0}".ApplyFormat(property));

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
                propertyInfo.SetValue(ob, value, null);

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
                fieldInfo.SetValue(ob, value);

            return ob;
        }

        public static object TryGetValue<TObject>(this TObject ob, string property)
        {
            var memberInfo = GetMemberInfo(ob, property, BindingFlags.GetField | BindingFlags.GetProperty);

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
                return propertyInfo.GetValue(ob, null);

            var fieldInfo = memberInfo as FieldInfo;
            return fieldInfo != null ? fieldInfo.GetValue(ob) : null;
        }

        #endregion

        static MemberInfo GetMemberInfo(object ob, string property, BindingFlags bindings)
        {
            var currentType = ob.GetType();
            while (currentType != null && currentType != typeof(object))
            {
                var hasMember = currentType
                        .GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | bindings)
                        .FirstOrDefault(r => r.Name.Equals(property));

                if (hasMember != null)
                    return hasMember;

                currentType = currentType.BaseType;
            }

            return null;
        }
    }
}