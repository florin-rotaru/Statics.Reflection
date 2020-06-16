using Air.Reflection;
using AutoFixture;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Test.Models;

namespace Test
{
    public class MembersInfo
    {
        Fixture Fixture { get; }

        readonly ITestOutputHelper Console;

        public MembersInfo(ITestOutputHelper console)
        {
            Fixture = new Fixture();
            Console = console;
        }

        [Fact]
        public void GetValueFromClass()
        {
            var members = TypeInfo.GetMembers(typeof(TClassMembers));
            var instance = Fixture.Create<TClassMembers>();

            var member = members.FirstOrDefault(m => m.Name == nameof(TClassMembers.StringType));
            object memberValue = member.GetValue(instance);
            Assert.Equal((string)memberValue, instance.StringType);

            member = members.FirstOrDefault(m => m.Name == nameof(TClassMembers.Int32Type));
            memberValue = member.GetValue(instance);
            Assert.Equal((int)memberValue, instance.Int32Type);
        }

        [Fact]
        public void GetValueFromFields()
        {
            var members = TypeInfo.GetMembers(typeof(TFieldsMembers));
            var instance = Fixture.Create<TFieldsMembers>();

            var member = members.FirstOrDefault(m => m.Name == nameof(TFieldsMembers.StringType));
            object memberValue = member.GetValue(instance);
            Assert.Equal((string)memberValue, instance.StringType);

            member = members.FirstOrDefault(m => m.Name == nameof(TFieldsMembers.Int32Type));
            memberValue = member.GetValue(instance);
            Assert.Equal((int)memberValue, instance.Int32Type);
        }

        [Fact]
        public void GetValueFromStruct()
        {
            var members = TypeInfo.GetMembers(typeof(TStructMembers));
            var instance = new TStructMembers { StringType = Fixture.Create<string>() };

            var member = members.FirstOrDefault(m => m.Name == nameof(TStructMembers.StringType));
            object memberValue = member.GetValue(instance);
            Assert.Equal((string)memberValue, instance.StringType);

            member = members.FirstOrDefault(m => m.Name == nameof(TStructMembers.Int32Type));
            memberValue = member.GetValue(instance);
            Assert.Equal((int)memberValue, instance.Int32Type);
        }

        [Fact]
        public void GetValueFromNullableStruct()
        {
            var members = TypeInfo.GetMembers(typeof(TStructMembers?));
            var instance = new TStructMembers?(new TStructMembers { StringType = Fixture.Create<string>() });

            var member = members.FirstOrDefault(m => m.Name == "Value");
            object memberValue = member.GetValue(instance);
            Assert.Equal(((TStructMembers)memberValue).StringType, instance.Value.StringType);
        }

        [Fact]
        public void GetValueFromStatic()
        {
            var members = TypeInfo.GetMembers(typeof(TStaticMembers));
            TStaticMembers.StringType = Fixture.Create<string>();

            var member = members.FirstOrDefault(m => m.Name == nameof(TStaticMembers.StringType));
            object memberValue = member.GetValue(null);
            Assert.Equal((string)memberValue, TStaticMembers.StringType);

            member = members.FirstOrDefault(m => m.Name == nameof(TStaticMembers.Int32Type));
            memberValue = member.GetValue(null);
            Assert.Equal((int)memberValue, TStaticMembers.Int32Type);
        }

        [Fact]
        public void GetValueFromInterface()
        {
            var members = TypeInfo.GetMembers(typeof(ISystemTypeCodes));
            var instance = Fixture.Create<TClassMembers>();

            var member = members.FirstOrDefault(m => m.Name == nameof(ISystemTypeCodes.StringType));
            object memberValue = member.GetValue(instance);
            Assert.Equal((string)memberValue, instance.StringType);

            member = members.FirstOrDefault(m => m.Name == nameof(ISystemTypeCodes.Int32Type));
            memberValue = member.GetValue(instance);
            Assert.Equal((int)memberValue, instance.Int32Type);
        }

        [Fact]
        public void GetValueFromAbstract()
        {
            var members = TypeInfo.GetMembers(typeof(TAbstractMembers));
            var instance = Fixture.Create<TExtendAbstractClass>();

            var member = members.FirstOrDefault(m => m.Name == nameof(TExtendAbstractClass.StringType));
            object memberValue = member.GetValue(instance);
            Assert.Equal((string)memberValue, instance.StringType);

            member = members.FirstOrDefault(m => m.Name == nameof(TExtendAbstractClass.Int32Type));
            memberValue = member.GetValue(instance);
            Assert.Equal((int)memberValue, instance.Int32Type);
        }

        [Fact]
        public void GetValueFromLiteral()
        {
            var members = TypeInfo.GetMembers(typeof(TLiteralMembers));
            var instance = Fixture.Create<TLiteralMembers>();

            var member = members.FirstOrDefault(m => m.Name == nameof(TLiteralMembers.StringType));
            object memberValue = member.GetValue(instance);
            Assert.Equal((string)memberValue, TLiteralMembers.StringType);

            member = members.FirstOrDefault(m => m.Name == nameof(TLiteralMembers.Int32Type));
            memberValue = member.GetValue(instance);
            Assert.Equal((int)memberValue, TLiteralMembers.Int32Type);
        }
    }
}
