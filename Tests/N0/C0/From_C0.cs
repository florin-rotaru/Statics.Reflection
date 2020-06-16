using Internal;
using Models;
using Xunit;
using Xunit.Abstractions;

namespace C0
{
	public class From_NS_C0_I0_Members : FromTo_N0_NonStatic_Members<TC0_I0_Members> { public From_NS_C0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C0")]
	public class From_S_C0_I0_Members : FromTo_N0_Static_Members<TC0_I0_Members>{ public From_S_C0_I0_Members(ITestOutputHelper console) : base(console) {} }

	public class From_NS_C0_I1_Nullable_Members : FromTo_N0_NonStatic_Members<TC0_I1_Nullable_Members> { public From_NS_C0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C0")]
	public class From_S_C0_I1_Nullable_Members : FromTo_N0_Static_Members<TC0_I1_Nullable_Members>{ public From_S_C0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	public class From_NS_C0_I2_Literal_Members : FromTo_N0_NonStatic_Members<TC0_I2_Literal_Members> { public From_NS_C0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C0")]
	public class From_S_C0_I2_Literal_Members : FromTo_N0_Static_Members<TC0_I2_Literal_Members>{ public From_S_C0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	public class From_NS_C0_I3_Readonly_Members : FromTo_N0_NonStatic_Members<TC0_I3_Readonly_Members> { public From_NS_C0_I3_Readonly_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C0")]
	public class From_S_C0_I3_Readonly_Members : FromTo_N0_Static_Members<TC0_I3_Readonly_Members>{ public From_S_C0_I3_Readonly_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C0")]
	public class From_C0_I4_Static_Members : FromTo_N0_Members<TC0_I4_Static_Members> { public From_C0_I4_Static_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_C0")]
	public class From_C0_I5_StaticNullable_Members : FromTo_N0_Members<TC0_I5_StaticNullable_Members> { public From_C0_I5_StaticNullable_Members(ITestOutputHelper console) : base(console) {} }

}
