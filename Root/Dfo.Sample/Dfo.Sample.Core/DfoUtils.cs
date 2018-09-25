using System;
using System.Configuration;

namespace Dfo.Sample.Core
{
    public class DfoUtils
    {
        public static string GetStringKey(string keyName, string defaultValue)
        {
            string value = GetConfigApp(keyName);
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        public static bool GetBoolKey(string keyName, bool defaultValue)
        {
            string value = GetConfigApp(keyName);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            return bool.TryParse(value, out bool returnValue) ? returnValue : defaultValue;
        }

        public static int GetIntKey(string keyName, int defaultValue)
        {
            string value = GetConfigApp(keyName);
            return int.TryParse(value, out int returnValue) ? returnValue : defaultValue;
        }

        public static long GetLongKey(string keyName, long defaultValue)
        {
            string value = GetConfigApp(keyName);
            return long.TryParse(value, out long returnValue) ? returnValue : defaultValue;
        }

        public static Guid GetGuiKey(string keyName, Guid defaultValue)
        {
            string value = GetConfigApp(keyName);
            return Guid.TryParse(value, out Guid returnValue) ? returnValue : defaultValue;
        }

        public static string GetConnectionString(string keyName)
        {
            return ConfigurationManager.ConnectionStrings[keyName].ConnectionString;
        }

        public static string GetConfigApp(string keyName)
        {
            return ConfigurationManager.AppSettings[keyName];
        }
    }
}