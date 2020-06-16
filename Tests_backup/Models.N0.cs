using System;

namespace Models
{
    #region Enums
    public enum TUndefinedABCEnum
    {
        Undefined,
        A,
        B,
        C
    }

    public enum TABCEnum
    {
        A,
        B,
        C
    }
    #endregion
    #region C0 - Class Members
    public class TC0_I0_Members : IComparable<TC0_I0_Members>
    {
        public bool BooleanMember { get; set; }
        public char CharMember { get; set; }
        public sbyte SByteMember { get; set; }
        public byte ByteMember { get; set; }
        public short Int16SMember { get; set; }
        public ushort UInt16Member { get; set; }
        public int Int32Member { get; set; }
        public uint UInt32Member { get; set; }
        public long Int64Member { get; set; }
        public ulong UInt64Member { get; set; }
        public float SingleMember { get; set; }
        public double DoubleMember { get; set; }
        public decimal DecimalMember { get; set; }
        public string StringMember { get; set; }

        public DateTime DateTimeMember { get; set; }
        public DateTimeOffset DateTimeOffsetMember { get; set; }
        public TimeSpan TimeSpanMember { get; set; }

        public Guid GuidMember { get; set; }

        public TUndefinedABCEnum UndefinedEnumMember { get; set; }
        public TABCEnum EnumMember { get; set; }

        public int CompareTo(TC0_I0_Members other)
        {
            return Int32Member.CompareTo(other.Int32Member);
        }
    }
    public class TC0_I1_Nullable_Members : IComparable<TC0_I1_Nullable_Members>
    {
        public bool? BooleanMember { get; set; }
        public char? CharMember { get; set; }
        public sbyte? SByteMember { get; set; }
        public byte? ByteMember { get; set; }
        public short? Int16SMember { get; set; }
        public ushort? UInt16Member { get; set; }
        public int? Int32Member { get; set; }
        public uint? UInt32Member { get; set; }
        public long? Int64Member { get; set; }
        public ulong? UInt64Member { get; set; }
        public float? SingleMember { get; set; }
        public double? DoubleMember { get; set; }
        public decimal? DecimalMember { get; set; }
        public string StringMember { get; set; }

        public DateTime? DateTimeMember { get; set; }
        public DateTimeOffset? DateTimeOffsetMember { get; set; }
        public TimeSpan? TimeSpanMember { get; set; }

        public Guid? GuidMember { get; set; }

        public TUndefinedABCEnum? UndefinedEnumMember { get; set; }
        public TABCEnum? EnumMember { get; set; }

        public int CompareTo(TC0_I1_Nullable_Members other)
        {
            return (Int32Member != null ? Int32Member.Value : 0)
                .CompareTo(other.Int32Member != null ? other.Int32Member.Value : 0);
        }
    }
    public class TC0_I2_Literal_Members : IComparable<TC0_I2_Literal_Members>
    {
        public const bool BooleanMember = true;
        public const char CharMember = '0';
        public const sbyte SByteMember = 1;
        public const byte ByteMember = 2;
        public const short Int16SMember = 3;
        public const ushort UInt16Member = 4;
        public const int Int32Member = 5;
        public const uint UInt32Member = 6;
        public const long Int64Member = 7;
        public const ulong UInt64Member = 8;
        public const float SingleMember = 9;
        public const double DoubleMember = 10;
        public const decimal DecimalMember = 11;
        public const string StringMember = "12";

        public const TUndefinedABCEnum UndefinedEnumMember = TUndefinedABCEnum.A;
        public const TABCEnum EnumMember = TABCEnum.B;

        public int CompareTo(TC0_I2_Literal_Members other)
        {
            return 0;
        }
    }
    public class TC0_I3_Readonly_Members : IComparable<TC0_I3_Readonly_Members>
    {
        public bool BooleanMember { get; } = true;
        public char CharMember { get; } = '1';
        public sbyte SByteMember { get; } = 2;
        public byte ByteMember { get; } = 3;
        public short Int16SMember { get; } = 4;
        public ushort UInt16Member { get; } = 5;
        public int Int32Member { get; } = 6;
        public uint UInt32Member { get; } = 7;
        public long Int64Member { get; } = 8;
        public ulong UInt64Member { get; } = 9;
        public float SingleMember { get; } = 10;
        public double DoubleMember { get; } = 11;
        public decimal DecimalMember { get; } = 12;
        public string StringMember { get; } = "13";

