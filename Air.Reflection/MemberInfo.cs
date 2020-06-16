using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Air.Reflection
{
    public class MemberInfo
    {
        public string Name => MemberTypeInfo.Name;

        public MemberTypes MemberType => MemberTypeInfo.MemberType;
        public System.Reflection.MemberInfo MemberTypeInfo { get; }

        public Type MemberOf { get; }

        public Type Type { get; }

        public bool IsEnum { get; private set; }
        public bool IsBuiltIn { get; private set; }
        public bool IsEnumerable { get; private set; }
        public bool IsNumeric { get; private set; }
        public bool HasDefaultConstructor { get; private set; }
        public bool IsLiteral { get; private set; }

        public bool IsStatic { get; set; }

        public bool HasGetMethod { get; }
        public bool HasSetMethod { get; }
        public object DefaultValue { get; }
        public Func<object, object> GetValue { get; }

        public MemberInfo(Type type, PropertyInfo property)
        {
            MemberOf = type;
            MemberTypeInfo = property;
            Type = property.PropertyType;

            SetFlags();
            IsStatic = property.GetGetMethod().Attributes.HasFlag(MethodAttributes.Static);

            HasGetMethod = ((property.DeclaringType != null) ? property.GetGetMethod() : property.DeclaringType.GetProperty(property.Name).GetGetMethod()) != null;
            HasSetMethod = ((property.DeclaringType != null) ? property.GetSetMethod() : property.DeclaringType.GetProperty(property.Name).GetSetMethod()) != null;

            GetValue = HasGetMethod ? CompileGetValue(type, property) : null;
            DefaultValue = TypeInfo.GetDefaultValue(type, property);
        }

        public MemberInfo(Type type, FieldInfo field)
        {
            MemberOf = type;
            MemberTypeInfo = field;
            Type = field.FieldType;

            SetFlags();
            IsStatic = field.Attributes.HasFlag(FieldAttributes.Static);

            HasGetMethod = true;
            HasSetMethod = !IsLiteral;

            DefaultValue = TypeInfo.GetDefaultValue(type, field);
            GetValue = CompileGetValue(type, field, DefaultValue);
        }

        private bool HasLiteralOrInitOnlyFlag(FieldInfo fieldInfo) =>
            fieldInfo.Attributes.HasFlag(FieldAttributes.Literal) ||
                fieldInfo.Attributes.HasFlag(FieldAttributes.InitOnly);

        private void SetFlags()
        {
            IsEnum = TypeInfo.IsEnum(Type);
            IsBuiltIn = TypeInfo.IsBuiltIn(Type);
            IsEnumerable = TypeInfo.IsEnumerable(Type);
            IsNumeric = TypeInfo.IsNumeric(Type);

            HasDefaultConstructor = IsBuiltIn || Type.GetConstructor(Type.EmptyTypes) != null;
            IsLiteral = MemberTypeInfo.MemberType == MemberTypes.Field && HasLiteralOrInitOnlyFlag((FieldInfo)MemberTypeInfo);
        }

        private static void CreateSignature(out DynamicMethod dynamicMethod, out Emit.ILGenerator il)
        {
            dynamicMethod = new DynamicMethod($"{nameof(Air)}{Guid.NewGuid():N}", typeof(object), new[] { typeof(object) }, false);
            il = new Emit.ILGenerator(dynamicMethod.GetILGenerator(), true);
        }

        private static Func<object, object> CompileGetValue(Type type, PropertyInfo property)
        {
            if (property.GetIndexParameters().Length != 0)
                throw new NotSupportedException();

            CreateSignature(out DynamicMethod dynamicMethod, out Emit.ILGenerator il);

            if (property.GetGetMethod().Attributes.HasFlag(MethodAttributes.Static))
            {
                il.Emit(OpCodes.Call, property.GetGetMethod());
            }
            else if (type.IsValueType)
            {
                LocalBuilder local = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.EmitStloc(local.LocalIndex);
                il.EmitLdloca(local.LocalIndex);
                il.Emit(OpCodes.Call, property.GetGetMethod());
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Callvirt, property.GetGetMethod());
            }

            if (property.PropertyType.IsValueType)
                il.Emit(OpCodes.Box, property.PropertyType);

            il.Emit(OpCodes.Ret);
            return (Func<object, object>)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
        }

        private static Func<object, object> CompileGetValue(Type type, FieldInfo field, object defaultValue)
        {
            CreateSignature(out DynamicMethod dynamicMethod, out Emit.ILGenerator il);

            if (field.Attributes.HasFlag(FieldAttributes.Literal))
            {
                il.EmitLoadLiteral(field.FieldType, defaultValue);
            }
            else if (field.Attributes.HasFlag(FieldAttributes.Static))
            {
                il.Emit(OpCodes.Ldsfld, field);
            }
            else if (type.IsValueType)
            {
                LocalBuilder local = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.EmitStloc(local.LocalIndex);
                il.EmitLdloca(local.LocalIndex);
                il.Emit(OpCodes.Ldfld, field);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Ldfld, field);
            }

            if (field.FieldType.IsValueType)
                il.Emit(OpCodes.Box, field.FieldType);

            il.Emit(OpCodes.Ret);

            return (Func<object, object>)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
        }
    }
}