﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBarToDo.Helpers
{
    public static class ExtensionMethods
    {
        public static bool IsLastCharReturn(this String str)
        {
            if (str.Substring(str.Length - 1, 1) == "\r")
                return true;
            else
                return false;
        }
    }
}