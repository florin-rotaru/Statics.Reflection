using Air.Reflection;
using AutoFixture;
using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Playground
{
    public class Types
    {
        private readonly ITestOutputHelper Console;

        private Fixture Fixture { get; }

        public Types(ITestOutputHelper console)
        {
            Console = console;
            Fixture = new Fixture();
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var customization = new SupportMutableValueTypesCustomization();
            customization.Customize(Fixture);
        }

        [Fact]
        public void LiteralValue()
        {
            MemberInfo memberInfo = TypeInfo.GetMembers(typeof(int)).First(w => w.Name == nameof(int.MaxValue));

            Assert.Equal(int.MaxValue, memberInfo.DefaultValue);
        }

        [Fact]
        public void Name()
        {
            var member = nameof(TNode.ParentNode) + "." + nameof(TNode.ParentNode.ParentNode) + "." + nameof(TNode.ParentNode.ParentNode.Name);

            Assert.Equal(nameof(TNode.ParentNode) + "." + nameof(TNode.ParentNode.ParentNode), TypeInfo.GetNodeName(member));
        }

        [Fact]
        public void Recursion()
        {
            var recursion_0 = TypeInfo.GetNodes(typeof(TNode), true).ToList();

            Assert.NotEmpty(recursion_0);

            var recursion_1 = TypeInfo.GetNodes(typeof(TNode), true, 1).ToList();

            Assert.True(recursion_1.Count > recursion_0.Count);
        }

        [Fact]
        public void GetName()
        {
            Expression<Func<TNode, object>> expression = s => s.ParentNode.Name;

            var name = TypeInfo.GetName(expression, true);
            Assert.Equal(string.Join(".", nameof(TNode.ParentNode), nameof(TNode.ParentNode.Name)), name);

            expression = s => nameof(TNode.Name);

            name = TypeInfo.GetName(expression, true);
            Assert.Equal(string.Join(".", nameof(TNode.ParentNode.Name)), name);
        }

        [Fact]
        public void GetNames()
        {
            Expression<Func<TNode, object>> expression = s => new { s.ParentNode.Name, s.ParentNode.ChildNodes };

            var names = TypeInfo.GetNames(expression, true).ToList();
            Assert.Equal(string.Join(".", nameof(TNode.ParentNode), nameof(TNode.ParentNode.Name)), names[0]);
            Assert.Equal(string.Join(".", nameof(TNode.ParentNode), nameof(TNode.ParentNode.ChildNodes)), names[1]);

            expression = s => new { Name = true, ChildNodes = true };
            names = TypeInfo.GetNames(expression, true).ToList();
            Assert.Equal(string.Join(".", nameof(TNode.ParentNode.Name)), names[0]);
            Assert.Equal(string.Join(".", nameof(TNode.ParentNode.ChildNodes)), names[1]);
        }
    }
}
