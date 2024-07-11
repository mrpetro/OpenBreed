using OpenBreed.Editor.VM.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace OpenBreed.Editor.UI.Wpf.ValidationRules
{
    public class ReferenceValidationRule : ValidationRule
    {
        public ReferenceValidationRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return ValidationResult.ValidResult;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            var result = base.Validate(value, cultureInfo, owner);
            var vm = (EntryRefIdEditorVM)((BindingExpression)owner).DataItem;

            if (vm is null)
            {
                return ValidationResult.ValidResult;
            }

            if (value is not string refId)
            {
                return new ValidationResult(false, "Reference ID can't be null.");
            }

            if (vm.IsValid)
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, vm.ValidationMessage);
        }
    }
}
