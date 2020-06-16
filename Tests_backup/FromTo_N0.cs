using Air.Reflection;
using AutoFixture;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Xunit;
using Xunit.Abstractions;
using Emit = Air.Reflection.Emit;
using static Air.Compare.Members;
using Newtonsoft.Json;
using Air.Mapper;

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
            IEnumerable<MemberInfo> sourceMembers,
            Type destinationType,
            IEnumerable<MemberInfo> destinationMembers)
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
                il.Emit(OpCodes.Call, sourceType.GetProperty("Value").GetGetMethod());
                il.EmitStoreLocal(sourceLocal);
            }

            destinationLocal = il.DeclareLocal(destination);
            il.EmitLoadLocal(destinationLocal, false);
            il.EmitInit(destination);

            var sourceMembersList = sourceMembers.ToList();
            var destinationMembersList = destinationMembers.ToList();

            for (int i = 0; i < sourceMembersList.Count; i++)
            {
                if (Emit.ILGenerator.CanEmitLoadAndSetValue(sourceMembersList[i], destinationMembersList[i]))
                    continue;

                if (!destination.IsValueType)
                    il.Emit(OpCodes.Dup);

                il.EmitLoadAndSetValue(
                () =>
                {
                    if (!destination.IsValueType && !destinationMembersList[i].IsStatic)
                        il.EmitLoadLocal(destinationLocal, true);

                    if (!sourceMembersList[i].IsStatic)
                    {
                        if (sourceUnderlyingType == null)
                            il.EmitLdarga(0);
                        else
                            il.EmitLoadLocal(sourceLocal, true);
                    }

                    il.EmitLoadMemberValue(sourceMembersList[i]);
                },
                sourceMembersList[i],
                destinationMembersList[i]);
            }


            if (destinationUnderlyingType != null)
            {
                il.EmitLoadLocal(destinationLocal, false);
                il.Emit(OpCodes.Newobj, destinationType.GetConstructor(new Type[] { destinationUnderlyingType }));
            }

            il.EmitLdloc(0);
            il.Emit(OpCodes.Ret);

            var log = il.GetLog().ToString();
            return returnValue;
        }

        private S NewSource(bool int32MemberOnly = false)
        {
            var members = int32MemberOnly ?
                new TC0_I0_Members { Int32Member = Fixture.Create<int>() } :
                Fixture.Create<TC0_I0_Members>();

            var source = Mapper<TC0_I0_Members, S>.Map(members);
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

        private bool CanSerialize<D>(S source, D destination)
        {
            return JsonConvert.SerializeObject(source) != null &&
                JsonConvert.SerializeObject(destination) != null;
        }

        private void AssertEqualsOrDefaultReadonly<D>(S source, D destination) where D : new()
        {
            Assert.True(CanSerialize(source, destination));

            if (Nullable.GetUnderlyingType(typeof(D)) != null)
                Assert.Null(destination);
            if (typeof(D).IsValueType)
                Assert.True(CompareEquals(new D(), destination));
            else
                Assert.Null(destination);
        }

        private void AssertEqualsOrDefault<D>(
           S[] source,
           D[] destination,
           bool hasReadonlyMembers,
           bool hasStaticMembers) where D : new()
        {
            if (hasStaticMembers)
                return;

            for (int i = 0; i < source.Length; i++)
                AssertEqualsOrDefault(source[i], destination[i], hasReadonlyMembers);
        }

        private void AssertEqualsOrDefault<D>(
            S source,
            D destination,
            bool hasReadonlyMembers) where D : new()
        {
            if (hasReadonlyMembers)
                AssertEqualsOrDefaultReadonly(source, destination);

            if (hasReadonlyMembers)
                return;

            Assert.True(CanSerialize(source, destination));

            if (source == null && Nullable.GetUnderlyingType(typeof(D)) == null && typeof(D).IsValueType)
                Assert.True(CompareEquals(destination, new D()));
            else
                Assert.True(CompareEquals(source, destination));
        }

        private void AssertDefaultReadonly<D>(S source, D destination) where D : new()
        {
            Assert.True(CanSerialize(source, destination));

            if (Nullable.GetUnderlyingType(typeof(S)) != null)
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
            else if (typeof(S).IsValueType)
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
            else
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
        }

        private void AssertDefault<D>(
            S source,
            D destination,
            bool hasReadonlyMembers) where D : new()
        {
            if (hasReadonlyMembers)
                AssertDefaultReadonly(source, destination);

            if (hasReadonlyMembers)
                return;

            Assert.True(CanSerialize(source, destination));

            if (Nullable.GetUnderlyingType(typeof(S)) != null)
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
            else if (typeof(S).IsValueType)
            {
                Assert.True(CompareEquals(source, destination, ignoreDefaultLeftValues: true));
            }
            else
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
        }

        private object MemberValue<T>(T source, string memberName)
        {
            return TypeInfo.GetMembers(typeof(T), true)
                .First(m => m.Name == memberName)
                .GetValue(source);
        }

        private void AssertIntToStringEquals<D>(S source, D destination)
        {
            Assert.True(
                CompareEquals(
                    MemberValue(source, "Int32Member"),
                    MemberValue(destination, "StringMember"),
                    ignoreDefaultLeftValues: true,
                    useConvert: true));
        }

        private void AssertDefaultStringMemberValue<D>(S source, D destination) where D : new()
        {
            if (Nullable.GetUnderlyingType(typeof(S)) != null)
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                {
                    if (source != null)
                        Assert.Null(MemberValue(destination, "StringMember"));
                }
                else
                    Assert.Null(destination);
            }
            else if (typeof(S).IsValueType)
            {
                AssertIntToStringEquals(source, destination);
            }
            else
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
        }

        private void AssertIntToDecimalEquals<D>(S source, D destination)
        {
            Assert.True(
                CompareEquals(
                    MemberValue(source, "Int32Member"),
                    MemberValue(destination, "DecimalMember"),
                    ignoreDefaultLeftValues: true,
                    useConvert: true));
        }

        private void AssertDefaultDecimalMemberValue<D>(S source, D destination) where D : new()
        {
            if (Nullable.GetUnderlyingType(typeof(S)) != null)
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                {
                    if (source != null)
                        Assert.True(
                            CompareEquals(
                                0,
                                MemberValue(destination, "DecimalMember"),
                                ignoreDefaultRightValues: true,
                                useConvert: true));

                }
                else
                    Assert.Null(destination);
            }
            else if (typeof(S).IsValueType)
            {
                AssertIntToDecimalEquals(source, destination);
            }
            else
            {
                if (Nullable.GetUnderlyingType(typeof(D)) != null)
                    Assert.Null(destination);
                else if (typeof(D).IsValueType)
                    Assert.True(CompareEquals(new D(), destination));
                else
                    Assert.Null(destination);
            }
        }

        private void MapperConvert<D>(bool hasReadonlyMembers) where D : new()
        {
            //if (hasReadonlyMembers)
            //{
            //    Assert.Throws<InvalidOperationException>(() => Mapper<S, D>.CompileActionRef(o => o
            //        .Ignore(i => i)
            //        .Map("Int32Member", "StringMember")));

            //    Assert.Throws<InvalidOperationException>(() => Mapper<S, D>.CompileFunc(o => o
            //        .Ignore(i => i)
            //        .Map("Int32Member", "DecimalMember")));

            //    return;
            //}

            //// =======
            //var convertActionRef = Mapper<S, D>.CompileActionRef(o => o
            //    .Ignore(i => i)
            //    .Map("Int32Member", "StringMember"));

            //var convertFunc = Mapper<S, D>.CompileFunc(o => o
            //    .Ignore(i => i)
            //    .Map("Int32Member", "DecimalMember"));

            //// =======
            //var source = NewSource(true);
            //var destination = new D();
            //convertActionRef(source, ref destination);
            //AssertIntToStringEquals(source, destination);

            //destination = default;
            //convertActionRef(source, ref destination);
            //AssertIntToStringEquals(source, destination);

            //destination = convertFunc(source);
            //AssertIntToDecimalEquals(source, destination);

            //// =======
            //source = DefaultSource();
            //convertActionRef(source, ref destination);
            //AssertDefaultStringMemberValue(source, destination);

            //destination = convertFunc(source);
            //AssertDefaultDecimalMemberValue(source, destination);
        }

        public void ToClass<D>(bool hasReadonlyMembers, bool hasStaticMembers) where D : new()
        {
            var sourceType = typeof(S);
            var destinationType = typeof(D);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var map = (Func<S, D>)CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers).CreateDelegate(typeof(Func<S, D>));

            S source = NewSource();
            D destination = new D();

            // =======
            destination = map(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            MapperConvert<D>(hasReadonlyMembers);
        }

        public void ToStruct<D>(bool hasReadonlyMembers, bool hasStaticMembers) where D : struct
        {
            var sourceType = typeof(S);
            var destinationType = typeof(D);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var map = (Func<S, D>)CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers).CreateDelegate(typeof(Func<S, D>));

            S source = NewSource();
            D destination = new D();

            // =======
            destination = map(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            MapperConvert<D>(hasReadonlyMembers);
        }

        public void ToNullableStruct<D>(bool hasReadonlyMembers, bool hasStaticMembers) where D : struct
        {
            var sourceType = typeof(S);
            var destinationType = typeof(D?);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var map = (Func<S, D?>)CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers).CreateDelegate(typeof(Func<S, D?>));

            S source = NewSource();
            D? destination = new D?();

            // =======
            destination = map(source);
            AssertEqualsOrDefault(source, destination, hasReadonlyMembers);

            MapperConvert<D>(hasReadonlyMembers);
        }
    }
}
