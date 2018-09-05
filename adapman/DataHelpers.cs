using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convert = System.Convert;

namespace adapman
{
    public static class DataHelpers
    {
        public static int GetValueWithDefault(this object value, int @default)
        {
            var result = @default;

            if (value == null)
                return result;

            try
            {
                result = Convert.ToInt32(value);
            }
            catch
            {
                result = @default;
            }

            return result;
        }

        public static string GetValueWithDefault(this object value, string @default)
        {
            var result = @default;

            if (value == null)
                return result;

            try
            {
                result = value.ToString();
            }
            catch
            {
                result = @default;
            }

            return result;
        }

        public static bool GetValueWithDefault(this object value, bool @default)
        {
            var result = @default;

            if (value == null)
                return result;

            try
            {
                result = Convert.ToBoolean(value);
            }
            catch
            {
                result = @default;
            }

            return result;
        }
    }
}
