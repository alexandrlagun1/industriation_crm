using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.NumberMask
{
    public static class NumberMask
    {
        public static NumberFormatInfo ni = new CultureInfo(CultureInfo.CurrentCulture.Name).NumberFormat;
        public static NumberFormatInfo GetNi()
        {
            ni.NumberDecimalDigits = 2;
            ni.NumberGroupSeparator = " ";
            ni.NumberGroupSizes = new int[] { 3 };
            return ni;
        }
    }
}
