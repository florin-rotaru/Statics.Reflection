using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Air.Reflection
{
    public static class TypeInfo
    {
        private const char DOT = '.';

        private static readonly ConcurrentDictionary<Type, List<MemberInfo>> MembersLists = new ConcurrentDictionary<Type, List<MemberInfo>>();

        public static readonly Type[] NumericBuiltInTypes = new Type[]
        {
            typeof(sbyte),
            typeof(byte),

            typeof(short),
            typeof(ushort),

            typeof(int),
            typeof(uint),

            typeof(long),
            typeof(ulong),

            typeof(float),
            typeof(double),
            typeof(decimal)
        };

        /// <summary>
        /// Gets the underlying type code of the specified System.Type and compares it against numeric ones
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumeric(Type type)
        {
            Type nType = Nullable.GetUnderlyingType(type) ?? type;
            for (int i = 0; i < NumericBuiltInTypes.Length; i++)
                if (NumericBuiltInTypes[i] == nType)
                    return true;

            return false;
        }

        /// <summary>
        /// Gets the underlying type code of the specified System.Type and compares it against numeric ones
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNumeric(object o) =>
            IsNumeric(o.GetType());

        public static bool IsEnumerable(Type type) =>
            type == typeof(IEnumerable) || typeof(IEnumerable).IsAssignableFrom(type);

        public static bool IsStatic(Type type) =>
            type.IsAbstract && type.IsSealed;

        private static readonly Type[] BuiltInTypes = new Type[]
        {
            typeof(bool),

            typeof(char),

            typeof(sbyte),
            typeof(byte),

            typeof(short),
            typeof(ushort),

            typeof(int),
            typeof(uint),

            typeof(long),
            typeof(ulong),

            typeof(float),
            typeof(double),

            typeof(decimal),

            typeof(string),

            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(Enum),
            typeof(Guid),
            typeof(TimeSpan),

            typeof(object)
        };

        public static IEnumerable<Type> GetBuiltInTypes() => BuiltInTypes;

        static bool BuiltInTypesContains(Type type)
        {
            for (int i = 0; i < BuiltInTypes.Length; i++)
                if (BuiltInTypes[i] == type)
                    return true;

            return false;
        }

        public static bool IsEnum(Type type) =>
            type.IsEnum || (Nullable.GetUnderlyingType(type) ?? type).IsEnum;

        public static bool IsBuiltIn(Type type) =>
            type.IsPrimitive ||
            IsEnum(type) ||
            BuiltInTypesContains(type) ||
            BuiltInTypesContains(Nullable.GetUnderlyingType(type) ?? type);

        public static bool IsNonBuiltInStruct(Type type) =>
            !IsBuiltIn(type) && type.IsValueType;

        /// <summary>
        /// Determines whether the type implements generic type interface (example: IEnumerable<>)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericTypeInterface"></param>
        /// <returns></returns>
        public static bool ImplementsGenericTypeInterface(Type type, Type genericTypeInterface) =>
           type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeInterface);

        /// <summary>
        /// Determines whether the type implements any of the generic type interfaces (example: IEnumerable<>)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericTypeInterface"></param>
        /// <returns></returns>
        public static bool ImplementsAnyGenericTypeInterface(Type type, params Type[] genericTypeInterface) =>
           type.GetInterfaces().Any(i => i.IsGenericType && genericTypeInterface.Contains(i.GetGenericTypeDefinition()));

        /// <summary>
        /// Returns the interface which is of type generic 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericTypeInterface"></param>
        /// <returns></returns>
        public static Type GetGenericTypeInterface(Type type, Type genericTypeInterface) =>
            type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeInterface);


        public static IEnumerable<TypeNode> GetNodes(
            Type type, 
            bool useNullableUnderlyingTypeMembers, 
            int recursion = 0, 
            int depth = 0,
            Type[] ignoreNodeTypes = null)
        {
            if (recursion < 0)
                throw new ArgumentException(nameof(recursion));

            List<TypeNode> returnValue = new List<TypeNode>();

            Queue<TypeNode> queue = new Queue<TypeNode>(new[] {
                new TypeNode(string.Empty, 0, type, IsStatic(type), useNullableUnderlyingTypeMembers)
            });

            while (queue.Count > 0)
            {
                TypeNode queueNode = queue.Dequeue();

                returnValue.Add(queueNode);

                foreach (MemberInfo member in queueNode.Members)
                {
                    if (member.IsEnum ||
                        member.IsBuiltIn ||
                        member.IsEnumerable ||
                        (depth != 0 && queueNode.Depth >= depth) ||
                        (ignoreNodeTypes != null && ignoreNodeTypes.Contains(member.Type)))
                        continue;

                    TypeNode node = new TypeNode(
                        queueNode.Name == string.Empty ? member.Name : queueNode.Name + DOT + member.Name,
                        queueNode.Depth + 1,
                        member.Type,
                        member.IsStatic,
                        useNullableUnderlyingTypeMembers);

                    int occurrence = returnValue.Count(n =>
                        n.Depth < node.Depth &&
                        (
                            n.Type == node.Type ||
                            n.Type == node.NullableUnderlyingType
                        ));

                    if (occurrence <= recursion)
                        queue.Enqueue(node);
                    else
                        returnValue.Add(node);
                }
            }

            return returnValue;
        }

        public static string GetNodeName(string member)
        {
            if (member != null)
                for (int c = member.Length - 1; c > -1; c--)
                    if (member[c] == DOT)
                        return member.Substring(0, c);

            return string.Empty;
        }

        private static readonly ConcurrentDictionary<Type, object> ValueTypesDefaultValue = new ConcurrentDictionary<Type, object>();

        private static object GetValueTypeDefaultValue(Type valueType)
        {
            if (ValueTypesDefaultValue.TryGetValue(valueType, out object value))
            {
                return value;
            }
            else
            {
                value = Activator.CreateInstance(valueType);
                ValueTypesDefaultValue.TryAdd(valueType, value);
                return value;
            }
        }

        public static object GetDefaultValue(Type type) =>
            type.IsValueType && Nullable.GetUnderlyingType(type) == null ? GetValueTypeDefaultValue(type) : default;

        public static object GetDefaultValue(Type type, FieldInfo field)
        {
            try
            {
                object instance = Activator.CreateInstance(type);
                return field.GetValue(instance);
            }
            catch
            {
                return GetDefaultValue(field.FieldType);
            }
        }

        public static object GetDefaultValue(Type type, PropertyInfo property)
        {
            try
            {
                object instance = Activator.CreateInstance(type);
                return property.GetValue(instance, null);
            }
            catch
            {
                return GetDefaultValue(property.PropertyType);
            }
        }

        public static string GetName(string member)
        {
            if (member != null)
                for (int c = member.Length - 1; c > -1; c--)
                    if (member[c] == DOT)
                        return member.Substring(c + 1, member.Length - (c + 1));

            return member ?? string.Empty;
        }

        public static string GetName(Expression expression, bool includePath)
        {
            Stack<string> retunValue = new Stack<string>();
            Expression evaluate = expression;

            while (evaluate != null)
            {
                if (evaluate.NodeType == ExpressionType.Parameter)
                {
                    evaluate = null;
                }
                else if (evaluate is MemberExpression)
                {
                    retunValue.Push(((MemberExpression)evaluate).Member.Name);
                    evaluate = includePath ? ((MemberExpression)evaluate).Expression : null;
                }
                else if (evaluate is UnaryExpression &&
                    evaluate.NodeType == ExpressionType.Convert ||
                    evaluate.NodeType == ExpressionType.ConvertChecked)
                {
                    evaluate = ((UnaryExpression)evaluate).Operand;
                }
                else if (evaluate is LambdaExpression)
                {
                    evaluate = ((LambdaExpression)evaluate).Body;
                }
                else
                {
                    evaluate = null;
                }
            }

            return retunValue.Count != 0 ? string.Join(".", retunValue.ToArray()) : string.Empty;
        }

        public static IEnumerable<MemberInfo> GetMembers(Type type, bool useNullableUnderlyingTypeMembers = false)
        {
            Type targetType = type;

            if (useNullableUnderlyingTypeMembers && Nullable.GetUnderlyingType(type) != null)
                targetType = Nullable.GetUnderlyingType(type);

            if (!MembersLists.TryGetValue(targetType, out List<MemberInfo> retunValue))
            {
                retunValue = new List<MemberInfo>(
                    targetType.GetProperties()
                    .Where(p => p.DeclaringType != null && p.GetIndexParameters().Length == 0)
                    .Select(s => { return new MemberInfo(targetType, s); }));

                retunValue.AddRange(new List<MemberInfo>(
                    targetType.GetFields().Select(s => { return new MemberInfo(targetType, s); }))
                );

                MembersLists.AddOrUpdate(targetType, retunValue, (k, v) => retunValue);
            }

            return retunValue;
        }

        public static IEnumerable<MemberInfo> GetGettableMembers(Type type, bool useNullableUnderlyingTypeMembers = false) =>
            GetMembers(type, useNullableUnderlyingTypeMembers).Where(w => w.HasGetMethod).ToList();

        public static IEnumerable<MemberInfo> GetSettableMembers(Type type, bool useNullableUnderlyingTypeMembers = false) =>
            GetMembers(type, useNullableUnderlyingTypeMembers).Where(w => w.HasSetMethod).ToList();

        public static IEnumerable<string> GetMembersNames(Type type, bool useNullableUnderlyingTypeMembers = false, int recursion = 0) =>
            GetNodes(type, useNullableUnderlyingTypeMembers, recursion)
                .SelectMany(
                    s => s.Members,
                    (node, member) => node.Name == string.Empty ? member.Name : node.Name + DOT + member.Name)
                .ToList();

        public static IEnumerable<string> GetMembersNames<T>(bool useNullableUnderlyingTypeMembers = false, int recursion = 0) =>
            GetMembersNames(typeof(T), useNullableUnderlyingTypeMembers, recursion);

        public static IEnumerable<string> GetNames(Expression expression)
        {
            List<string> retunValue = new List<string>();
            Queue<Expression> queue = new Queue<Expression>(new[] { expression });

            while (queue.Count != 0)
            {
                Expression expr = queue.Dequeue();
                if (expr is MemberExpression)
                    retunValue.Add(((MemberExpression)expr).Member.Name);
                else if (expr is NewExpression)
                    retunValue.AddRange(((NewExpression)expr).Members.Select(s => s.Name));
                else if (expr is UnaryExpression && expr.NodeType == ExpressionType.Convert || expr.NodeType == ExpressionType.ConvertChecked)
                    queue.Enqueue(((UnaryExpression)expr).Operand);
                else if (expr is LambdaExpression)
                    queue.Enqueue(((LambdaExpression)expr).Body);
            }

            return retunValue;
        }

        public static IDictionary<string, MemberInfo> MembersInfoDictionary(Type type, bool useNullableUnderlyingTypeMembers = false, int recursion = 0) =>
            GetNodes(type, useNullableUnderlyingTypeMembers, recursion)
                .SelectMany(
                    node => node.Members,
                    (node, member) => new KeyValuePair<string, MemberInfo>(
                        node.Name == string.Empty ? member.Name : node.Name + DOT + member.Name,
                        member))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

        public static IDictionary<string, object> ValuesDictionary<T>(T values, int recursion = 0)
        {
            Dictionary<string, object> retunValue = new Dictionary<string, object>();

            List<TypeNode> nodes = GetNodes(typeof(T), true, recursion).ToList();
            nodes.First(w => w.Depth == 0).Value = values;

            for (int n = 0; n < nodes.Count; n++)
            {
                foreach (MemberInfo member in nodes[n].Members)
                {
                    if (!member.HasGetMethod)
                        continue;

                    string name = nodes[n].Name == string.Empty ? member.Name : nodes[n].Name + DOT + member.Name;

                    if (member.IsEnum || member.IsBuiltIn || member.IsEnumerable)
                    {
                        retunValue.Add(name, member.GetValue(nodes[n].Value));
                    }
                    else
                    {
                        TypeNode memberNode = nodes.FirstOrDefault(w => w.Name == name);

                        if (memberNode != null)
                            memberNode.Value = member.GetValue(nodes[n].Value);
                    }
                }
            }

            return retunValue;
        }
    }
}
