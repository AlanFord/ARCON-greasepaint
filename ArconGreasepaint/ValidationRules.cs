using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;

namespace ArconGreasepaint
{
    public class IntValidationRule : ValidationRule
    {
        private int _min;
        private int _max;

        public IntValidationRule()
        {
        }

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int distance = 0;

            try
            {
                if (((string)value).Length > 0)
                    distance = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if ((distance < Min) || (distance > Max))
            {
                return new ValidationResult(false,
                  "Please enter an Integer in the range: " + Min + " to " + Max + ".");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }

    public class DoubleValidationRule : ValidationRule
    {
        private double _min;
        private double _max;

        public DoubleValidationRule()
        {
        }

        public double Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public double Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double distance = 0;

            try
            {
                if (((string)value).Length > 0)
                    distance = Double.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if ((distance < Min) || (distance > Max))
            {
                return new ValidationResult(false,
                  "Please enter a value in the range: " + Min + " to " + Max + ".");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }

    public class YesNoValidationRule : ValidationRule
    {

        public YesNoValidationRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string waldo;
            try
            {
                waldo = (string)value;
                waldo = waldo.ToLower();
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if ((waldo.Length != 1) || (waldo != "n" && waldo != "y") )
            {
                return new ValidationResult(false,
                  "Please enter a single character, either y or n");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}

