using System;
using System.Collections.Generic;

namespace Test
{
    public class Models
    {
        public static class TStatic
        {
            public static string Name { get; set; }
        }

        public class RecursiveNode
        {
            public string Name { get; set; }

            public RecursiveNode ParentNode { get; set; }
            public List<RecursiveNode> ChildNodes { get; set; }

        }

        public enum EnumType
        {
            Undefined,
            A,
            B,
            C
        }

        public enum DtoEnumType
        {
            A,
            B,
            C
        }

        public class Node
        {
            public string Name { get; set; }
            public Segment Segment { get; set; }
        }

        public class StaticNode
        {
            public static string Name { get; set; }
            public static Segment Segment { get; set; }
        }

        public class Segment
        {
            public string Name { get; set; }
            public TClassMembers Members { get; set; }
            public TNullableMembers NullableMembers { get; set; }
        }

        public abstract class TAbstractMembers
        {
            public abstract bool BooleanType { get; set; }
            public abstract char CharType { get; set; }
            public abstract sbyte SByteType { get; set; }
            public abstract byte ByteType { get; set; }
            public abstract short Int16SType { get; set; }
            public abstract ushort UInt16Type { get; set; }
            public abstract int Int32Type { get; set; }
            public abstract uint UInt32Type { get; set; }
            public abstract long Int64Type { get; set; }
            public abstract ulong UInt64Type { get; set; }
            public abstract float SingleType { get; set; }
            public abstract double DoubleType { get; set; }
            public abstract decimal DecimalType { get; set; }
            public abstract string StringType { get; set; }

            public abstract DateTime DateTimeType { get; set; }
            public abstract DateTimeOffset DateTimeOffsetType { get; set; }
            public abstract TimeSpan TimeSpanType { get; set; }

            public abstract Guid GuidType { get; set; }

            public abstract EnumType EnumType { get; set; }
            public abstract DtoEnumType DtoEnumType { get; set; }

            public abstract object ObjectType { get; set; }
        }

        public class TExtendAbstractClass : TAbstractMembers
        {
            public override bool BooleanType { get; set; }
            public override char CharType { get; set; }
            public override sbyte SByteType { get; set; }
            public override byte ByteType { get; set; }
            public override short Int16SType { get; set; }
            public override ushort UInt16Type { get; set; }
            public override int Int32Type { get; set; }
            public override uint UInt32Type { get; set; }
            public override long Int64Type { get; set; }
            public override ulong UInt64Type { get; set; }
            public override float SingleType { get; set; }
            public override double DoubleType { get; set; }
            public override decimal DecimalType { get; set; }
            public override string StringType { get; set; }

            public override DateTime DateTimeType { get; set; }
            public override DateTimeOffset DateTimeOffsetType { get; set; }
            public override TimeSpan TimeSpanType { get; set; }

            public override Guid GuidType { get; set; }

            public override EnumType EnumType { get; set; }
            public override DtoEnumType DtoEnumType { get; set; }

            public override object ObjectType { get; set; }
        }

        public interface ISystemTypeCodes
        {
            bool BooleanType { get; set; }
            char CharType { get; set; }
            sbyte SByteType { get; set; }
            byte ByteType { get; set; }
            short Int16SType { get; set; }
            ushort UInt16Type { get; set; }
            int Int32Type { get; set; }
            uint UInt32Type { get; set; }
            long Int64Type { get; set; }
            ulong UInt64Type { get; set; }
            float SingleType { get; set; }
            double DoubleType { get; set; }
            decimal DecimalType { get; set; }
            string StringType { get; set; }

            DateTime DateTimeType { get; set; }
            DateTimeOffset DateTimeOffsetType { get; set; }
            TimeSpan TimeSpanType { get; set; }

            Guid GuidType { get; set; }

            EnumType EnumType { get; set; }
            DtoEnumType DtoEnumType { get; set; }

            object ObjectType { get; set; }
        }

        public class TFieldsMembers
        {
            public bool BooleanType;
            public char CharType;
            public sbyte SByteType;
            public byte ByteType;
            public short Int16SType;
            public ushort UInt16Type;
            public int Int32Type;
            public uint UInt32Type;
            public long Int64Type;
            public ulong UInt64Type;
            public float SingleType;
            public double DoubleType;
            public decimal DecimalType;
            public string StringType;

            public DateTime DateTimeType;
            public DateTimeOffset DateTimeOffsetType;
            public TimeSpan TimeSpanType;

            public Guid GuidType;

            public EnumType EnumType;
            public DtoEnumType DtoEnumType;

