using Air.Reflection;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Xunit;
using Xunit.Abstractions;
using static Air.Compare.Members;
using static Test.Models;
using Emit = Air.Reflection.Emit;

namespace Test
{
    public class ILGenerator
    {
        readonly ITestOutputHelper Console;
        Fixture Fixture { get; }

        public ILGenerator(ITestOutputHelper console)
        {
            Console = console;
            Fixture = new Fixture();
        }

        DynamicMethod CompileMethod(
            Type sourceType,
            IEnumerable<MemberInfo> sourceMembers,
            Type destinationType,
            IEnumerable<MemberInfo> destinationMembers)
        {
            var returnValue = new DynamicMethod($"{nameof(Air)}{Guid.NewGuid():N}", destinationType, new[] { sourceType }, false);
            var il = new Emit.ILGenerator(returnValue.GetILGenerator(), true);

            Action load_S;
            Action load_D;
            Action store_D;

            LocalBuilder destination = il.DeclareLocal(destinationType);

            if (TypeInfo.IsNonBuiltInStruct(sourceType))
                load_S = () => { il.Emit(OpCodes.Ldarga_S, destination); };
            else
                load_S = () => { il.Emit(OpCodes.Ldarg_S, destination); };


            if (TypeInfo.IsNonBuiltInStruct(destinationType))
            {
                load_D = () => { il.Emit(OpCodes.Ldloca_S, destination); };
                store_D = () => { };
                il.Emit(OpCodes.Ldloca_S, destination);
                il.EmitInit(destinationType);
            }
            else
            {
                load_D = () => { il.Emit(OpCodes.Ldloc_S, destination); };
                store_D = () => { il.Emit(OpCodes.Stloc_S, destination); };
                il.EmitInit(destinationType);
                store_D();
            }

            var sourceMembersList = sourceMembers.ToList();
            var destinationMembersList = destinationMembers.ToList();

            for (int i = 0; i < sourceMembersList.Count; i++)
            {
                if (Emit.ILGenerator.CanEmitLoadAndSetValue(sourceMembersList[i], destinationMembersList[i]))
                {
                    il.EmitLoadAndSetValue(
                        () =>
                        {
                            if (!destinationMembersList[i].IsStatic)
                                load_D();

                            if (!sourceMembersList[i].IsStatic)
                                load_S();

                            il.EmitLoadMemberValue(sourceMembersList[i]);
                        },
                        sourceMembersList[i],
                        destinationMembersList[i]);
                }
            }

            il.EmitLdloc(0);
            il.Emit(OpCodes.Ret);

            var log = il.GetLog().ToString();
            return returnValue;
        }

        [Fact]
        public void EmitLoadDefaultValueForBuiltInStruct()
        {
            Type destinationType = typeof(int);

            var method = new DynamicMethod($"{nameof(Air)}{Guid.NewGuid().ToString("N")}", destinationType, null, false);
            var il = new Emit.ILGenerator(method.GetILGenerator(), true);
            il.EmitLoadDefaultValue(destinationType);
            il.Emit(OpCodes.Ret);
            Func<int> func = (Func<int>)method.CreateDelegate(typeof(Func<int>));

            var log = il.GetLog().ToString();

            var destination = func();

            Assert.Equal(default(int), destination);
        }

        [Fact]
        public void EmitLoadDefaultValueForNullable()
        {
            Type destinationType = typeof(int?);

            var method = new DynamicMethod($"{nameof(Air)}{Guid.NewGuid().ToString("N")}", destinationType, null, false);
            var il = new Emit.ILGenerator(method.GetILGenerator(), true);
            il.EmitLoadDefaultValue(destinationType);
            il.Emit(OpCodes.Ret);
            Func<int?> func = (Func<int?>)method.CreateDelegate(typeof(Func<int?>));

            var log = il.GetLog().ToString();

            var destination = func();

            Assert.Equal(default(int?), destination);
        }

