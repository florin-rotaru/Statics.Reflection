namespace Statics.Reflection.Emit
{
    public partial class ILGenerator
    {
        public static class Nullables
        {
            public static T GetValue<T>(T? source)
                where T : struct =>
                source.Value;

            public static T GetValueOrDefault<T>(T? source)
                where T : struct =>
                source.GetValueOrDefault();

            public static T? GetDefaultValue<T>()
                where T : struct =>
                default;
        }
    }
}
