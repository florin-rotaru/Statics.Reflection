using Internal;
using Models;
using Xunit;
using Xunit.Abstractions;

namespace NS0
{
	public class From_NS_NS0_I0_Members : FromTo_N0_NonStatic_Members<TS0_I0_Members?> { public From_NS_NS0_I0_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S0")]
	public class From_S_NS0_I0_Members : FromTo_N0_Static_Members<TS0_I0_Members?>{ public From_S_NS0_I0_Members(ITestOutputHelper console) : base(console) {} }

	public class From_NS_NS0_I1_Nullable_Members : FromTo_N0_NonStatic_Members<TS0_I1_Nullable_Members?> { public From_NS_NS0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S0")]
	public class From_S_NS0_I1_Nullable_Members : FromTo_N0_Static_Members<TS0_I1_Nullable_Members?>{ public From_S_NS0_I1_Nullable_Members(ITestOutputHelper console) : base(console) {} }

	public class From_NS_NS0_I2_Literal_Members : FromTo_N0_NonStatic_Members<TS0_I2_Literal_Members?> { public From_NS_NS0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S0")]
	public class From_S_NS0_I2_Literal_Members : FromTo_N0_Static_Members<TS0_I2_Literal_Members?>{ public From_S_NS0_I2_Literal_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S0")]
	public class From_NS0_I3_Static_Members : FromTo_N0_Members<TS0_I3_Static_Members?> { public From_NS0_I3_Static_Members(ITestOutputHelper console) : base(console) {} }

	[Collection("S_S0")]
	public class From_NS0_I4_StaticNullable_Members : FromTo_N0_Members<TS0_I4_StaticNullable_Members?> { public From_NS0_I4_StaticNullable_Members(ITestOutputHelper console) : base(console) {} }

}
