using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;

namespace Air.Reflection.Emit
{
    public partial class ILGenerator
    {
        System.Reflection.Emit.ILGenerator IL { get; }
        bool LogEnabled { get; }
        StringBuilder Log { get; } = new StringBuilder();

        public ILGenerator(System.Reflection.Emit.ILGenerator iLGenerator, bool enableLog = false)
        {
            IL = iLGenerator;
            LogEnabled = enableLog;
        }

        void AppendLineToLog(string value)
        {
            Log.AppendLine(value);
        }

        public StringBuilder GetLog()
        {
            return new StringBuilder(Log.ToString());
        }

        #region System.Reflection.Emit.ILGenerator 4.0.3.0

        public int ILOffset => IL.ILOffset;

        public virtual void BeginCatchBlock(Type exceptionType)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginCatchBlock)} (Type {nameof(exceptionType)}: {exceptionType.FullName})");
            IL.BeginCatchBlock(exceptionType);
        }

        public virtual void BeginExceptFilterBlock()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginExceptFilterBlock)}");
            IL.BeginExceptFilterBlock();
        }

        public virtual Label BeginExceptionBlock()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginExceptionBlock)}");
            return IL.BeginExceptionBlock();
        }

        public virtual void BeginFaultBlock()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginFaultBlock)}");
            IL.BeginFaultBlock();
        }

        public virtual void BeginFinallyBlock()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginFinallyBlock)}");
            IL.BeginFinallyBlock();
        }

        public virtual void BeginScope()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(BeginScope)}");
            IL.BeginScope();
        }

        public virtual LocalBuilder DeclareLocal(Type localType)
        {
            LocalBuilder returnValue = IL.DeclareLocal(localType);
            if (LogEnabled) AppendLineToLog($"{nameof(DeclareLocal)} [{returnValue.LocalIndex}] (Type {nameof(localType)}: {localType.FullName})");
            return returnValue;
        }

        public virtual LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            LocalBuilder returnValue = IL.DeclareLocal(localType, pinned);
            if (LogEnabled) AppendLineToLog($"{nameof(DeclareLocal)} [{returnValue.LocalIndex}] (Type {nameof(localType)}: {localType.FullName}, bool pinned: {pinned})");
            return returnValue;
        }

        public virtual Label DefineLabel()
        {
            return IL.DefineLabel();
        }

        public virtual void Emit(OpCode opcode)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode})");
            IL.Emit(opcode);
        }

        public virtual void Emit(OpCode opcode, byte arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, byte {nameof(arg)}: {arg})");
            IL.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, double arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, double {nameof(arg)}: {arg})");
            IL.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, short arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, short {nameof(arg)}: {arg})");
            IL.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, int arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, int {nameof(arg)}: {arg})");
            IL.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, long arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, long {nameof(arg)}: {arg})");
            IL.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, ConstructorInfo con)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, ConstructorInfo {nameof(con)}: {con.DeclaringType} {con})");
            IL.Emit(opcode, con);
        }

        public virtual void Emit(OpCode opcode, Label label)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, Label {nameof(label)}: label_{label.GetHashCode()}) {Environment.NewLine}");
            IL.Emit(opcode, label);
        }

        public virtual void Emit(OpCode opcode, Label[] labels)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, Label[] {nameof(labels)}: {string.Join(", ", labels.Select(s => "label_" + s.GetHashCode().ToString()))} {Environment.NewLine}");
            IL.Emit(opcode, labels);
        }

        public virtual void Emit(OpCode opcode, LocalBuilder local)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, LocalBuilder {nameof(local)}: {local})");
            IL.Emit(opcode, local);
        }

        public virtual void Emit(OpCode opcode, SignatureHelper signature)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, SignatureHelper {nameof(signature)}: {signature})");
            IL.Emit(opcode, signature);
        }

        public virtual void Emit(OpCode opcode, FieldInfo field)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, FieldInfo {nameof(field)}: {field})");
            IL.Emit(opcode, field);
        }

        public virtual void Emit(OpCode opcode, MethodInfo meth)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, MethodInfo {meth.DeclaringType} {nameof(meth)}: {meth})");
            IL.Emit(opcode, meth);
        }

        public virtual void Emit(OpCode opcode, float arg)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, float {nameof(arg)}: {arg})");
            IL.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, string str)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, string {nameof(str)}: {str})");
            IL.Emit(opcode, str);
        }

        public virtual void Emit(OpCode opcode, Type cls)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(Emit)} (OpCode {nameof(opcode)}: {opcode}, Type {nameof(cls)}: {cls})");
            IL.Emit(opcode, cls);
        }

        public virtual void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitCall)} (OpCode {nameof(opcode)}: {opcode}, MethodInfo {methodInfo.DeclaringType} {nameof(methodInfo)}: {methodInfo}, Type[] {nameof(optionalParameterTypes)}: {optionalParameterTypes})");
            IL.EmitCall(opcode, methodInfo, optionalParameterTypes);
        }

        public virtual void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitCalli)} (OpCode {nameof(opcode)}: {opcode}, " +
                $"CallingConventions {nameof(callingConvention)}: {callingConvention}, Type {nameof(returnType)}: {returnType}, " +
                $"Type[] {nameof(parameterTypes)}: {parameterTypes}, Type[] {nameof(optionalParameterTypes)}: {optionalParameterTypes})");
            IL.EmitCalli(opcode, callingConvention, returnType, parameterTypes, optionalParameterTypes);
        }

        public virtual void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitCalli)} (OpCode {nameof(opcode)}: {opcode}, " +
              $"CallingConvention {nameof(unmanagedCallConv)}: {unmanagedCallConv}, Type {nameof(returnType)}: {returnType}, " +
              $"Type[] {nameof(parameterTypes)}: {parameterTypes})");
            IL.EmitCalli(opcode, unmanagedCallConv, returnType, parameterTypes);
        }

        public virtual void EmitWriteLine(LocalBuilder localBuilder)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitWriteLine)} (LocalBuilder {nameof(localBuilder)}: {localBuilder})");
            IL.EmitWriteLine(localBuilder);
        }

        public virtual void EmitWriteLine(FieldInfo fld)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitWriteLine)} (FieldInfo {nameof(fld)}: {fld})");
            IL.EmitWriteLine(fld);
        }

        public virtual void EmitWriteLine(string value)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EmitWriteLine)} (string {nameof(value)}: {value})");
            IL.EmitWriteLine(value);
        }

        public virtual void EndExceptionBlock()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EndExceptionBlock)}");
            IL.EndExceptionBlock();
        }

        public virtual void EndScope()
        {
            if (LogEnabled) AppendLineToLog($"{nameof(EndScope)}");
            IL.EndScope();
        }

        public virtual void MarkLabel(Label loc)
        {
            if (LogEnabled) AppendLineToLog($"label_{loc.GetHashCode()} {Environment.NewLine}");
            IL.MarkLabel(loc);
        }

        public virtual void ThrowException(Type excType)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(ThrowException)} (Type {nameof(excType)}: {excType})");
            IL.ThrowException(excType);
        }

        public virtual void UsingNamespace(string usingNamespace)
        {
            if (LogEnabled) AppendLineToLog($"{nameof(UsingNamespace)} (string {nameof(usingNamespace)}: {usingNamespace})");
            IL.UsingNamespace(usingNamespace);
        }

        #endregion

        private protected const string To = "To";

        public static readonly FieldInfo DBNullValue =
          typeof(DBNull).GetFields().First(f => f.Name == nameof(DBNull.Value) && f.FieldType == typeof(DBNull));

        public static readonly MethodInfo GetTypeFromHandle =
            typeof(Type).GetMethods(BindingFlags.Public | BindingFlags.Static).Single(m =>
                m.Name == nameof(Type.GetTypeFromHandle) &&
                m.ReturnType == typeof(Type) &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == typeof(RuntimeTypeHandle));

        static readonly MethodInfo EnumParse = typeof(Enum).GetMethods(BindingFlags.Public | BindingFlags.Static).First(m =>
            m.IsGenericMethod &&
            m.Name == nameof(Enum.Parse) &&
            m.GetParameters().Length == 1 &&
            m.GetParameters()[0].ParameterType == typeof(string));

        static readonly MethodInfo ObjectToString = typeof(object).GetMethod(nameof(object.ToString));

        static readonly ConstructorInfo DateTimeConstructorTicks = typeof(DateTime).GetConstructor(new[] { typeof(long) });
        static readonly ConstructorInfo DateTimeOffsetConstructorDateTime = typeof(DateTimeOffset).GetConstructor(new[] { typeof(DateTime) });
        static readonly ConstructorInfo DecimalConstructorBits = typeof(decimal).GetConstructor(new[] { typeof(int[]) });
        static readonly ConstructorInfo GuidConstructorBytes = typeof(Guid).GetConstructor(new[] { typeof(byte[]) });
        static readonly ConstructorInfo TimeSpanConstructorTicks = typeof(TimeSpan).GetConstructor(new[] { typeof(long) });

        public void EmitLdc_I4(int value)
        {
            switch (value)
            {
                case -1: Emit(OpCodes.Ldc_I4_M1); break;
                case 0: Emit(OpCodes.Ldc_I4_0); break;
                case 1: Emit(OpCodes.Ldc_I4_1); break;
                case 2: Emit(OpCodes.Ldc_I4_2); break;
                case 3: Emit(OpCodes.Ldc_I4_3); break;
                case 4: Emit(OpCodes.Ldc_I4_4); break;
                case 5: Emit(OpCodes.Ldc_I4_5); break;
                case 6: Emit(OpCodes.Ldc_I4_6); break;
                case 7: Emit(OpCodes.Ldc_I4_7); break;
                case 8: Emit(OpCodes.Ldc_I4_8); break;
                default:
                    if (value >= sbyte.MinValue && value <= sbyte.MaxValue) { Emit(OpCodes.Ldc_I4_S, (sbyte)value); }
                    else { Emit(OpCodes.Ldc_I4, value); }
                    break;
            }
        }

        public void EmitLdarg(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));
            switch (index)
            {
                case 0: Emit(OpCodes.Ldarg_0); break;
                case 1: Emit(OpCodes.Ldarg_1); break;
                case 2: Emit(OpCodes.Ldarg_2); break;
                case 3: Emit(OpCodes.Ldarg_3); break;
                default:
                    if (index <= byte.MaxValue)
                    {
                        Emit(OpCodes.Ldarg_S, (byte)index);
                    }
                    else
                    {
                        Emit(OpCodes.Ldarg, (short)index);
                    }
                    break;
            }
        }

        public void EmitLdarga(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));

            if (index <= byte.MaxValue)
            {
                Emit(OpCodes.Ldarga_S, (byte)index);
            }
            else
            {
                Emit(OpCodes.Ldarga, (short)index);
            }
        }

        public void EmitStarg(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));
            switch (index)
            {
                default:
                    if (index <= byte.MaxValue)
                    {
                        Emit(OpCodes.Starg_S, (byte)index);
                    }
                    else
                    {
                        Emit(OpCodes.Starg, (short)index);
                    }
                    break;
            }
        }

        public void EmitLdloc(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));
            switch (index)
            {
                case 0: Emit(OpCodes.Ldloc_0); break;
                case 1: Emit(OpCodes.Ldloc_1); break;
                case 2: Emit(OpCodes.Ldloc_2); break;
                case 3: Emit(OpCodes.Ldloc_3); break;
                default:
                    if (index <= byte.MaxValue)
                    {
                        Emit(OpCodes.Ldloc_S, (byte)index);
                    }
                    else
                    {
                        Emit(OpCodes.Ldloc, (short)index);
                    }
                    break;
            }
        }

        public void EmitLdloca(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));

            if (index <= byte.MaxValue)
            {
                Emit(OpCodes.Ldloca_S, (byte)index);
            }
            else
            {
                Emit(OpCodes.Ldloca, (short)index);
            }
        }

        public void EmitStloc(int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentOutOfRangeException(nameof(index));
            switch (index)
            {
                case 0: Emit(OpCodes.Stloc_0); break;
                case 1: Emit(OpCodes.Stloc_1); break;
                case 2: Emit(OpCodes.Stloc_2); break;
                case 3: Emit(OpCodes.Stloc_3); break;
                default:
                    if (index <= byte.MaxValue)
                    {
                        Emit(OpCodes.Stloc_S, (byte)index);
                    }
                    else
                    {
                        Emit(OpCodes.Stloc, (short)index);
                    }
                    break;
            }
        }

        public void EmitInit(Type type)
        {
            Type t = type.GetTypeInfo().IsEnum ? Enum.GetUnderlyingType(type) : type;

            if (t.GetTypeInfo().IsValueType)
                Emit(OpCodes.Initobj, t);
            else if (t.GetConstructor(Type.EmptyTypes) != null)
                Emit(OpCodes.Newobj, t.GetConstructor(Type.EmptyTypes));
            else
                throw new InvalidOperationException($"Failed to initialize Type {type}");
        }

        public void EmitLoadLiteral(Type type, object value)
        {
            if (type == typeof(bool)) { EmitLdc_I4((bool)value ? 1 : 0); return; }
            if (type == typeof(char)) { EmitLdc_I4((char)value); return; }
            if (type == typeof(sbyte)) { EmitLdc_I4((sbyte)value); return; }
            if (type == typeof(byte)) { EmitLdc_I4((byte)value); return; }
            if (type == typeof(short)) { EmitLdc_I4((short)value); return; }
            if (type == typeof(ushort)) { EmitLdc_I4((ushort)value); return; }
            if (type == typeof(int)) { EmitLdc_I4((int)value); return; }
            if (type == typeof(uint)) { Emit(OpCodes.Ldc_I4, (uint)value); return; }
            if (type == typeof(long)) { Emit(OpCodes.Ldc_I8, (long)value); return; }
            if (type == typeof(ulong)) { Emit(OpCodes.Ldc_I8, (long)(ulong)value); Emit(OpCodes.Conv_U8); return; }
            if (type == typeof(float)) { Emit(OpCodes.Ldc_R4, (float)value); return; }
            if (type == typeof(double)) { Emit(OpCodes.Ldc_R8, (double)value); return; }
            if (type == typeof(decimal))
            {
                int[] memberDecimalBits = decimal.GetBits((decimal)value);

                EmitLdc_I4(4);
                Emit(OpCodes.Newarr, typeof(int));

                for (int i = 0; i < memberDecimalBits.Length; i++)
                {
                    Emit(OpCodes.Dup);
                    EmitLdc_I4(i);
                    EmitLdc_I4(memberDecimalBits[i]);
                    Emit(OpCodes.Stelem_I4);
                }

                Emit(OpCodes.Newobj, DecimalConstructorBits);

                return;
            }
            if (type == typeof(string))
            {
                Emit(OpCodes.Ldstr, (string)value);
                return;
            }
            if (type == typeof(DateTime))
            {
                Emit(OpCodes.Ldc_I8, ((DateTime)value).Ticks);
                Emit(OpCodes.Newobj, DateTimeConstructorTicks);
                return;
            }
            if (type == typeof(DateTimeOffset))
            {
                Emit(OpCodes.Ldc_I8, ((DateTimeOffset)value).Ticks);
                Emit(OpCodes.Newobj, DateTimeConstructorTicks);
                Emit(OpCodes.Newobj, DateTimeOffsetConstructorDateTime);
                return;
            }
            if (type.IsEnum)
            {
                EmitLdc_I4((int)value);
                return;
            }
            if (type == typeof(Guid))
            {
                byte[] memberGuidBytes = ((Guid)value).ToByteArray();
                EmitLdc_I4(16);
                Emit(OpCodes.Newarr, typeof(byte));

                for (int i = 0; i < memberGuidBytes.Length; i++)
                {
                    Emit(OpCodes.Dup);
                    EmitLdc_I4(i);
                    EmitLdc_I4(memberGuidBytes[i]);
                    Emit(OpCodes.Stelem_I1);
                }

                Emit(OpCodes.Newobj, GuidConstructorBytes);
                return;
            }
            if (type == typeof(TimeSpan))
            {
                Emit(OpCodes.Ldc_I8, ((TimeSpan)value).Ticks);
                Emit(OpCodes.Newobj, TimeSpanConstructorTicks);
                return;
            }
            else
            {
                throw new NotImplementedException($"{type}");
            }
        }

        public void EmitLoadLiteral(MemberInfo member)
        {
            if (!member.IsLiteral)
                throw new InvalidOperationException($"{member} is not a literal.");

            EmitLoadLiteral(member.Type, member.DefaultValue);
        }

        public void EmitLoadDefaultValue(Type type)
        {
            if (!type.IsValueType)
            {
                Emit(OpCodes.Ldnull);
                return;
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                Emit(OpCodes.Call, typeof(Nullables).GetMethod(nameof(Nullables.GetDefaultValue)).MakeGenericMethod(Nullable.GetUnderlyingType(type)));
                return;
            }

            if (type == typeof(bool)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(char)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(sbyte)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(byte)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(short)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(ushort)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(int)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(uint)) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(long)) { Emit(OpCodes.Ldc_I4_0); Emit(OpCodes.Conv_I8); return; }
            if (type == typeof(ulong)) { Emit(OpCodes.Ldc_I4_0); Emit(OpCodes.Conv_I8); return; }
            if (type == typeof(float)) { Emit(OpCodes.Ldc_R4, (float)0); return; }
            if (type == typeof(double)) { Emit(OpCodes.Ldc_R8, (double)0); return; }
            if (type == typeof(decimal)) { Emit(OpCodes.Ldsfld, typeof(decimal).GetFields().First(f => f.Name == nameof(Decimal.Zero))); return; }
            if (type == typeof(string)) { Emit(OpCodes.Ldnull); return; }
            if (type == typeof(DateTime))
            {
                Emit(OpCodes.Ldc_I4_0);
                Emit(OpCodes.Conv_I8);
                Emit(OpCodes.Newobj, DateTimeConstructorTicks);
                return;
            }
            if (type == typeof(DateTimeOffset))
            {
                Emit(OpCodes.Ldc_I4_0);
                Emit(OpCodes.Conv_I8);
                Emit(OpCodes.Newobj, DateTimeConstructorTicks);
                Emit(OpCodes.Newobj, DateTimeOffsetConstructorDateTime);
                return;
            }
            if (type.IsEnum) { Emit(OpCodes.Ldc_I4_0); return; }
            if (type == typeof(Guid)) { Emit(OpCodes.Ldsfld, typeof(Guid).GetFields().First(f => f.Name == nameof(Guid.Empty))); return; }
            if (type == typeof(TimeSpan))
            {
                Emit(OpCodes.Ldc_I4_0);
                Emit(OpCodes.Conv_I8);
                Emit(OpCodes.Newobj, TimeSpanConstructorTicks);
                return;
            }
            else
            {
                throw new NotSupportedException($"{type}");
            }
        }

        public void EmitCall(MemberInfo member, MethodInfo method)
        {
            Emit(member.IsStatic || member.MemberOf.IsValueType ? OpCodes.Call : OpCodes.Callvirt, method);
        }

        public void EmitLoadStaticMemberValue(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Property)
                EmitCall(member, ((PropertyInfo)member.MemberTypeInfo).GetGetMethod());
            else if (member.MemberType == MemberTypes.Field)
                Emit(OpCodes.Ldsfld, (FieldInfo)member.MemberTypeInfo);
            else
                throw new NotImplementedException();
        }

        public void EmitLoadMemberValue(MemberInfo member)
        {
            if (member.IsLiteral)
                EmitLoadLiteral(member);
            else if (member.IsStatic)
                EmitLoadStaticMemberValue(member);
            else if (member.MemberType == MemberTypes.Property)
                EmitCall(member, ((PropertyInfo)member.MemberTypeInfo).GetGetMethod());
            else if (member.MemberType == MemberTypes.Field)
                Emit(OpCodes.Ldfld, (FieldInfo)member.MemberTypeInfo);
            else
                throw new NotImplementedException();
        }

        public void EmitSetStaticMemberValue(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Property)
                EmitCall(member, ((PropertyInfo)member.MemberTypeInfo).GetSetMethod());
            else if (member.MemberType == MemberTypes.Field)
                Emit(OpCodes.Stsfld, (FieldInfo)member.MemberTypeInfo);
            else
                throw new NotImplementedException();
        }

        public void EmitSetMemberValue(MemberInfo member)
        {
            if (member.IsLiteral)
                throw new InvalidOperationException($"{nameof(EmitSetMemberValue)} {member}");
            else if (member.IsStatic)
                EmitSetStaticMemberValue(member);
            else if (member.MemberType == MemberTypes.Property)
                EmitCall(member, ((PropertyInfo)member.MemberTypeInfo).GetSetMethod());
            else if (member.MemberType == MemberTypes.Field)
                Emit(OpCodes.Stfld, (FieldInfo)member.MemberTypeInfo);
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Transfers control to a target instruction if value is false, a null reference, or zero.
        /// </summary>
        /// <param name="whenFalseNullOrZero"></param>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrfalse_s(
            Action whenTrueNotNullOrNonZero,
            Action whenFalseNullOrZero,
            bool useBr_S = false)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brfalse_S, evaluate);

            whenTrueNotNullOrNonZero();

            Label end = DefineLabel();

            Emit(useBr_S ? OpCodes.Br_S : OpCodes.Br, end);

            MarkLabel(evaluate);

            whenFalseNullOrZero();

            MarkLabel(end);
        }

        /// <summary>
        /// Transfers control to a target instruction if value is false, a null reference, or zero.
        /// </summary>
        /// <param name="whenFalseNullOrZero"></param>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrfalse(
            Action whenTrueNotNullOrNonZero,
            Action whenFalseNullOrZero,
            bool useBr_S = false)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brfalse, evaluate);

            whenTrueNotNullOrNonZero();

            Label end = DefineLabel();

            Emit(useBr_S ? OpCodes.Br_S : OpCodes.Br, end);

            MarkLabel(evaluate);

            whenFalseNullOrZero();

            MarkLabel(end);
        }

        /// <summary>
        /// Transfers control to a target instruction if value is false, a null reference, or zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrfalse_s(
            Action whenTrueNotNullOrNonZero)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brfalse_S, evaluate);

            whenTrueNotNullOrNonZero();

            MarkLabel(evaluate);
        }

        /// <summary>
        /// Transfers control to a target instruction if value is false, a null reference, or zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrfalse(
            Action whenTrueNotNullOrNonZero)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brfalse, evaluate);

            whenTrueNotNullOrNonZero();

            MarkLabel(evaluate);
        }

        /// <summary>
        /// Transfers control to a target instruction (short form) if value is true, not null, or non-zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        /// <param name="whenFalseNullOrZero"></param>
        public void EmitBrtrue_s(
            Action whenFalseNullOrZero,
            Action whenTrueNotNullOrNonZero,
            bool useBr_S = false)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brtrue_S, evaluate);

            whenFalseNullOrZero();

            Label end = DefineLabel();
            Emit(useBr_S ? OpCodes.Br_S : OpCodes.Br, end);

            MarkLabel(evaluate);

            whenTrueNotNullOrNonZero();

            MarkLabel(end);
        }
        /// <summary>
        /// Transfers control to a target instruction (short form) if value is true, not null, or non-zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        /// <param name="whenFalseNullOrZero"></param>

        public void EmitBrtrue(
            Action whenFalseNullOrZero,
            Action whenTrueNotNullOrNonZero,
            bool useBr_S = false)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brtrue, evaluate);

            whenFalseNullOrZero();

            Label end = DefineLabel();
            Emit(useBr_S ? OpCodes.Br_S : OpCodes.Br, end);

            MarkLabel(evaluate);

            whenTrueNotNullOrNonZero();

            MarkLabel(end);
        }

        /// <summary>
        /// Transfers control to a target instruction (short form) if value is true, not null, or non-zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrtrue_s(
            Action whenFalseNullOrZero)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brtrue_S, evaluate);

            whenFalseNullOrZero();

            MarkLabel(evaluate);
        }

        /// <summary>
        /// Transfers control to a target instruction (short form) if value is true, not null, or non-zero.
        /// </summary>
        /// <param name="whenTrueNotNullOrNonZero"></param>
        public void EmitBrtrue(
            Action whenFalseNullOrZero)
        {
            Label evaluate = DefineLabel();
            Emit(OpCodes.Brtrue, evaluate);

            whenFalseNullOrZero();

            MarkLabel(evaluate);
        }

        private void EmitConvToNonNullableNumeric(Type nonNullableNumericType)
        {
            if (nonNullableNumericType == typeof(sbyte)) { Emit(OpCodes.Conv_Ovf_I1); return; }
            if (nonNullableNumericType == typeof(byte)) { Emit(OpCodes.Conv_Ovf_I1_Un); return; }
            if (nonNullableNumericType == typeof(short)) { Emit(OpCodes.Conv_Ovf_I2); return; }
            if (nonNullableNumericType == typeof(ushort)) { Emit(OpCodes.Conv_Ovf_I2_Un); return; }
            if (nonNullableNumericType == typeof(int)) { Emit(OpCodes.Conv_Ovf_I4); return; }
            if (nonNullableNumericType == typeof(uint)) { Emit(OpCodes.Conv_Ovf_I4_Un); return; }
            if (nonNullableNumericType == typeof(long)) { Emit(OpCodes.Conv_Ovf_I8); return; }
            if (nonNullableNumericType == typeof(ulong)) { Emit(OpCodes.Conv_Ovf_I8_Un); return; }
            if (nonNullableNumericType == typeof(float)) { Emit(OpCodes.Conv_R4); return; }

            if (nonNullableNumericType == typeof(double)) { Emit(OpCodes.Conv_R8); return; }
        }

        public void EmitStore(Type destination)
        {
            if (destination == typeof(bool)) { Emit(OpCodes.Stind_I1); return; }

            if (destination == typeof(sbyte)) { Emit(OpCodes.Stind_I1); return; }
            if (destination == typeof(byte)) { Emit(OpCodes.Stind_I1); return; }

            if (destination == typeof(char)) { Emit(OpCodes.Stind_I2); return; }

            if (destination == typeof(short)) { Emit(OpCodes.Stind_I2); return; }
            if (destination == typeof(ushort)) { Emit(OpCodes.Stind_I2); return; }

            if (destination == typeof(int)) { Emit(OpCodes.Stind_I4); return; }
            if (destination == typeof(uint)) { Emit(OpCodes.Stind_I4); return; }

            if (destination == typeof(long)) { Emit(OpCodes.Stind_I8); return; }
            if (destination == typeof(ulong)) { Emit(OpCodes.Stind_I8); return; }

            if (destination == typeof(float)) { Emit(OpCodes.Stind_R4); return; }
            if (destination == typeof(double)) { Emit(OpCodes.Stind_R8); return; }

            if (destination == typeof(string)) { Emit(OpCodes.Stind_Ref); return; }

            if (destination.IsValueType)
                Emit(OpCodes.Stobj, destination);
            else
                Emit(OpCodes.Stind_Ref);
        }

        public static bool CanEmitSetOrConvert(
            Type source,
            Type destination)
        {
            Type nonNullableSourceType = Nullable.GetUnderlyingType(source) ?? source;
            Type nonNullableDestinationType = Nullable.GetUnderlyingType(destination) ?? destination;

            if (nonNullableSourceType == typeof(object) || nonNullableDestinationType == typeof(object))
                return true;

            if (nonNullableSourceType == nonNullableDestinationType)
                return true;

            if (TypeInfo.IsBuiltIn(nonNullableSourceType) != TypeInfo.IsBuiltIn(nonNullableDestinationType))
                return false;

            if (TypeInfo.IsNumeric(nonNullableSourceType))
            {
                if (nonNullableDestinationType == typeof(char) ||
                    TypeInfo.IsEnum(nonNullableDestinationType))
                    return CanEmitSetOrConvert(nonNullableSourceType, typeof(int));

                if (TypeInfo.IsNumeric(nonNullableDestinationType) &&
                    nonNullableSourceType != typeof(decimal) &&
                    nonNullableDestinationType != typeof(decimal))
                    return true;
            }

            if (nonNullableDestinationType == typeof(string))
                return true;

            if (TypeInfo.IsEnum(nonNullableSourceType))
            {
                if (TypeInfo.IsNumeric(nonNullableDestinationType))
                    return CanEmitSetOrConvert(typeof(int), destination);

                if (nonNullableDestinationType == typeof(char))
                    return true;

                if (TypeInfo.IsEnum(nonNullableDestinationType))
                    return true;
            }

            if (TypeInfo.IsEnum(nonNullableDestinationType))
            {
                if (nonNullableSourceType == typeof(string))
                    return true;

                if (nonNullableSourceType == typeof(char))
                    return true;
            }

            if (nonNullableDestinationType == typeof(bool))
            {
                if (nonNullableSourceType == typeof(string))
                    return true;

                if (TypeInfo.IsNumeric(nonNullableSourceType))
                    return true;
            }

            ConstructorInfo nonNullableDestinationTypeConstructor = nonNullableDestinationType.GetConstructor(new[] { nonNullableSourceType });
            if (nonNullableDestinationTypeConstructor != null)
                return true;

            MethodInfo convertToNonNullable = nonNullableSourceType.GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
                m.Name.Contains(To + nonNullableSourceType.Name) &&
                m.ReturnType == nonNullableDestinationType &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == nonNullableDestinationType);

            if (convertToNonNullable != null)
                return true;

            convertToNonNullable = typeof(Converters).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
                m.Name.Contains(To + nonNullableSourceType.Name) &&
                m.ReturnType == nonNullableDestinationType &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == nonNullableDestinationType);

            if (convertToNonNullable != null)
                return true;

            convertToNonNullable = typeof(Convert).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
                m.Name.Contains(To + nonNullableSourceType.Name) &&
                m.ReturnType == nonNullableDestinationType &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == nonNullableDestinationType);

            if (convertToNonNullable != null)
                return true;

            convertToNonNullable = typeof(IConvertible).GetMethods().FirstOrDefault(m =>
                m.Name.Contains(To + nonNullableDestinationType.Name) &&
                m.ReturnType == nonNullableDestinationType &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == typeof(IFormatProvider));

            if (convertToNonNullable != null)
                return true;

            return false;
        }

        public void EmitSetOrConvert(
            Type source,
            Type destination)
        {
            Type underlyingSourceType = Nullable.GetUnderlyingType(source);
            Type underlyingDestinationType = Nullable.GetUnderlyingType(destination);

            Type nonNullableSourceType = underlyingSourceType ?? source;
            Type nonNullableDestinationType = underlyingDestinationType ?? destination;

            if (underlyingSourceType != null)
            {
                if (source == typeof(object))
                    Emit(OpCodes.Unbox_Any, source);

                Emit(OpCodes.Call, typeof(Nullables).GetMethod(nameof(Nullables.GetValueOrDefault)).MakeGenericMethod(Nullable.GetUnderlyingType(source)));
                EmitSetOrConvert(underlyingSourceType, destination);
                return;
            }

            void unbox()
            {
                if (nonNullableSourceType == typeof(object))
                    Emit(OpCodes.Unbox_Any, nonNullableSourceType);
            }

            void box()
            {
                if (nonNullableSourceType != typeof(object))
                    Emit(OpCodes.Box, nonNullableSourceType);
            }

            void emitConvert(Action onBefore, Action convert)
            {
                onBefore();
                convert();

                if (underlyingDestinationType != null)
                    Emit(OpCodes.Newobj, typeof(Nullable<>).MakeGenericType(underlyingDestinationType).GetConstructor(new[] { underlyingDestinationType }));

                if (destination == typeof(object))
                    Emit(OpCodes.Box, destination);
            }

            if (nonNullableSourceType == typeof(object))
            {
                if (nonNullableDestinationType == typeof(string))
                {
                    Emit(OpCodes.Callvirt, ObjectToString);
                    return;
                }

                emitConvert(() => { if (nonNullableDestinationType != typeof(object)) Emit(OpCodes.Unbox_Any, nonNullableDestinationType); }, () => { });
                return;
            }

            if (destination == typeof(object))
            {
                Emit(OpCodes.Box, nonNullableSourceType);
                return;
            }

            if (nonNullableSourceType == nonNullableDestinationType)
            {
                emitConvert(unbox, () => { });
                return;
            }

            if (TypeInfo.IsNumeric(nonNullableSourceType))
            {
                if (nonNullableDestinationType == typeof(char) ||
                    TypeInfo.IsEnum(nonNullableDestinationType))
                {
                    EmitSetOrConvert(nonNullableSourceType, typeof(int));
                    return;
                }

                if (TypeInfo.IsNumeric(nonNullableDestinationType) &&
                    nonNullableSourceType != typeof(decimal) &&
                    nonNullableDestinationType != typeof(decimal))
                {
                    emitConvert(unbox, () => EmitConvToNonNullableNumeric(nonNullableDestinationType));
                    return;
                }
            }

            if (nonNullableDestinationType == typeof(string))
            {
                emitConvert(box, () => Emit(OpCodes.Callvirt, ObjectToString));
                return;
            }

            if (TypeInfo.IsEnum(nonNullableSourceType))
            {
                if (TypeInfo.IsNumeric(nonNullableDestinationType))
                {
                    EmitSetOrConvert(typeof(int), destination);
                    return;
                }

                if (nonNullableDestinationType == typeof(char))
                {
                    emitConvert(box, () =>
                    {
                        Emit(OpCodes.Callvirt, ObjectToString);
                        EmitLdc_I4(0);
                        Emit(OpCodes.Callvirt, typeof(string).GetProperties().First(p => p.GetIndexParameters().Length > 0).GetGetMethod());
                    });
                    return;
                }

                if (TypeInfo.IsEnum(nonNullableDestinationType))
                {
                    emitConvert(box, () =>
                    {
                        Emit(OpCodes.Callvirt, ObjectToString);
                        Emit(OpCodes.Call, EnumParse.MakeGenericMethod(nonNullableDestinationType));
                    });
                    return;
                }
            }

            if (TypeInfo.IsEnum(nonNullableDestinationType))
            {
                if (nonNullableSourceType == typeof(string))
                {
                    emitConvert(unbox, () => Emit(OpCodes.Call, EnumParse.MakeGenericMethod(nonNullableDestinationType)));
                    return;
                }

                if (nonNullableSourceType == typeof(char))
                {
                    emitConvert(box, () =>
                    {
                        Emit(OpCodes.Callvirt, ObjectToString);
                        Emit(OpCodes.Call, EnumParse.MakeGenericMethod(nonNullableDestinationType));
                    });
                    return;
                }
            }

            if (nonNullableDestinationType == typeof(bool))
            {
                if (nonNullableSourceType == typeof(string))
                {
                    emitConvert(unbox, () => Emit(OpCodes.Call, typeof(bool).GetMethod(nameof(bool.Parse), new Type[] { typeof(string) })));
                    return;
                }

                if (TypeInfo.IsNumeric(nonNullableSourceType))
                {
                    if (nonNullableSourceType != typeof(decimal))
                        emitConvert(unbox, () =>
                        {
                            if (nonNullableSourceType != typeof(int))
                                EmitConvToNonNullableNumeric(typeof(int));

                            Emit(OpCodes.Ldc_I4_0);
                            Emit(OpCodes.Cgt);
                        });
                    else
                        emitConvert(unbox, () =>
                        {
                            Emit(OpCodes.Call, typeof(decimal).GetMethod(nameof(decimal.ToInt32), new Type[] { typeof(decimal) }));
                            Emit(OpCodes.Ldc_I4_0);
                            Emit(OpCodes.Cgt);
                        });

                    return;
                }
            }

            ConstructorInfo nonNullableDestinationTypeConstructor = nonNullableDestinationType.GetConstructor(new[] { nonNullableSourceType });
            if (nonNullableDestinationTypeConstructor != null)
            {
                emitConvert(unbox, () => Emit(OpCodes.Newobj, nonNullableDestinationTypeConstructor));
                return;
            }

            MethodInfo convertToNonNullable = nonNullableSourceType.GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
                m.Name.Contains(To + nonNullableSourceType.Name) &&
                m.ReturnType == nonNullableDestinationType &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == nonNullableDestinationType);

            if (convertToNonNullable != null)
            {
                emitConvert(unbox, () => Emit(OpCodes.Call, convertToNonNullable));
                return;
            }

            convertToNonNullable = typeof(Converters).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
                m.Name.Contains(To + nonNullableSourceType.Name) &&
                m.ReturnType == nonNullableDestinationType &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == nonNullableDestinationType);

            if (convertToNonNullable != null)
            {
                emitConvert(unbox, () => Emit(OpCodes.Call, convertToNonNullable));
                return;
            }

            convertToNonNullable = typeof(Convert).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
                m.Name.Contains(To + nonNullableSourceType.Name) &&
                m.ReturnType == nonNullableDestinationType &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == nonNullableDestinationType);

            if (convertToNonNullable != null)
            {
                emitConvert(unbox, () => Emit(OpCodes.Call, convertToNonNullable));
                return;
            }

            convertToNonNullable = typeof(IConvertible).GetMethods().FirstOrDefault(m =>
                m.Name.Contains(To + nonNullableDestinationType.Name) &&
                m.ReturnType == nonNullableDestinationType &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == typeof(IFormatProvider));

            if (convertToNonNullable != null)
            {
                emitConvert(box, () =>
                {
                    Emit(OpCodes.Ldnull);
                    Emit(OpCodes.Call, convertToNonNullable);
                });
                return;
            }

            throw new Exception($"Cannot convert from {nonNullableSourceType} to {destination}");
        }

        public static bool CanEmitLoadAndSetValue(
            MemberInfo source,
            MemberInfo destination)
        {
            if (source == null || 
                source.Type == null ||
                destination == null || 
                destination.Type == null ||
                !source.HasGetMethod ||
                !destination.HasSetMethod)
                return false;

            return CanEmitSetOrConvert(source.Type, destination.Type);
        }

        public void EmitLoadAndSetValue(
            Action load,
            MemberInfo source,
            MemberInfo destination)
        {
            if (load == null) throw new ArgumentException(nameof(load));
            if (source == null || source.Type == null) throw new ArgumentException(nameof(source));
            if (destination == null || destination.Type == null) throw new ArgumentException(nameof(destination));

            if (source.Type == destination.Type)
            {
                load();
                EmitSetMemberValue(destination);

                return;
            }

            if (TypeInfo.IsBuiltIn(source.Type) != TypeInfo.IsBuiltIn(destination.Type))
                throw new Exception($"Cannot load and set from {source} to {destination}");

            load();
            EmitSetOrConvert(source.Type, destination.Type);
            EmitSetMemberValue(destination);
        }
    }
}