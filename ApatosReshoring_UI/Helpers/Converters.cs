using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_UI.Helpers
{
    internal static class Converters
    {
        public static string DecimalFeetToFeetInches_32ndInch(double? value)
        {
            double _value = value == null || value.HasValue == false
                    ? 0.0
                    : value.Value;

            double _feet = Math.Floor(_value);

            double _inches = Math.Floor((_value - _feet) * 12.0);

            double _fractionalInches = Math.Ceiling((_value - _feet - (_inches / 12.0)) / 32.0);

            string _display =
                Convert.ToInt32(_feet) + "' " +
                Convert.ToInt32(_inches);

            if (_fractionalInches > 0.0) _display += "-" + Convert.ToInt32(_fractionalInches) + "/32\"";
            else _display += "\"";

            return _display;
        }

        public static double FeetInchesToDecimalFeet(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0.0;

            double? _nullableFeet = null;
            double? _nullableInches = null;
            double? _nullableFractionalInches = null;
            double? _nullableInchDividedBy = null;

            foreach (string _valuePart in value.Split(new string[] { "' ", " ", "-", "/", "\"" }, StringSplitOptions.RemoveEmptyEntries))
            { 
                if (_nullableFeet == null)
                {
                    if (double.TryParse(_valuePart.Trim(), out double _feet))
                    {
                        _nullableFeet = _feet;
                    }
                    else
                    {
                        _nullableFeet = 0;
                    }
                }
                else if (_nullableInches == null)
                {
                    if (double.TryParse(_valuePart.Trim(), out double _inches))
                    {
                        _nullableInches = _inches;
                    }
                    else
                    {
                        _nullableInches = 0;
                    }
                }
                else if (_nullableFractionalInches == null)
                {
                    if (double.TryParse(_valuePart.Trim(), out double _fractionalInches))
                    {
                        _nullableFractionalInches = _fractionalInches;
                    }
                    else
                    {
                        _nullableFractionalInches = 0;
                    }
                }
                else if (_nullableInchDividedBy == null)
                {
                    if (double.TryParse(_valuePart.Trim(), out double _inchDividedBy))
                    {
                        _nullableInchDividedBy = _inchDividedBy;
                    }
                    else
                    {
                        _nullableInchDividedBy = 0;
                    }
                }
            }

            double _value = _nullableFeet.Value;
            if (_nullableInches != null && _nullableInches.HasValue) _value += _nullableInches.Value / 12.0;
            if (_nullableFractionalInches != null && _nullableFractionalInches.HasValue && _nullableInchDividedBy != null && _nullableInchDividedBy.HasValue) _value += _nullableFractionalInches.Value / ( 12.0 * _nullableInchDividedBy.Value);

            return _value;
        }

        public static string ToPCF(double? value)
        {
            double _value = value == null || value.HasValue == false
                ? 0.0
                : value.Value;
            return Math.Round(_value, 3).ToString("N3") + " PCF";
        }

        public static double FromPCF(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0.0;

            string _sanitizedValue = string.Empty;
            if (value.Contains("PCF")) _sanitizedValue = value.Replace("PCF", string.Empty);
            else _sanitizedValue = value;

            double _result = 0.0;
            if (double.TryParse(_sanitizedValue.Trim(), out _result) == false)
            {
                string _numericValue = string.Empty;
                foreach (char _char in _sanitizedValue)
                {
                    if (char.IsDigit(_char) || _char == '.') _numericValue += _char;
                }
                double.TryParse(_numericValue, out _result);
            }

            return _result;
        }

        public static string ToPSF(double? value)
        {
            double _value = value == null || value.HasValue == false
                ? 0.0
                : value.Value;
            return Math.Round(_value, 3).ToString("N3") + " PSF";
        }

        public static double FromPSF(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0.0;

            string _sanitizedValue = string.Empty;
            if (value.Contains("PSF")) _sanitizedValue = value.Replace("PSF", string.Empty);
            else _sanitizedValue = value;

            double _result = 0.0;
            if (double.TryParse(_sanitizedValue.Trim(), out _result) == false)
            {
                string _numericValue = string.Empty;
                foreach (char _char in _sanitizedValue)
                {
                    if (char.IsDigit(_char) || _char == '.') _numericValue += _char;
                }
                double.TryParse(_numericValue, out _result);
            }

            return _result;
        }

        public static string ToPLF(double? value)
        {
            double _value = value == null || value.HasValue == false
                ? 0.0
                : value.Value;
            return Math.Round(_value, 3).ToString("N3") + " PLF";
        }

        public static double FromPLF(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0.0;

            string _sanitizedValue = string.Empty;
            if (value.Contains("PLF")) _sanitizedValue = value.Replace("PLF", string.Empty);
            else _sanitizedValue = value;

            double _result = 0.0;
            if (double.TryParse(_sanitizedValue.Trim(), out _result) == false)
            {
                string _numericValue = string.Empty;
                foreach (char _char in _sanitizedValue)
                {
                    if (char.IsDigit(_char) || _char == '.') _numericValue += _char;
                }
                double.TryParse(_numericValue, out _result);
            }

            return _result;
        }

        public static string ToString(double? value, int decimals)
        {
            double _value = value == null || value.HasValue == false
                ? 0.0
                : value.Value;

            return _value.ToString("N" + decimals);
        }

        public static double ToDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0.0;

            double _result = 0.0;
            if (double.TryParse(value.Trim(), out _result) == false)
            {
                string _numericValue = string.Empty;
                foreach (char _char in value)
                {
                    if (char.IsDigit(_char) || _char == '.') _numericValue += _char;
                }
                double.TryParse(_numericValue, out _result);
            }
            return _result;
        }

        public static string ToString(int? value)
        {
            int _value = value == null || value.HasValue == false
                ? 0
                : value.Value;

            return _value.ToString();
        }

        public static int ToInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            int _result = 0;
            if (int.TryParse(value.Trim(), out _result) == false)
            {
                string _numericValue = string.Empty;
                foreach (char _char in value)
                {
                    if (char.IsDigit(_char) || _char == '.') _numericValue += _char;
                }
                int.TryParse(_numericValue, out _result);
            }
            return _result;
        }
    }
}
