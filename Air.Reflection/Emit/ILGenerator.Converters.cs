using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Air.Reflection.Emit
{
    public partial class ILGenerator
    {
        public static class Converters
        {
            public static class Enum<TEnum> where TEnum : struct
            {
                private static readonly Dictionary<TEnum, string> Values = Enum.GetNames(typeof(TEnum)).ToDictionary(k => Enum.Parse<TEnum>(k), v => v);

                public static string GetName(TEnum value) => Values[value];
            }

            #region
            public static T[] ToArray<T>(ICollection<T> collection)
            {
                T[] array = new T[collection.Count];
                collection.CopyTo(array, 0);
                return array;
            }

            public static T[] ToArray<T>(IProducerConsumerCollection<T> collection)
            {
                T[] array = new T[collection.Count];
                collection.CopyTo(array, 0);
                return array;
            }
            #endregion

            #region TimeSpan
            public static TimeSpan ToTimeSpan(long value) =>
                new TimeSpan(value);

            public static TimeSpan ToTimeSpan(long? value) =>
                new TimeSpan(value.GetValueOrDefault());

            public static TimeSpan ToTimeSpan(TimeSpan? value) =>
                value.GetValueOrDefault();

            public static TimeSpan ToTimeSpan(object value)
            {
                Type valueType = value.GetType();

                if (value == null)
                    return default;

                if (valueType == typeof(TimeSpan))
                    return (TimeSpan)value;

                if (valueType == typeof(TimeSpan?))
                    return ToTimeSpan((TimeSpan?)value);

                if (valueType == typeof(long))
                    return ToTimeSpan((long)value);

                if (valueType == typeof(long?))
                    return ToTimeSpan((long?)value);

                throw new Exception($"Cannot convert from {valueType} to {typeof(TimeSpan)}");
            }
            #endregion

            #region DateTimeOffset
            public static DateTimeOffset ToDateTimeOffset(DateTime value) =>
                new DateTimeOffset(value);

            public static DateTimeOffset ToDateTimeOffset(DateTime? value) =>
                new DateTimeOffset(value.GetValueOrDefault());

            public static DateTimeOffset ToDateTimeOffset(DateTimeOffset? value) =>
                value.GetValueOrDefault();

            public static DateTimeOffset ToDateTimeOffset(object value)
            {
                Type valueType = value.GetType();

                if (value == null)
                    return default;

                if (valueType == typeof(DateTimeOffset))
                    return (DateTimeOffset)value;

                if (valueType == typeof(DateTimeOffset?))
                    return ToDateTimeOffset((DateTimeOffset?)value);

                if (valueType == typeof(DateTime))
                    return ToDateTimeOffset((DateTime)value);

                if (valueType == typeof(DateTime?))
                    return ToDateTimeOffset((DateTime?)value);

                throw new Exception($"Cannot convert from {valueType} to {typeof(DateTimeOffset)}");
            }
            #endregion

            #region Guid
            public static Guid ToGuid(string value) =>
                new Guid(value);

            public static Guid ToGuid(Guid? value) =>
                value.GetValueOrDefault();

            public static Guid ToGuid(object value)
            {
                Type valueType = value.GetType();

                if (value == null)
                    return default;

                if (valueType == typeof(Guid))
                    return (Guid)value;

                if (valueType == typeof(Guid?))
                    return ToGuid((Guid?)value);

                if (valueType == typeof(string))
                    return ToGuid((string)value);

                throw new Exception($"Cannot convert from {valueType} to {typeof(Guid)}");
            }
            #endregion
        }
    }
}