        public DateTime DateTimeMember { get; } = new DateTime(2019, 11, 30);
        public DateTimeOffset DateTimeOffsetMember { get; } = new DateTimeOffset(new DateTime(2019, 11, 30), new TimeSpan(12, 00, 00));
        public TimeSpan TimeSpanMember { get; } = new TimeSpan(12, 00, 00);

        public Guid GuidMember { get; } = Guid.Parse("C5E39EA0-0031-41F9-9B4A-515D2B216267");

        public TUndefinedABCEnum UndefinedEnumMember { get; } = TUndefinedABCEnum.A;
        public TABCEnum EnumMember { get; } = TABCEnum.A;

        public int CompareTo(TC0_I3_Readonly_Members other)
        {
            return 0;
        }
    }
    public class TC0_I4_Static_Members : IComparable<TC0_I4_Static_Members>
    {
        public static bool BooleanMember { get; set; }
        public static char CharMember { get; set; }
        public static sbyte SByteMember { get; set; }
        public static byte ByteMember { get; set; }
        public static short Int16SMember { get; set; }
        public static ushort UInt16Member { get; set; }
        public static int Int32Member { get; set; }
        public static uint UInt32Member { get; set; }
        public static long Int64Member { get; set; }
        public static ulong UInt64Member { get; set; }
        public static float SingleMember { get; set; }
        public static double DoubleMember { get; set; }
        public static decimal DecimalMember { get; set; }
        public static string StringMember { get; set; }

        public static DateTime DateTimeMember { get; set; }
        public static DateTimeOffset DateTimeOffsetMember { get; set; }
        public static TimeSpan TimeSpanMember { get; set; }

        public static Guid GuidMember { get; set; }

        public static TUndefinedABCEnum UndefinedEnumMember { get; set; }
        public static TABCEnum EnumMember { get; set; }

        public int CompareTo(TC0_I4_Static_Members other)
        {
            return 0;
        }
    }
    public class TC0_I5_StaticNullable_Members : IComparable<TC0_I5_StaticNullable_Members>
    {
        public static bool? BooleanMember { get; set; }
        public static char? CharMember { get; set; }
        public static sbyte? SByteMember { get; set; }
        public static byte? ByteMember { get; set; }
        public static short? Int16SMember { get; set; }
        public static ushort? UInt16Member { get; set; }
        public static int? Int32Member { get; set; }
        public static uint? UInt32Member { get; set; }
        public static long? Int64Member { get; set; }
        public static ulong? UInt64Member { get; set; }
        public static float? SingleMember { get; set; }
        public static double? DoubleMember { get; set; }
        public static decimal? DecimalMember { get; set; }
        public static string StringMember { get; set; }

        public static DateTime? DateTimeMember { get; set; }
        public static DateTimeOffset? DateTimeOffsetMember { get; set; }
        public static TimeSpan? TimeSpanMember { get; set; }

        public static Guid? GuidMember { get; set; }

        public static TUndefinedABCEnum? UndefinedEnumMember { get; set; }
        public static TABCEnum? EnumMember { get; set; }

        public int CompareTo(TC0_I5_StaticNullable_Members other)
        {
            return 0;
        }
    }
    #endregion
    #region S0 - Struct Members
    public struct TS0_I0_Members : IComparable<TS0_I0_Members>
    {
        public bool BooleanMember { get; set; }
        public char CharMember { get; set; }
        public sbyte SByteMember { get; set; }
        public byte ByteMember { get; set; }
        public short Int16SMember { get; set; }
        public ushort UInt16Member { get; set; }
        public int Int32Member { get; set; }
        public uint UInt32Member { get; set; }
        public long Int64Member { get; set; }
        public ulong UInt64Member { get; set; }
        public float SingleMember { get; set; }
        public double DoubleMember { get; set; }
        public decimal DecimalMember { get; set; }
        public string StringMember { get; set; }

        public DateTime DateTimeMember { get; set; }
        public DateTimeOffset DateTimeOffsetMember { get; set; }
        public TimeSpan TimeSpanMember { get; set; }

        public Guid GuidMember { get; set; }

        public TUndefinedABCEnum UndefinedEnumMember { get; set; }
        public TABCEnum EnumMember { get; set; }

