using Air.Mapper;
using AutoFixture;
using AutoFixture.Kernel;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Xunit;
using Xunit.Abstractions;
using Emit = Air.Reflection.Emit;
using MemberInfo = Air.Reflection.MemberInfo;
using TypeInfo = Air.Reflection.TypeInfo;

namespace Internal
{
    public class FromTo_N0<S> where S : new()
    {
        private readonly ITestOutputHelper Console;
        private Fixture Fixture { get; }

        public FromTo_N0(ITestOutputHelper console)
        {
            Console = console;
            Fixture = new Fixture();
        }

        internal DynamicMethod CompileMethod(
            Type sourceType,
            List<MemberInfo> sourceMembers,
            Type destinationType,
            List<MemberInfo> destinationMembers)
        {
            var returnValue = new DynamicMethod($"{nameof(Air)}{Guid.NewGuid():N}", destinationType, new[] { sourceType }, false);
            var il = new Emit.ILGenerator(returnValue.GetILGenerator(), true);

            var sourceUnderlyingType = Nullable.GetUnderlyingType(sourceType);
            var destinationUnderlyingType = Nullable.GetUnderlyingType(destinationType);

            var destination = destinationUnderlyingType ?? destinationType;

            LocalBuilder sourceLocal = null;
            LocalBuilder destinationLocal = null;

            if (sourceUnderlyingType != null)
            {
                sourceLocal = il.DeclareLocal(sourceUnderlyingType);
                il.EmitLoadArgument(sourceType, 0);
                il.EmitCallMethod(sourceType.GetProperty("Value").GetGetMethod());
                il.EmitStoreLocal(sourceLocal);
            }

            if (!destinationType.IsValueType)
            {
                il.EmitInit(destinationType);
            }
            else
            {
                destinationLocal = il.DeclareLocal(destination);
                il.EmitInit(destinationLocal);
            }

            for (int i = 0; i < sourceMembers.Count; i++)
            {
                if (!destination.IsValueType && !destinationMembers[i].IsStatic)
                    il.Emit(OpCodes.Dup);

                il.EmitLoadAndSetValue(
                () =>
                {
                    if (destinationType.IsValueType && !destinationMembers[i].IsStatic)
                        il.EmitLoadLocal(destinationLocal, true);

                    if (!sourceMembers[i].IsStatic)
                    {
                        if (sourceUnderlyingType == null)
                            il.EmitLoadArgument(sourceType, 0);
                        else
                            il.EmitLoadLocal(sourceLocal, true);
                    }

                    il.EmitLoadMemberValue(sourceMembers[i]);
                },
                sourceMembers[i],
                destinationMembers[i]);
            }

            if (destinationUnderlyingType != null)
            {
                il.EmitLoadLocal(destinationLocal, false);
                il.Emit(OpCodes.Newobj, destinationType.GetConstructor(new Type[] { destinationUnderlyingType }));
            }
            else if (destination.IsValueType)
            {
                il.EmitLoadLocal(destinationLocal, false);
            }

            il.Emit(OpCodes.Ret);

            //var log = il.GetLog().ToString();

            return returnValue;
        }

        private S NewSource()
        {
            var instance = Fixture.Create<TC0_I0_Members>();

            var source = Mapper<TC0_I0_Members, S>.Map(instance);
            source = source != null ? source : new S();

            if (source != null)
                return source;

            var nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(S));
            if (nullableUnderlyingType != null)
            {
                var undelyingInstance = Activator.CreateInstance(nullableUnderlyingType);
                source = (S)Activator.CreateInstance(typeof(S), new[] { undelyingInstance });
            }

            return source;
        }

        private S NewSource(string fixtureMember, object fixtureMemberValue = null)
        {
            var instance = new TC0_I0_Members();
            var member = TypeInfo.GetMembers(typeof(TC0_I0_Members), true).First(m => m.Name == fixtureMember);

            if (member.HasSetMethod)
            {
                var setMethod = ((PropertyInfo)member.MemberTypeInfo).GetSetMethod();
                setMethod.Invoke(instance, new[] { ConvertTo(typeof(object), member.Type, fixtureMemberValue) ?? FixtureCreate(member.Type) });
            }

            var source = Mapper<TC0_I0_Members, S>.Map(instance);
            source = source != null ? source : new S();

            if (source != null)
                return source;

            var nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(S));
            if (nullableUnderlyingType != null)
            {
                var undelyingInstance = Activator.CreateInstance(nullableUnderlyingType);
                source = (S)Activator.CreateInstance(typeof(S), new[] { undelyingInstance });
            }

