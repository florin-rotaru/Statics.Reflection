using Air.Reflection;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;
using static Test.Models;

namespace Test
{
    public class TypesInfo
    {
        Fixture Fixture { get; }

        readonly ITestOutputHelper Console;

        public TypesInfo(ITestOutputHelper console)
        {
            Fixture = new Fixture();
            Console = console;
        }

        public TStructMembers CreateStructSystemTypeCodes()
        {
            return new TStructMembers
            {
                BooleanType = true,
                ByteType = Fixture.Create<byte>(),
                CharType = Fixture.Create<char>(),
                DateTimeOffsetType = Fixture.Create<DateTimeOffset>(),
                DateTimeType = Fixture.Create<DateTime>(),
                DecimalType = Fixture.Create<decimal>(),
                DoubleType = Fixture.Create<double>(),
                DtoEnumType = DtoEnumType.B,
                EnumType = EnumType.C,
                GuidType = Guid.NewGuid(),
                Int16SType = Fixture.Create<short>(),
                Int32Type = Fixture.Create<int>(),
                Int64Type = Fixture.Create<long>(),
                SByteType = Fixture.Create<sbyte>(),
                SingleType = Fixture.Create<float>(),
                StringType = Fixture.Create<string>(),
                TimeSpanType = Fixture.Create<TimeSpan>(),
                UInt16Type = Fixture.Create<ushort>(),
                UInt32Type = Fixture.Create<uint>(),
                UInt64Type = Fixture.Create<ulong>()
            };
        }

        [Fact]
        public void Nodes()
        {
            var nodes = TypeInfo.GetNodes(typeof(TD1ClassNodeWithClassMembers), false);
            Assert.Contains(nodes, n => n.Name == $"{nameof(TD1ClassNodeWithClassMembers.Members)}");

            nodes = TypeInfo.GetNodes(typeof(TD1ClassNodeWithStructMembers), false);
            Assert.Contains(nodes, n => n.Name == $"{nameof(TD1ClassNodeWithStructMembers.Members)}");

            nodes = TypeInfo.GetNodes(typeof(TD1ClassNodeWithNullableMembers), false);
            Assert.Contains(nodes, n => n.Name == $"{nameof(TD1ClassNodeWithNullableMembers.Members)}");

            nodes = TypeInfo.GetNodes(typeof(TD1ClassNodeWithStaticClassMembers), false);
            Assert.Contains(nodes, n => n.Name == $"{nameof(TD1ClassNodeWithStaticClassMembers.Members)}");

            nodes = TypeInfo.GetNodes(typeof(TD1ClassNodeWithStaticStructMembers), false);
            Assert.Contains(nodes, n => n.Name == $"{nameof(TD1ClassNodeWithStaticStructMembers.Members)}");

            nodes = TypeInfo.GetNodes(typeof(TD1ClassNodeWithStaticNullableMembers), false);
            Assert.Contains(nodes, n => n.Name == $"{nameof(TD1ClassNodeWithStaticNullableMembers.Members)}");

            nodes = TypeInfo.GetNodes(typeof(TD1ClassNodeWithStaticNullableStructMembers), false);
            Assert.Contains(nodes, n => n.Name == $"{nameof(TD1ClassNodeWithStaticNullableStructMembers.Members)}");
        }

        [Fact]
        public void IsNumeric()
        {
            Assert.True(TypeInfo.IsNumeric(typeof(decimal)));
            Assert.True(TypeInfo.IsNumeric(typeof(byte)));
            Assert.False(TypeInfo.IsNumeric(typeof(bool)));
        }

        [Fact]
        public void IsEnumerable()
        {
            Assert.True(TypeInfo.IsEnumerable(typeof(List<int>)));
            Assert.True(TypeInfo.IsEnumerable(typeof(string)));
            Assert.False(TypeInfo.IsEnumerable(typeof(decimal)));
        }

        [Fact]
        public void IsStatic()
        {
            Assert.True(TypeInfo.IsStatic(typeof(TStatic)));
            Assert.False(TypeInfo.IsStatic(typeof(TClassMembers)));
        }

        [Fact]
        public void IsEnum()
        {
            Assert.True(TypeInfo.IsEnum(typeof(DtoEnumType)));
            Assert.False(TypeInfo.IsEnum(typeof(TClassMembers)));
        }

