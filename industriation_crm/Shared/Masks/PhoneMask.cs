using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Masks
{
    public static class PhoneMask
    {
        public static string GetNumber(string? phone)
        {
            string? format_phone = "";
            if (phone?.Count() == 11)
            {
                format_phone = $"+{phone[0]} ({phone[1]}{phone[2]}{phone[3]}) {phone[4]}{phone[5]}{phone[6]}-{phone[7]}{phone[8]}-{phone[9]}{phone[10]}";
            }
            else
                return phone;
            return format_phone;
        }
    }
}
