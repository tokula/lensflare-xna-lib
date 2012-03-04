using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util {
    public static class EnumExtension {
        public static int GetLength<T>() {
            return Enum.GetNames(typeof(T)).Length;
        }
    }
}
