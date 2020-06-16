using Models;
using Xunit;
using Xunit.Abstractions;

namespace Internal
{
	public class FromTo_N0_Static_Members<S> : FromTo_N0<S> where S : new()
	{
		public FromTo_N0_Static_Members(ITestOutputHelper console) : base(console) { }

		#region To C0
		[Fact]
		public void To_C0_I4_Static_Members() => ToClass<TC0_I4_Static_Members>(false, true);
		[Fact]
		public void To_C0_I5_StaticNullable_Members() => ToClass<TC0_I5_StaticNullable_Members>(false, true);
		#endregion
		#region To S0
		[Fact]
		public void To_S0_I3_Static_Members() => ToStruct<TS0_I3_Static_Members>(false, true);
		[Fact]
		public void To_S0_I4_StaticNullable_Members() => ToStruct<TS0_I4_StaticNullable_Members>(false, true);
		#endregion
		#region To NS0
		[Fact]
		public void To_NS0_I3_Static_Members() => ToNullableStruct<TS0_I3_Static_Members>(false, true);
		[Fact]
		public void To_NS0_I4_StaticNullable_Members() => ToNullableStruct<TS0_I4_StaticNullable_Members>(false, true);
		#endregion
	}
}