        [Fact]
        public void IsBuiltIn()
        {
            Assert.True(TypeInfo.IsBuiltIn(typeof(int)));
            Assert.True(TypeInfo.IsBuiltIn(typeof(int?)));
            Assert.True(TypeInfo.IsBuiltIn(typeof(string)));
            Assert.True(TypeInfo.IsBuiltIn(typeof(Guid)));
            Assert.True(TypeInfo.IsBuiltIn(typeof(Enum)));
            Assert.True(TypeInfo.IsBuiltIn(typeof(DtoEnumType)));
            Assert.False(TypeInfo.IsBuiltIn(typeof(TStructMembers)));
            Assert.False(TypeInfo.IsBuiltIn(typeof(TStructMembers?)));
            Assert.False(TypeInfo.IsBuiltIn(typeof(TClassMembers)));
        }

        [Fact]
        public void IsNonBuiltInStruct()
        {
            Assert.True(TypeInfo.IsNonBuiltInStruct(typeof(TStructMembers)));
            Assert.True(TypeInfo.IsNonBuiltInStruct(typeof(TStructMembers?)));
            Assert.False(TypeInfo.IsNonBuiltInStruct(typeof(TClassMembers)));
            Assert.False(TypeInfo.IsNonBuiltInStruct(typeof(int)));
            Assert.False(TypeInfo.IsNonBuiltInStruct(typeof(int?)));
            Assert.False(TypeInfo.IsNonBuiltInStruct(typeof(string)));
            Assert.False(TypeInfo.IsNonBuiltInStruct(typeof(Guid)));
            Assert.False(TypeInfo.IsNonBuiltInStruct(typeof(Enum)));
            Assert.False(TypeInfo.IsNonBuiltInStruct(typeof(DtoEnumType)));
        }

        [Fact]
        public void LiteralValue()
        {
            MemberInfo memberInfo = TypeInfo.GetMembers(typeof(int)).First(w => w.Name == nameof(int.MaxValue));

            Assert.Equal(int.MaxValue, memberInfo.DefaultValue);
        }

        [Fact]
        public void GetNameInstanceMember()
        {
            Expression<Func<Node, object>> expression = model => model.Segment.Members.BooleanType;

            var memberName = TypeInfo.GetName(expression, true);
            Assert.Equal(
                nameof(Node.Segment) + "." + nameof(Node.Segment.Members) + "." + nameof(Node.Segment.Members.BooleanType),
                memberName);

            memberName = TypeInfo.GetName(expression, false);
            Assert.Equal(nameof(Node.Segment.Members.BooleanType), memberName);
        }

        [Fact]
        public void GetNameStaticMember()
        {
            Expression<Func<StaticNode, object>> expression = model => StaticNode.Segment.Members.BooleanType;

            var memberName = TypeInfo.GetName(expression, true);
            Assert.Equal(
                nameof(StaticNode.Segment) + "." + nameof(StaticNode.Segment.Members) + "." + nameof(StaticNode.Segment.Members.BooleanType),
                memberName);

            memberName = TypeInfo.GetName(expression, false);
            Assert.Equal(nameof(Node.Segment.Members.BooleanType), memberName);
        }

        [Fact]
        public void Settable()
        {
            var settable = TypeInfo.GetSettableMembers(typeof(TClassMembers));
            Assert.True(settable.FirstOrDefault(w => w.Name == nameof(TClassMembers.BooleanType)) != null);

            settable = TypeInfo.GetSettableMembers(typeof(TReadonlyMembers));
            Assert.True(settable.FirstOrDefault(w => w.Name == nameof(TReadonlyMembers.BooleanType)) == null);
        }

        [Fact]
        public void MembersInfoDictionary()
        {
            var membersInfo = TypeInfo.MembersInfoDictionary(typeof(Node), true);

            Assert.True(membersInfo.ContainsKey(nameof(Node.Segment) + "." + nameof(Node.Segment.Members)));
            Assert.True(membersInfo.ContainsKey(nameof(Node.Segment) + "." + nameof(Node.Segment.Members) + "." + nameof(Node.Segment.Members.BooleanType)));

            membersInfo = TypeInfo.MembersInfoDictionary(typeof(TStructSegment), true);

            Assert.True(membersInfo.ContainsKey(nameof(TStructSegment.Members)));
            Assert.True(membersInfo.ContainsKey(nameof(TStructSegment.Members) + "." + nameof(TStructSegment.Members.BooleanType)));

            membersInfo = TypeInfo.MembersInfoDictionary(typeof(TStructSegment?), true);

            Assert.True(membersInfo.ContainsKey(nameof(TStructSegment.Members)));
            Assert.True(membersInfo.ContainsKey(nameof(TStructSegment.Members) + "." + nameof(TStructSegment.Members.BooleanType)));

            membersInfo = TypeInfo.MembersInfoDictionary(typeof(TStructSegment?), false);

            Assert.False(membersInfo.ContainsKey(nameof(TStructSegment.Members)));
            Assert.False(membersInfo.ContainsKey(nameof(TStructSegment.Members) + "." + nameof(TStructSegment.Members.BooleanType)));
        }