            public object ObjectType;
        }

        public class TClassMembers : ISystemTypeCodes
        {
            public bool BooleanType { get; set; }
            public char CharType { get; set; }
            public sbyte SByteType { get; set; }
            public byte ByteType { get; set; }
            public short Int16SType { get; set; }
            public ushort UInt16Type { get; set; }
            public int Int32Type { get; set; }
            public uint UInt32Type { get; set; }
            public long Int64Type { get; set; }
            public ulong UInt64Type { get; set; }
            public float SingleType { get; set; }
            public double DoubleType { get; set; }
            public decimal DecimalType { get; set; }
            public string StringType { get; set; }

            public DateTime DateTimeType { get; set; }
            public DateTimeOffset DateTimeOffsetType { get; set; }
            public TimeSpan TimeSpanType { get; set; }

            public Guid GuidType { get; set; }

            public EnumType EnumType { get; set; }
            public DtoEnumType DtoEnumType { get; set; }

            public object ObjectType { get; set; }
        }

        public class TLiteralMembers
        {
            public const bool BooleanType = true;
            public const char CharType = '0';
            public const sbyte SByteType = 1;
            public const byte ByteType = 2;
            public const short Int16SType = 3;
            public const ushort UInt16Type = 4;
            public const int Int32Type = 5;
            public const uint UInt32Type = 6;
            public const long Int64Type = 7;
            public const ulong UInt64Type = 8;
            public const float SingleType = 9;
            public const double DoubleType = 10;
            public const decimal DecimalType = 11;
            public const string StringType = "12";
        }

        public class TLiteralSegment
        {
            public TLiteralMembers Members { get; set; }
        }

        public class TLiteralNode
        {
            public const string Name = nameof(Name);
            public TLiteralSegment Segment { get; set; }
        }

        public class TReadonlyMembers
        {
            public bool BooleanType { get; }
            public char CharType { get; }
            public sbyte SByteType { get; }
            public byte ByteType { get; }
            public short Int16SType { get; }
            public ushort UInt16Type { get; }
            public int Int32Type { get; }
            public uint UInt32Type { get; }
            public long Int64Type { get; }
            public ulong UInt64Type { get; }
            public float SingleType { get; }
            public double DoubleType { get; }
            public decimal DecimalType { get; }
            public string StringType { get; }

            public DateTime DateTimeType { get; }
            public DateTimeOffset DateTimeOffsetType { get; }
            public TimeSpan TimeSpanType { get; }

            public Guid GuidType { get; }

            public EnumType EnumType { get; }
            public DtoEnumType DtoEnumType { get; }

            public object ObjectType { get; }
        }

        public class TNullableMembers
        {
            public bool? BooleanType { get; set; }
            public char? CharType { get; set; }
            public sbyte? SByteType { get; set; }
            public byte? ByteType { get; set; }
            public short? Int16SType { get; set; }
            public ushort? UInt16Type { get; set; }
            public int? Int32Type { get; set; }
            public uint? UInt32Type { get; set; }
            public long? Int64Type { get; set; }
            public ulong? UInt64Type { get; set; }
            public float? SingleType { get; set; }
            public double? DoubleType { get; set; }
            public decimal? DecimalType { get; set; }
            public string StringType { get; set; }

            public DateTime? DateTimeType { get; set; }
            public DateTimeOffset? DateTimeOffsetType { get; set; }
            public TimeSpan? TimeSpanType { get; set; }

            public Guid? GuidType { get; set; }

            public EnumType? EnumType { get; set; }
            public DtoEnumType? DtoEnumType { get; set; }

            public object ObjectType { get; set; }
        }

        public class TStaticMembers
        {
            public static bool BooleanType { get; set; }
            public static char CharType { get; set; }
            public static sbyte SByteType { get; set; }
            public static byte ByteType { get; set; }
            public static short Int16SType { get; set; }
            public static ushort UInt16Type { get; set; }
            public static int Int32Type { get; set; }
            public static uint UInt32Type { get; set; }
            public static long Int64Type { get; set; }
            public static ulong UInt64Type { get; set; }
            public static float SingleType { get; set; }
            public static double DoubleType { get; set; }
            public static decimal DecimalType { get; set; }
            public static string StringType { get; set; }

            public static DateTime DateTimeType { get; set; }
            public static DateTimeOffset DateTimeOffsetType { get; set; }
            public static TimeSpan TimeSpanType { get; set; }

            public static Guid GuidType { get; set; }

            public static EnumType EnumType { get; set; }
            public static DtoEnumType DtoEnumType { get; set; }