        [Fact]
        public void EmitSetValueFromToStatic()
        {
            var nonStaticSourceType = typeof(TClassMembers);
            var sourceType = typeof(TStaticMembers);
            var destinationType = typeof(TStaticMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var nonStaticToStatic = CompileMethod(nonStaticSourceType, TypeInfo.GetMembers(nonStaticSourceType), sourceType, TypeInfo.GetMembers(sourceType));
            Func<TClassMembers, TStaticMembers> nonStaticToStaticFunc =
                (Func<TClassMembers, TStaticMembers>)nonStaticToStatic.CreateDelegate(typeof(Func<TClassMembers, TStaticMembers>));

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TStaticMembers, TStaticMembers> func =
                (Func<TStaticMembers, TStaticMembers>)method.CreateDelegate(typeof(Func<TStaticMembers, TStaticMembers>));

            var nonStaticSource = Fixture.Create<TClassMembers>();
            var source = nonStaticToStaticFunc(nonStaticSource);

            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromStaticToClass()
        {
            var nonStaticSourceType = typeof(TClassMembers);
            var sourceType = typeof(TStaticMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var nonStaticToStatic = CompileMethod(nonStaticSourceType, TypeInfo.GetMembers(nonStaticSourceType), sourceType, TypeInfo.GetMembers(sourceType));
            Func<TClassMembers, TStaticMembers> nonStaticToStaticFunc =
                (Func<TClassMembers, TStaticMembers>)nonStaticToStatic.CreateDelegate(typeof(Func<TClassMembers, TStaticMembers>));

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TStaticMembers, TClassMembers> func =
                (Func<TStaticMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TStaticMembers, TClassMembers>));

            var nonStaticSource = Fixture.Create<TClassMembers>();
            var source = nonStaticToStaticFunc(nonStaticSource);

            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromStaticToNullableMembers()
        {
            var nonStaticSourceType = typeof(TClassMembers);
            var sourceType = typeof(TStaticMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var nonStaticToStatic = CompileMethod(nonStaticSourceType, TypeInfo.GetMembers(nonStaticSourceType), sourceType, TypeInfo.GetMembers(sourceType));
            Func<TClassMembers, TStaticMembers> nonStaticToStaticFunc =
                (Func<TClassMembers, TStaticMembers>)nonStaticToStatic.CreateDelegate(typeof(Func<TClassMembers, TStaticMembers>));

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TStaticMembers, TNullableMembers> func =
                (Func<TStaticMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TStaticMembers, TNullableMembers>));

            var nonStaticSource = Fixture.Create<TClassMembers>();
            var source = nonStaticToStaticFunc(nonStaticSource);

            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromStaticToStaticNullableMembers()
        {
            var nonStaticSourceType = typeof(TClassMembers);
            var sourceType = typeof(TStaticMembers);
            var destinationType = typeof(TNullableStaticMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var nonStaticToStatic = CompileMethod(nonStaticSourceType, TypeInfo.GetMembers(nonStaticSourceType), sourceType, TypeInfo.GetMembers(sourceType));
            Func<TClassMembers, TStaticMembers> nonStaticToStaticFunc =
                (Func<TClassMembers, TStaticMembers>)nonStaticToStatic.CreateDelegate(typeof(Func<TClassMembers, TStaticMembers>));

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TStaticMembers, TNullableStaticMembers> func =
                (Func<TStaticMembers, TNullableStaticMembers>)method.CreateDelegate(typeof(Func<TStaticMembers, TNullableStaticMembers>));

            var nonStaticSource = Fixture.Create<TClassMembers>();
            var source = nonStaticToStaticFunc(nonStaticSource);

            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromToStaticNullableMembers()
        {
            var nonStaticSourceType = typeof(TClassMembers);
            var sourceType = typeof(TNullableStaticMembers);
            var destinationType = typeof(TNullableStaticMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var nonStaticToStatic = CompileMethod(nonStaticSourceType, TypeInfo.GetMembers(nonStaticSourceType), sourceType, TypeInfo.GetMembers(sourceType));
            Func<TClassMembers, TNullableStaticMembers> nonStaticToStaticFunc =
                (Func<TClassMembers, TNullableStaticMembers>)nonStaticToStatic.CreateDelegate(typeof(Func<TClassMembers, TNullableStaticMembers>));

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TNullableStaticMembers, TNullableStaticMembers> func =
                (Func<TNullableStaticMembers, TNullableStaticMembers>)method.CreateDelegate(typeof(Func<TNullableStaticMembers, TNullableStaticMembers>));

            var nonStaticSource = Fixture.Create<TClassMembers>();
            var source = nonStaticToStaticFunc(nonStaticSource);

            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromStaticNullableMembersToStatic()
        {
            var nonStaticSourceType = typeof(TClassMembers);
            var sourceType = typeof(TNullableStaticMembers);
            var destinationType = typeof(TStaticMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var nonStaticToStatic = CompileMethod(nonStaticSourceType, TypeInfo.GetMembers(nonStaticSourceType), sourceType, TypeInfo.GetMembers(sourceType));
            Func<TClassMembers, TNullableStaticMembers> nonStaticToStaticFunc =
                (Func<TClassMembers, TNullableStaticMembers>)nonStaticToStatic.CreateDelegate(typeof(Func<TClassMembers, TNullableStaticMembers>));

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TNullableStaticMembers, TStaticMembers> func =
                (Func<TNullableStaticMembers, TStaticMembers>)method.CreateDelegate(typeof(Func<TNullableStaticMembers, TStaticMembers>));

            var nonStaticSource = Fixture.Create<TClassMembers>();
            var source = nonStaticToStaticFunc(nonStaticSource);

            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromStaticNullableMembersToClass()
        {
            var nonStaticSourceType = typeof(TClassMembers);
            var sourceType = typeof(TNullableStaticMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var nonStaticToStatic = CompileMethod(nonStaticSourceType, TypeInfo.GetMembers(nonStaticSourceType), sourceType, TypeInfo.GetMembers(sourceType));
            Func<TClassMembers, TNullableStaticMembers> nonStaticToStaticFunc =
                (Func<TClassMembers, TNullableStaticMembers>)nonStaticToStatic.CreateDelegate(typeof(Func<TClassMembers, TNullableStaticMembers>));

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TNullableStaticMembers, TClassMembers> func =
                (Func<TNullableStaticMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TNullableStaticMembers, TClassMembers>));

            var nonStaticSource = Fixture.Create<TClassMembers>();
            var source = nonStaticToStaticFunc(nonStaticSource);

            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromStaticNullableMembersToNullableMembers()
        {
            var nonStaticSourceType = typeof(TClassMembers);
            var sourceType = typeof(TNullableStaticMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var nonStaticToStatic = CompileMethod(nonStaticSourceType, TypeInfo.GetMembers(nonStaticSourceType), sourceType, TypeInfo.GetMembers(sourceType));
            Func<TClassMembers, TNullableStaticMembers> nonStaticToStaticFunc =
                (Func<TClassMembers, TNullableStaticMembers>)nonStaticToStatic.CreateDelegate(typeof(Func<TClassMembers, TNullableStaticMembers>));

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TNullableStaticMembers, TNullableMembers> func =
                (Func<TNullableStaticMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TNullableStaticMembers, TNullableMembers>));

            var nonStaticSource = Fixture.Create<TClassMembers>();
            var source = nonStaticToStaticFunc(nonStaticSource);

            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromToClass()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromToNullableMembers()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TNullableMembers, TNullableMembers> func =
                (Func<TNullableMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TNullableMembers>));

            var source = Fixture.Create<TNullableMembers>();
            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromNullableMembersToClass()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TNullableMembers, TClassMembers> func =
                (Func<TNullableMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TClassMembers>));

            var source = Fixture.Create<TNullableMembers>();
            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromEnumToDtoEnum()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.EnumType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.DtoEnumType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            source.EnumType = EnumType.B;

            var destination = func(source);

            Assert.Equal(source.EnumType.ToString(), destination.DtoEnumType.ToString());
        }

        [Fact]
        public void EmitSetValueFromEnumToNullableDtoEnum()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.EnumType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TNullableMembers.DtoEnumType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TNullableMembers> func =
                (Func<TClassMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TNullableMembers>));

            var source = Fixture.Create<TClassMembers>();
            source.EnumType = EnumType.B;

            var destination = func(source);

            Assert.Equal(source.EnumType.ToString(), destination.DtoEnumType.ToString());
        }

        [Fact]
        public void EmitSetValueFromNullableEnumToDtoEnum()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TNullableMembers.EnumType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.DtoEnumType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TNullableMembers, TClassMembers> func =
                (Func<TNullableMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TClassMembers>));

            var source = Fixture.Create<TNullableMembers>();
            source.EnumType = EnumType.B;

            var destination = func(source);

            Assert.Equal(source.EnumType.ToString(), destination.DtoEnumType.ToString());
        }

        [Fact]
        public void EmitSetValueFromNullableEnumToNullableDtoEnum()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TNullableMembers.EnumType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TNullableMembers.DtoEnumType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TNullableMembers, TNullableMembers> func =
                (Func<TNullableMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TNullableMembers>));

            var source = Fixture.Create<TNullableMembers>();
            source.EnumType = EnumType.B;

            var destination = func(source);

            Assert.Equal(source.EnumType.ToString(), destination.DtoEnumType.ToString());
        }

        [Fact]
        public void EmitSetValueFromClassToNullableMembers()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TClassMembers, TNullableMembers> func =
                (Func<TClassMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TNullableMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromClassToStruct()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TStructMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TClassMembers, TStructMembers> func =
                (Func<TClassMembers, TStructMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TStructMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromStructToClass()
        {
            var sourceType = typeof(TStructMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMembers = TypeInfo.GetMembers(sourceType);
            var destinationMembers = TypeInfo.GetMembers(destinationType);

            var typeToStructMethod = CompileMethod(
                typeof(TClassMembers),
                TypeInfo.GetMembers(typeof(TClassMembers)),
                typeof(TStructMembers),
                TypeInfo.GetMembers(typeof(TStructMembers)));

            Func<TClassMembers, TStructMembers> typeToStructFunc =
                (Func<TClassMembers, TStructMembers>)typeToStructMethod.CreateDelegate(typeof(Func<TClassMembers, TStructMembers>));

            var sourceFixture = Fixture.Create<TClassMembers>();
            var source = typeToStructFunc(sourceFixture);

            var method = CompileMethod(sourceType, sourceMembers, destinationType, destinationMembers);

            Func<TStructMembers, TClassMembers> func =
                (Func<TStructMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TStructMembers, TClassMembers>));

            var destination = func(source);

            Assert.True(CompareEquals(source, destination));
        }

        [Fact]
        public void EmitSetValueFromIntToLong()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.Int64Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.Equal(source.Int32Type, destination.Int64Type);
        }

        [Fact]
        public void EmitSetValueFromIntToBool()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.BooleanType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.Equal(source.Int32Type > 0, destination.BooleanType);
        }

        [Fact]
        public void EmitSetValueFromBoolToInt()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.BooleanType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.Int32Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.Equal(source.BooleanType, destination.Int32Type > 0);
        }

        [Fact]
        public void EmitSetValueFromDecimalToBool()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.DecimalType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.BooleanType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.Equal(source.DecimalType > 0, destination.BooleanType);
        }

        [Fact]
        public void EmitSetValueFromBoolToDecimal()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);
            
            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.BooleanType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.DecimalType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.Equal(source.BooleanType, destination.DecimalType > 0);
        }

