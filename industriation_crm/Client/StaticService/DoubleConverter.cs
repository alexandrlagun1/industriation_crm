using System.ComponentModel;

namespace industriation_crm.Client
{
    public static class DoubleConverter
    {
        public static double? ConvertDouble(string? value)
        {
            double? @double = null;
            
            try
            {
                value = value.Replace('.', ',');
                @double = Convert.ToDouble(value);
            }
            catch
            {
                try
                {
                    value = value.Replace(',', '.');
                    @double = Convert.ToDouble(value);
                }
                catch
                {
                    @double = 0;
                }
            }
            return @double;
        }
    }
}
