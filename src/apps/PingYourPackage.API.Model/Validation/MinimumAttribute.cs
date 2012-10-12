using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model.Validation {

    public class MinimumAttribute : ValidationAttribute {

        private readonly int _minimumValue;

        public MinimumAttribute(int minimum) : 
            base(errorMessage: "The {0} field value must be minimum {1}.") {

            _minimumValue = minimum;
        }

        public override string FormatErrorMessage(string name) {

            return string.Format(
                CultureInfo.CurrentCulture,
                base.ErrorMessageString,
                name,
                _minimumValue);
        }

        public override bool IsValid(object value) {

            int intValue;
            if (value != null && int.TryParse(value.ToString(), out intValue)) {

                return (intValue >= _minimumValue);
            }

            return false;
        }
    }
}
