using System;
using System.Collections.Generic;

namespace Air.Reflection
{
    public class TypeNode
    {
        public string Name { get; set; }
        public int Depth { get; set; }
        public Type Type { get; set; }
        public bool IsStatic { get; set; }
        public Type NullableUnderlyingType { get; set; }
        public List<MemberInfo> Members { get; set; }

        public object Value { get; set; }

        public TypeNode() { }
        public TypeNode(string name, int depth, Type type, bool isStatic, bool useNullableUnderlyingTypeMembers)
        {
            Name = name;
            Depth = depth;
            Type = type;
            IsStatic = isStatic;
            NullableUnderlyingType = Nullable.GetUnderlyingType(type);

            Members = TypeInfo.GetMembers(
                useNullableUnderlyingTypeMembers && NullableUnderlyingType != null ? NullableUnderlyingType : type,
                false);
        }
    }
}