        [Fact]
        public void EmitSetValueFromIntToDouble()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.DoubleType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.Equal(Convert.ToDouble(source.Int32Type), destination.DoubleType);
        }

        [Fact]
        public void EmitSetValueFromDoubleToInt()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.DoubleType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.Int32Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.Equal(Convert.ToInt32(source.DoubleType), destination.Int32Type);
        }

        [Fact]
        public void EmitSetValueFromNullableIntToDouble()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TNullableMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.DoubleType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TNullableMembers, TClassMembers> func =
                (Func<TNullableMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TClassMembers>));

            var source = Fixture.Create<TNullableMembers>();
            var destination = func(source);

            Assert.Equal(Convert.ToDouble(source.Int32Type), destination.DoubleType);
        }

        [Fact]
        public void EmitSetValueFromNullableDoubleToInt()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TNullableMembers.DoubleType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.Int32Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TNullableMembers, TClassMembers> func =
                (Func<TNullableMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TClassMembers>));

            var source = Fixture.Create<TNullableMembers>();
            var destination = func(source);

            Assert.Equal(Convert.ToInt32(source.DoubleType), destination.Int32Type);
        }

        [Fact]
        public void EmitSetValueFromNullableDoubleToNullableInt()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TNullableMembers.DoubleType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TNullableMembers.Int32Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TNullableMembers, TNullableMembers> func =
                (Func<TNullableMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TNullableMembers>));

            var source = Fixture.Create<TNullableMembers>();
            var destination = func(source);

            Assert.Equal(Convert.ToInt32(source.DoubleType), destination.Int32Type);
        }

        [Fact]
        public void EmitSetValueFromNullableIntToNullableLong()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TNullableMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TNullableMembers.Int64Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TNullableMembers, TNullableMembers> func =
                (Func<TNullableMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TNullableMembers>));

            var source = Fixture.Create<TNullableMembers>();
            var destination = func(source);

            Assert.Equal(source.Int32Type, destination.Int64Type);
        }

        [Fact]
        public void EmitSetValueFromLongToNullableInt()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.Int64Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TNullableMembers.Int32Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TNullableMembers> func =
                (Func<TClassMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TNullableMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.Equal(source.Int64Type, Convert.ToInt64(destination.Int32Type));
        }

        [Fact]
        public void EmitSetValueFromNullableIntToLong()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TNullableMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.Int64Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TNullableMembers, TClassMembers> func =
                (Func<TNullableMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TClassMembers>));

            var source = Fixture.Create<TNullableMembers>();
            var destination = func(source);

            Assert.Equal(Convert.ToInt32(source.Int32Type), destination.Int64Type);
        }

        [Fact]
        public void EmitSetValueFromNullableIntToDecimal()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TNullableMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.DecimalType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TNullableMembers, TClassMembers> func =
                (Func<TNullableMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TClassMembers>));

            var source = Fixture.Create<TNullableMembers>();
            var destination = func(source);

            Assert.Equal(Convert.ToInt32(source.Int32Type), Convert.ToInt32(destination.DecimalType));
        }

        [Fact]
        public void EmitSetValueFromNullableDecimalToInt()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TNullableMembers.DecimalType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.Int32Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TNullableMembers, TClassMembers> func =
                (Func<TNullableMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TClassMembers>));

            var source = Fixture.Create<TNullableMembers>();
            var destination = func(source);

            Assert.Equal(Convert.ToInt32(source.DecimalType), destination.Int32Type);
        }

        [Fact]
        public void EmitSetValueFromIntToString()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.StringType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.Equal(source.Int32Type, Convert.ToInt32(destination.StringType));
        }

        [Fact]
        public void EmitSetValueFromIntToObject()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.ObjectType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            var destination = func(source);

            Assert.Equal(source.Int32Type, Convert.ToInt32(destination.ObjectType));
        }

        [Fact]
        public void EmitSetValueFromObjectToInt()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.ObjectType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.Int32Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            source.ObjectType = 7;
            var destination = func(source);

            Assert.Equal(Convert.ToInt32(source.ObjectType), destination.Int32Type);
        }

        [Fact]
        public void EmitSetValueFromObjectToString()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.ObjectType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.StringType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            source.ObjectType = 7;
            var destination = func(source);

            Assert.Equal(source.ObjectType.ToString(), destination.StringType);
        }

        [Fact]
        public void EmitSetValueFromNullableIntToString()
        {
            var sourceType = typeof(TNullableMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TNullableMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.StringType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TNullableMembers, TClassMembers> func =
                (Func<TNullableMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TNullableMembers, TClassMembers>));

            var source = Fixture.Create<TNullableMembers>();
            var destination = func(source);

            Assert.Equal(source.Int32Type, Convert.ToInt32(destination.StringType));
        }

        [Fact]
        public void EmitSetValueFromStringToInt()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TClassMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.StringType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.Int32Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            Random random = new Random();
            source.StringType = random.Next(1024, 10240).ToString();

            var destination = func(source);

            Assert.Equal(Convert.ToInt32(source.StringType), destination.Int32Type);
        }

        [Fact]
        public void EmitSetValueFromStringToNullableInt()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.StringType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TNullableMembers.Int32Type));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TNullableMembers> func =
                (Func<TClassMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TNullableMembers>));

            var source = Fixture.Create<TClassMembers>();
            Random random = new Random();
            source.StringType = random.Next(1024, 10240).ToString();

            var destination = func(source);

            Assert.Equal(Convert.ToInt32(source.StringType), destination.Int32Type);
        }

        [Fact]
        public void EmitSetValueFromStringToEnum()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = sourceType;

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.StringType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.EnumType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            source.StringType = EnumType.B.ToString();

            var destination = func(source);

            Assert.Equal(source.StringType, destination.EnumType.ToString());
        }

        [Fact]
        public void EmitSetValueFromStringToNullableEnum()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = typeof(TNullableMembers);

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.StringType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TNullableMembers.EnumType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TNullableMembers> func =
                (Func<TClassMembers, TNullableMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TNullableMembers>));

            var source = Fixture.Create<TClassMembers>();
            source.StringType = EnumType.B.ToString();

            var destination = func(source);

            Assert.Equal(source.StringType, destination.EnumType.ToString());
        }

        [Fact]
        public void EmitSetValueFromDecimalToEnum()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = sourceType;

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.DecimalType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.EnumType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            source.DecimalType = Convert.ToDecimal((int)EnumType.B);

            var destination = func(source);

            Assert.Equal(Convert.ToInt32(source.DecimalType), (int)destination.EnumType);
        }

        [Fact]
        public void EmitSetValueFromIntToEnum()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = sourceType;

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.Int32Type));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.EnumType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            source.Int32Type = (int)EnumType.B;

            var destination = func(source);

            Assert.Equal(source.Int32Type, (int)destination.EnumType);
        }

        [Fact]
        public void EmitSetValueFromEnumToDecimal()
        {
            var sourceType = typeof(TClassMembers);
            var destinationType = sourceType;

            var sourceMember = TypeInfo.GetMembers(sourceType).First(w => w.Name == nameof(TClassMembers.EnumType));
            var destinationMember = TypeInfo.GetMembers(destinationType).First(w => w.Name == nameof(TClassMembers.DecimalType));

            var method = CompileMethod(sourceType, new MemberInfo[] { sourceMember }, destinationType, new MemberInfo[] { destinationMember });

            Func<TClassMembers, TClassMembers> func =
                (Func<TClassMembers, TClassMembers>)method.CreateDelegate(typeof(Func<TClassMembers, TClassMembers>));

            var source = Fixture.Create<TClassMembers>();
            source.EnumType = EnumType.B;

            var destination = func(source);

            Assert.Equal(Convert.ToDecimal(source.EnumType), destination.DecimalType);
        }
    }
}