        public int CompareTo(TS0_I0_Members other)
        {
            return Int32Member.CompareTo(other.Int32Member);
        }
    }
    public struct TS0_I1_Nullable_Members : IComparable<TS0_I1_Nullable_Members>
    {
        public bool? BooleanMember { get; set; }
        public char? CharMember { get; set; }
        public sbyte? SByteMember { get; set; }
        public byte? ByteMember { get; set; }
        public short? Int16SMember { get; set; }
        public ushort? UInt16Member { get; set; }
        public int? Int32Member { get; set; }
        public uint? UInt32Member { get; set; }
        public long? Int64Member { get; set; }
        public ulong? UInt64Member { get; set; }
        public float? SingleMember { get; set; }
        public double? DoubleMember { get; set; }
        public decimal? DecimalMember { get; set; }
        public string StringMember { get; set; }

        public DateTime? DateTimeMember { get; set; }
        public DateTimeOffset? DateTimeOffsetMember { get; set; }
        public TimeSpan? TimeSpanMember { get; set; }

        public Guid? GuidMember { get; set; }

        public TUndefinedABCEnum? UndefinedEnumMember { get; set; }
        public TABCEnum? EnumMember { get; set; }

        public int CompareTo(TS0_I1_Nullable_Members other)
        {
            return (Int32Member != null ? Int32Member.Value : 0)
                .CompareTo(other.Int32Member != null ? other.Int32Member.Value : 0);
        }
    }
    public struct TS0_I2_Literal_Members : IComparable<TS0_I2_Literal_Members>
    {
        public const bool BooleanMember = true;
        public const char CharMember = '0';
        public const sbyte SByteMember = 1;
        public const byte ByteMember = 2;
        public const short Int16SMember = 3;
        public const ushort UInt16Member = 4;
        public const int Int32Member = 5;
        public const uint UInt32Member = 6;
        public const long Int64Member = 7;
        public const ulong UInt64Member = 8;
        public const float SingleMember = 9;
        public const double DoubleMember = 10;
        public const decimal DecimalMember = 11;
        public const string StringMember = "12";

        public const TUndefinedABCEnum UndefinedEnumMember = TUndefinedABCEnum.A;
        public const TABCEnum EnumMember = TABCEnum.B;

        public int CompareTo(TS0_I2_Literal_Members other)
        {
            return 0;
        }
    }
    public struct TS0_I3_Static_Members : IComparable<TS0_I3_Static_Members>
    {
        public static bool BooleanMember { get; set; }
        public static char CharMember { get; set; }
        public static sbyte SByteMember { get; set; }
        public static byte ByteMember { get; set; }
        public static short Int16SMember { get; set; }
        public static ushort UInt16Member { get; set; }
        public static int Int32Member { get; set; }
        public static uint UInt32Member { get; set; }
        public static long Int64Member { get; set; }
        public static ulong UInt64Member { get; set; }
        public static float SingleMember { get; set; }
        public static double DoubleMember { get; set; }
        public static decimal DecimalMember { get; set; }
        public static string StringMember { get; set; }

        public static DateTime DateTimeMember { get; set; }
        public static DateTimeOffset DateTimeOffsetMember { get; set; }
        public static TimeSpan TimeSpanMember { get; set; }

        public static Guid GuidMember { get; set; }

        public static TUndefinedABCEnum UndefinedEnumMember { get; set; }
        public static TABCEnum EnumMember { get; set; }

        public int CompareTo(TS0_I3_Static_Members other)
        {
            return 0;
        }
    }
    public struct TS0_I4_StaticNullable_Members : IComparable<TS0_I4_StaticNullable_Members>
    {
        public static bool? BooleanMember { get; set; }
        public static char? CharMember { get; set; }
        public static sbyte? SByteMember { get; set; }
        public static byte? ByteMember { get; set; }
        public static short? Int16SMember { get; set; }
        public static ushort? UInt16Member { get; set; }
        public static int? Int32Member { get; set; }
        public static uint? UInt32Member { get; set; }
        public static long? Int64Member { get; set; }
        public static ulong? UInt64Member { get; set; }
        public static float? SingleMember { get; set; }
        public static double? DoubleMember { get; set; }
        public static decimal? DecimalMember { get; set; }
        public static string StringMember { get; set; }

        public static DateTime? DateTimeMember { get; set; }
        public static DateTimeOffset? DateTimeOffsetMember { get; set; }
        public static TimeSpan? TimeSpanMember { get; set; }

        public static Guid? GuidMember { get; set; }

        public static TUndefinedABCEnum? UndefinedEnumMember { get; set; }
        public static TABCEnum? EnumMember { get; set; }

        public int CompareTo(TS0_I4_StaticNullable_Members other)
        {
            return 0;
        }
    }
    #endregion
}