            return source;
        }

        private static bool CanSerialize<D>(S source, D destination)
        {
            return JsonConvert.SerializeObject(source) != null &&
                JsonConvert.SerializeObject(destination) != null;
        }

        private static void AssertEqualsOrDefault<D>(
            S source,
            D destination,
            bool hasReadonlyMembers) where D : new()
        {
            if (hasReadonlyMembers)
                return;

            if (hasReadonlyMembers)
                return;

            Assert.True(CanSerialize(source, destination));
            Assert.True(CompareEquals(source, destination));
        }

        private static bool CompareEquals<L, R>(L left, R right)
        {
            var sourceMembers = TypeInfo.GetMembers(typeof(L), true);
            var destinationMembers = TypeInfo.GetMembers(typeof(R), true);

            foreach (MemberInfo sourceMember in sourceMembers)
            {
                var sourceMemberValue = sourceMember.GetValue(left);
                var destinationMemberValue = destinationMembers.First(m => m.Name == sourceMember.Name).GetValue(right);

                if (!object.Equals(sourceMemberValue, destinationMemberValue))
                    return false;
            }

            return true;
        }

        private static readonly MethodInfo ObjectToString = typeof(object).GetMethod(nameof(object.ToString), Type.EmptyTypes);

        private static void AssertMembersEqual<L, R>(L left, string leftMemberName, R right, string rightMemberName)
        {
            var leftMember = TypeInfo.GetMembers(typeof(L), true).First(m => m.Name == leftMemberName);
            var rightMember = TypeInfo.GetMembers(typeof(R), true).First(m => m.Name == rightMemberName);

            if (leftMember.Type == rightMember.Type)
            {
                Assert.Equal(leftMember.GetValue(left), rightMember.GetValue(right));
                return;
            }

            var rightMemberValue = rightMember.GetValue(right);
            var leftMemberValue = leftMember.GetValue(left);

            if (rightMember.IsEnum)
            {
                if (leftMember.IsNumeric)
                {
                    rightMemberValue = Convert.ChangeType(rightMemberValue, Enum.GetUnderlyingType(rightMember.Type));
                    Assert.Equal(leftMemberValue.ToString(), rightMemberValue.ToString());
                }
                else if (leftMember.Type == typeof(string))
                {
                    Assert.Equal(leftMemberValue, ObjectToString.Invoke(null, new[] { rightMemberValue }));
                }
            }
            else
            {
                Assert.True(ConvertTo(leftMember.Type, rightMember.Type, leftMemberValue).Equals(rightMemberValue));
            }
        }

        private static object ConvertTo(Type source, Type destination, object value)
        {
            var method = GetConvertToMethodInfo(source, destination);
            return method?.Invoke(null, new[] { value });
        }

        private static MethodInfo GetConvertToMethodInfo(
            Type nonNullableSourceType,
            Type nonNullableDestinationType)
        {
            return typeof(Convert).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(m =>
                m.Name.Contains($"To{nonNullableDestinationType.Name}") &&
                m.ReturnType == nonNullableDestinationType &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == nonNullableSourceType);
        }

        public object FixtureCreate(Type type)
        {
            var context = new SpecimenContext(Fixture);
            return context.Resolve(type);
        }

        private bool ConvertWillThrowException(
            Type nonNullableSourceType,
            Type nonNullableDestinationType)
        {
            var source = FixtureCreate(nonNullableSourceType);
            var method = GetConvertToMethodInfo(nonNullableSourceType, nonNullableDestinationType);

            if (method == null)
                return false;

            try
            {
                method.Invoke(null, new[] { source });
                return false;
            }
            catch
            {
                return true;
            }
        }

        private static Type GetUndelyingType(Type type) =>
            Nullable.GetUnderlyingType(type) ?? type;

        private void MapperConvert<D>(
            Type sourceType,
            List<MemberInfo> sourceMembers,
            Type destinationType,
            List<MemberInfo> destinationMembers) where D : new()
        {
            Random random = new Random();

            for (int s = 0; s < sourceMembers.Count; s++)
            {
                for (int d = 0; d < destinationMembers.Count; d++)
                {
                    if (ConvertWillThrowException(
                            GetUndelyingType(sourceMembers[s].Type),
                            GetUndelyingType(destinationMembers[d].Type)))
                        continue;

                    if (!Emit.ILGenerator.CanEmitLoadAndSetValue(sourceMembers[s], destinationMembers[d]))
                        continue;

                    if (GetConvertToMethodInfo(sourceMembers[s].Type, destinationMembers[d].Type) == null)
                        continue;

                    object fixtureMemberValue = null;

                    if (sourceMembers[s].IsNumeric &&
                        (destinationMembers[d].IsNumeric || destinationMembers[d].Type == typeof(char)))
                        fixtureMemberValue = random.Next(0, 127);

                    if (sourceMembers[s].IsNumeric && destinationMembers[d].IsEnum)
                        fixtureMemberValue = 1;

                    if (destinationMembers[d].IsEnum)
                    {
                        if (sourceMembers[s].Type == typeof(string))
                            fixtureMemberValue = "B";

                        if (sourceMembers[s].Type == typeof(char))
                            fixtureMemberValue = 'B';
                    }

                    var source = NewSource(
                        sourceMembers[s].Name,
                        fixtureMemberValue ?? ConvertTo(sourceMembers[s].Type, destinationMembers[d].Type, FixtureCreate(sourceMembers[s].Type)));

                    var map = (Func<S, D>)CompileMethod(
                            sourceType,
                            new List<MemberInfo> { sourceMembers[s] },
                            destinationType,
                            new List<MemberInfo> { destinationMembers[d] })
                        .CreateDelegate(typeof(Func<S, D>));

                    var destination = map(source);

                    AssertMembersEqual(source, sourceMembers[s].Name, destination, destinationMembers[d].Name);
                }
            }
        }

        private static void GetMembers(Type sourceType, Type destinationType, out List<MemberInfo> sourceMembers, out List<MemberInfo> destinationMembers)
        {
            var outSourceMembers = TypeInfo.GetMembers(sourceType, true);
            var outDestinationMembers = TypeInfo.GetMembers(destinationType, true);

            sourceMembers = outSourceMembers.Where(s => outDestinationMembers.Any(m => m.Name == s.Name)).OrderBy(o => o.Name).ToList();
            destinationMembers = outDestinationMembers.Where(s => outSourceMembers.Any(m => m.Name == s.Name)).OrderBy(o => o.Name).ToList();
        }

        public void ToClass<D>(bool hasReadonlyMembers, bool hasStaticMembers) where D : new()
        {
            if (hasReadonlyMembers)
                return;

            var sourceType = typeof(S);
            var destinationType = typeof(D);

            GetMembers(sourceType, destinationType, out var sourceMembers, out var destinationMembers);

            var map = (Func<S, D>)CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers).CreateDelegate(typeof(Func<S, D>));

            S source = NewSource();
            D destination = new D();

            // =======
            destination = map(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            MapperConvert<D>(sourceType, sourceMembers, destinationType, destinationMembers);
        }

        public void ToStruct<D>(bool hasReadonlyMembers, bool hasStaticMembers) where D : struct
        {
            if (hasReadonlyMembers)
                return;

            var sourceType = typeof(S);
            var destinationType = typeof(D);

            GetMembers(sourceType, destinationType, out var sourceMembers, out var destinationMembers);

            var map = (Func<S, D>)CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers).CreateDelegate(typeof(Func<S, D>));

            S source = NewSource();
            D destination = new D();

            // =======
            destination = map(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            MapperConvert<D>(sourceType, sourceMembers, destinationType, destinationMembers);
        }

        public void ToNullableStruct<D>(bool hasReadonlyMembers, bool hasStaticMembers) where D : struct
        {
            if (hasReadonlyMembers)
                return;

            var sourceType = typeof(S);
            var destinationType = typeof(D?);

            GetMembers(sourceType, destinationType, out var sourceMembers, out var destinationMembers);

            var map = (Func<S, D?>)CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers).CreateDelegate(typeof(Func<S, D?>));

            S source = NewSource();
            D? destination = new D?();

            // =======
            destination = map(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            MapperConvert<D?>(sourceType, sourceMembers, destinationType, destinationMembers);
        }
    }
}