            public static object ObjectType { get; set; }
        }

        public class TNullableStaticMembers
        {
            public static bool? BooleanType { get; set; }
            public static char? CharType { get; set; }
            public static sbyte? SByteType { get; set; }
            public static byte? ByteType { get; set; }
            public static short? Int16SType { get; set; }
            public static ushort? UInt16Type { get; set; }
            public static int? Int32Type { get; set; }
            public static uint? UInt32Type { get; set; }
            public static long? Int64Type { get; set; }
            public static ulong? UInt64Type { get; set; }
            public static float? SingleType { get; set; }
            public static double? DoubleType { get; set; }
            public static decimal? DecimalType { get; set; }
            public static string StringType { get; set; }

            public static DateTime? DateTimeType { get; set; }
            public static DateTimeOffset? DateTimeOffsetType { get; set; }
            public static TimeSpan? TimeSpanType { get; set; }

            public static Guid? GuidType { get; set; }

            public static EnumType? EnumType { get; set; }
            public static DtoEnumType? DtoEnumType { get; set; }

            public static object ObjectType { get; set; }
        }

        public struct TStructSegment
        {
            public TStructMembers Members { get; set; }
            public TNullableStructMembers NullableMembers { get; set; }
        }

        public struct TStructMembers
        {
            public bool BooleanType { get; set; }
            public char CharType { get; set; }
            public sbyte SByteType { get; set; }
            public byte ByteType { get; set; }
            public short Int16SType { get; set; }
            public ushort UInt16Type { get; set; }
            public int Int32Type { get; set; }
            public uint UInt32Type { get; set; }
            public long Int64Type { get; set; }
            public ulong UInt64Type { get; set; }
            public float SingleType { get; set; }
            public double DoubleType { get; set; }
            public decimal DecimalType { get; set; }
            public string StringType { get; set; }

            public DateTime DateTimeType { get; set; }
            public DateTimeOffset DateTimeOffsetType { get; set; }
            public TimeSpan TimeSpanType { get; set; }

            public Guid GuidType { get; set; }

            public EnumType EnumType { get; set; }
            public DtoEnumType DtoEnumType { get; set; }

            public object ObjectType { get; set; }
        }

        public struct TNullableStructMembers
        {
            public bool? BooleanType { get; set; }
            public char? CharType { get; set; }
            public sbyte? SByteType { get; set; }
            public byte? ByteType { get; set; }
            public short? Int16SType { get; set; }
            public ushort? UInt16Type { get; set; }
            public int? Int32Type { get; set; }
            public uint? UInt32Type { get; set; }
            public long? Int64Type { get; set; }
            public ulong? UInt64Type { get; set; }
            public float? SingleType { get; set; }
            public double? DoubleType { get; set; }
            public decimal? DecimalType { get; set; }
            public string StringType { get; set; }

            public DateTime? DateTimeType { get; set; }
            public DateTimeOffset? DateTimeOffsetType { get; set; }
            public TimeSpan? TimeSpanType { get; set; }

            public Guid? GuidType { get; set; }

            public EnumType? EnumType { get; set; }
            public DtoEnumType? DtoEnumType { get; set; }

            public object ObjectType { get; set; }
        }

        public class TStaticSegment
        {
            public static TStaticMembers Members { get; set; }
            public static TNullableStaticMembers NullableMembers { get; set; }
        }

        public class TStaticWrapper
        {
            public TStaticSegment Segment { get; set; }
        }

        public class TSegment
        {
            public TClassMembers Members { get; set; }
            public TNullableMembers NullableMembers { get; set; }
        }

        public class D1ClassNodeWithMembers<T>
        {
            public T Members { get; set; }
        }
        public class D1ClassNodeWithNullableMembers<T> where T : struct
        {
            public T? Members { get; set; }
        }

        public class TD1ClassNodeWithClassMembers : D1ClassNodeWithMembers<TClassMembers> { }
        public class TD1ClassNodeWithStructMembers : D1ClassNodeWithMembers<TStructMembers> { }
        public class TD1ClassNodeWithNullableMembers : D1ClassNodeWithNullableMembers<TStructMembers> { }

        public class TD1ClassNodeWithStaticClassMembers { public static TClassMembers Members { get; set; } }
        public class TD1ClassNodeWithStaticStructMembers { public static TStructMembers Members { get; set; } }
        public class TD1ClassNodeWithStaticNullableMembers { public static TStructMembers? Members { get; set; } }

        public class TD1ClassNodeWithStaticNullableStructMembers { public static TStructMembers Members { get; set; } }
    }
}
