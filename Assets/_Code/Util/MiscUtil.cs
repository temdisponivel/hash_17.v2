using System;

namespace HASH
{
    public static class MiscUtil
    {
        public static bool TryParseEnum<T>(string value, out T result)
        {
            result = default(T);
            
            var type = typeof(T);
            if (!type.IsEnum)
                return false;

            bool found = false;
            var names = Enum.GetNames(type);
            for (int i = 0; i < names.Length; i++)
            {
                var name = names[i];
                if (string.Equals(name, value, StringComparison.InvariantCultureIgnoreCase))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                return false;

            result = (T) Enum.Parse(type, value, true);
            return true;
        }
    }
}