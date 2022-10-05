using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Useful.Extensions
{
    public static class ObjectExtension
    {
		public static string GetQueryStringFromObject(this object obj)
		{
			if(obj == null)
				return string.Empty;

			var myDict = new Dictionary<string, dynamic>();
			var type = obj.GetType();
			foreach (PropertyInfo pi in type.GetProperties())
			{
				var value = pi.GetValue(obj);
				if (value == null)
					continue;

				var pt = pi.PropertyType;
				if (pt == typeof(string) && string.IsNullOrEmpty((string)value))
					continue;
				if (pt == typeof(int) && (int)value == 0)
					continue;
				if (pt == typeof(long) && (long)value == 0)
					continue;

				myDict[pi.Name.FirstCharacterToLower()] = value.ToString();
			}

			return string.Join("&", myDict.Select(x => $"{ x.Key }={ x.Value }"));
		}
	}
}
