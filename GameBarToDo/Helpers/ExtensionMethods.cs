using System;

namespace GameBarToDo.Helpers
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checks whether the last character in a string is a return
        /// </summary>
        /// <param name="str">String to be checked</param>
        /// <returns>True or False</returns>
        public static bool IsLastCharReturn(this String str)
        {
            if (str.Substring(str.Length - 1, 1) == "\r")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Removes all return characters from a string
        /// </summary>
        /// <param name="str">String to be cleaned</param>
        /// <returns>New string</returns>
        public static string RemoveReturnChars(this String str)
        {
            if (str.Contains("\r"))
            {
                string newStr = str.Replace("\r", "");
                return newStr;
            }

            return str;
        }
    }
}
