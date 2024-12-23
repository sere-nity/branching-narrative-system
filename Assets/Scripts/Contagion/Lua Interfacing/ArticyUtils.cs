using PixelCrushers.DialogueSystem;

namespace Contagion.Lua_Interfacing
{
    public static class ArticyUtils
    {
        public static string GetVariableFromLuaExpression(string variable)
        {
            if (string.IsNullOrEmpty(variable))
            {
                return variable;
            }
            int num = variable.IndexOf('[');
            int num2 = variable.IndexOf(']');
            if (num > 0 && num2 > num)
            {
                return variable.Substring(num + 2, num2 - num - 3);
            }
            return variable;
        }
        
        // New SetTableValue method
        public static void SetTableValue(string table, string element, object value)
        {
            Lua.Run(string.Format("{0}['{1}']={2}", table, element, FormatLuaValue(value)));
        }

        // Helper method for formatting Lua values
        private static string FormatLuaValue(object value)
        {
            if (value == null)
            {
                return "";
            }

            // Handle numeric strings
            if (float.TryParse(value as string, out float result))
            {
                return result.ToString();
            }

            // Handle strings
            if (value is string stringValue)
            {
                // If string starts with [ or { (likely a table or array), return as-is
                if (stringValue.StartsWith("[") || stringValue.StartsWith("{"))
                {
                    return stringValue;
                }
                // Otherwise wrap in quotes
                return $"\"{stringValue}\"";
            }

            // All other types converted to lowercase string
            return value.ToString().ToLower();
        }

        // Convenience method for setting variables
        public static void SetVariable(string variableName, object value)
        {
            SetTableValue("Variable", variableName, value);
        }
    }
}