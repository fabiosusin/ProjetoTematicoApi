using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Useful.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Get the Description from the DescriptionAttribute
        /// </summary>
        public static string GetDescription(this Enum enumValue) =>
            enumValue?.GetType()?.GetMember(enumValue.ToString())?.FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty;

        /// <summary>
        /// Get the enum value from the matching DescriptionAttribute
        /// </summary>
        public static T GetByDescription<T>(this T enumDefaultValue, string description) where T : Enum
        {
            var value = description == null ? null : enumDefaultValue.GetType().GetFields().FirstOrDefault(x => x.GetCustomAttribute<DescriptionAttribute>()?.Description == description)?.GetValue(null);
            return value == null ? enumDefaultValue : (T)value;
        }
    }
}
