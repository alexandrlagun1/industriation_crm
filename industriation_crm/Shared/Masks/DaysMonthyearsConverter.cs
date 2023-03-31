using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared
{
    public static class DaysMonthyearsConverter
    {
        public static string GetDaysFormat(int? day)
        {
            if(day == null)
                return string.Empty;
            string format_day = "рабочий день";
            if (day >= 100)
            {
                day = day / 10;
            }
            if(day > 1 && day < 5)
                format_day = "рабочих дня";
            if(day > 4 && day < 21)
                format_day = "рабочих дней";
            if(day > 21 && day < 25)
                format_day = "рабочих дня";
            if(day > 24 && day < 31)
                format_day = "рабочих дней";
            if (day > 31 && day < 35)
                format_day = "рабочих дня";
            if (day > 34 && day < 41)
                format_day = "рабочих дней";
            if (day > 41 && day < 45)
                format_day = "рабочих дня";
            if (day > 44 && day < 51)
                format_day = "рабочих дней";
            if (day > 51 && day < 55)
                format_day = "рабочих дня";
            if (day > 54 && day < 61)
                format_day = "рабочих дней";
            if (day > 61 && day < 65)
                format_day = "рабочих дня";
            if (day > 64 && day < 71)
                format_day = "рабочих дней";
            if (day > 71 && day < 75)
                format_day = "рабочих дня";
            if (day > 74 && day < 81)
                format_day = "рабочих дней";
            if (day > 81 && day < 85)
                format_day = "рабочих дня";
            if (day > 84 && day < 91)
                format_day = "рабочих дней";
            if (day > 91 && day < 95)
                format_day = "рабочих дня";
            if (day > 94)
                format_day = "рабочих дней";
            return format_day;
        }
        public static string GetMonthFormat(int? month)
        {
            if (month == null)
                return string.Empty;
            string format_day = "месяц";
            if (month >= 100)
            {
                month = month / 10;
            }
            if (month > 1 && month < 5)
                format_day = "месяца";
            if (month > 4 && month < 21)
                format_day = "месяцев";
            if (month > 21 && month < 25)
                format_day = "месяца";
            if (month > 24 && month < 31)
                format_day = "месяцев";
            if (month > 31 && month < 35)
                format_day = "месяца";
            if (month > 34 && month < 41)
                format_day = "месяцев";
            if (month > 41 && month < 45)
                format_day = "месяца";
            if (month > 44 && month < 51)
                format_day = "месяцев";
            if (month > 51 && month < 55)
                format_day = "месяца";
            if (month > 54 && month < 61)
                format_day = "месяцев";
            if (month > 61 && month < 65)
                format_day = "месяца";
            if (month > 64 && month < 71)
                format_day = "месяцев";
            if (month > 71 && month < 75)
                format_day = "месяца";
            if (month > 74 && month < 81)
                format_day = "месяцев";
            if (month > 81 && month < 85)
                format_day = "месяца";
            if (month > 84 && month < 91)
                format_day = "месяцев";
            if (month > 91 && month < 95)
                format_day = "месяца";
            if (month > 94)
                format_day = "месяцев";
            return format_day;
        }
        public static string GetWeekFormat(int? week)
        {
            if (week == null)
                return String.Empty;
            string format_day = "неделя";
            if (week >= 100)
            {
                week = week / 10;
            }
            if (week > 1 && week < 5)
                format_day = "недели";
            if (week > 4 && week < 21)
                format_day = "недель";
            if (week > 21 && week < 25)
                format_day = "недели";
            if (week > 24 && week < 31)
                format_day = "недель";
            if (week > 31 && week < 35)
                format_day = "недели";
            if (week > 34 && week < 41)
                format_day = "недель";
            if (week > 41 && week < 45)
                format_day = "недели";
            if (week > 44 && week < 51)
                format_day = "недель";
            if (week > 51 && week < 55)
                format_day = "недели";
            if (week > 54 && week < 61)
                format_day = "недель";
            if (week > 61 && week < 65)
                format_day = "недели";
            if (week > 64 && week < 71)
                format_day = "недель";
            if (week > 71 && week < 75)
                format_day = "недели";
            if (week > 74 && week < 81)
                format_day = "недель";
            if (week > 81 && week < 85)
                format_day = "недели";
            if (week > 84 && week < 91)
                format_day = "недель";
            if (week > 91 && week < 95)
                format_day = "недели";
            if (week > 94)
                format_day = "недель";
            return format_day;
        }
    }
}
