using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accountings.Common.Helpers
{
    public static class StringHelper
    {
        public static string ToNormalize(this string text)
        {
            return text.Trim().ToUpperInvariant();
        }
    }
}
