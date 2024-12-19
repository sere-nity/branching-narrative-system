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
    }
}