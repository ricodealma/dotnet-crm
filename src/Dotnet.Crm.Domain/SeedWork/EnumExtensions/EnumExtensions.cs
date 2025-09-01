using System.ComponentModel;
using System.Reflection;

namespace Dotnet.Crm.Domain.SeedWork.EnumExtensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static bool IsValidEnumValue<TEnum>(string value) where TEnum : Enum
        {
            if (Enum.TryParse(typeof(TEnum), value, true, out _))
            {
                return true;
            }

            return typeof(TEnum)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Any(field =>
                {
                    var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                    return attribute != null && attribute.Description.Equals(value, StringComparison.OrdinalIgnoreCase);
                });
        }

        public static bool IsValidEnumValue<TEnum>(int value) where TEnum : Enum
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }
    }
}