        [Fact]
        public void Names()
        {
            var names = TypeInfo.GetMembersNames<Node>(true);

            Assert.Contains(nameof(Node.Segment) + "." + nameof(Node.Segment.Members), names);
            Assert.Contains(nameof(Node.Segment) + "." + nameof(Node.Segment.Members) + "." + nameof(Node.Segment.Members.BooleanType), names);

            names = TypeInfo.GetMembersNames<TStructSegment>(true);

            Assert.Contains(nameof(TStructSegment.Members), names);
            Assert.Contains(nameof(TStructSegment.Members) + "." + nameof(TStructSegment.Members.BooleanType), names);

            names = TypeInfo.GetMembersNames<TStructSegment?>(true);

            Assert.Contains(nameof(TStructSegment.Members), names);
            Assert.Contains(nameof(TStructSegment.Members) + "." + nameof(TStructSegment.Members.BooleanType), names);

            names = TypeInfo.GetMembersNames<TStructSegment?>(false);

            Assert.DoesNotContain(nameof(TStructSegment.Members), names);
            Assert.DoesNotContain(nameof(TStructSegment.Members) + "." + nameof(TStructSegment.Members.BooleanType), names);
        }

        [Fact]
        public void ValuesDictionary()
        {
            var model = Fixture.Create<Node>();

            var toDictionary = TypeInfo.ValuesDictionary(model);

            toDictionary.TryGetValue(nameof(Node.Segment) + "." + nameof(Node.Segment.Members) + "." + nameof(Node.Segment.Members.StringType), out object member);
            Assert.True(member.Equals(model.Segment.Members.StringType));


            var structSegment = new TStructSegment
            {
                Members = new TStructMembers
                {
                    StringType = model.Segment.Members.StringType
                }
            };

            toDictionary = TypeInfo.ValuesDictionary(structSegment);
            toDictionary.TryGetValue(nameof(TStructSegment.Members) + "." + nameof(TStructSegment.Members.StringType), out member);
            Assert.True(member.Equals(model.Segment.Members.StringType));


            var nullableStructSegment = new TStructSegment?();
            nullableStructSegment = structSegment;

            toDictionary = TypeInfo.ValuesDictionary(nullableStructSegment);

            toDictionary.TryGetValue(nameof(TStructSegment.Members) + "." + nameof(TStructSegment.Members.StringType), out member);
            Assert.True(member.Equals(model.Segment.Members.StringType));
        }

        [Fact]
        public void GetNodes()
        {
            var nodes = TypeInfo.GetNodes<Node>(true);

            Assert.True(nodes.Exists(w => w.Name == nameof(Node.Segment)));
            Assert.True(nodes.Exists(w => w.Name == nameof(Node.Segment) + "." + nameof(Node.Segment.Members)));

            nodes = TypeInfo.GetNodes<TStructSegment>(true);

            Assert.True(nodes.Exists(w => w.Name == nameof(TStructSegment.Members)));

            nodes = TypeInfo.GetNodes<TStructSegment?>(true);

            Assert.True(nodes.Exists(w => w.Name == nameof(TStructSegment.Members)));

            nodes = TypeInfo.GetNodes<TStructSegment?>(false);

            Assert.False(nodes.Exists(w => w.Name == nameof(TStructSegment.Members)));

            nodes = TypeInfo.GetNodes<TLiteralNode>(true);

            Assert.True(nodes.Exists(w => w.Name == nameof(TLiteralNode.Segment)));
        }

        [Fact]
        public void GetNodeName()
        {
            var member = nameof(Node.Segment) + "." + nameof(Node.Segment.Members) + "." + nameof(Node.Segment.Members.StringType);

            Assert.Equal(nameof(Node.Segment) + "." + nameof(Node.Segment.Members), TypeInfo.GetNodeName(member));
        }

        [Fact]
        public void Recursion()
        {
            var recursion_0 = TypeInfo.GetNodes(typeof(RecursiveNode), true);

            Assert.NotEmpty(recursion_0);

            var recursion_1 = TypeInfo.GetNodes(typeof(RecursiveNode), true, 1);

            Assert.True(recursion_1.Count > recursion_0.Count);
        }
    }
}
