using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestDA.Common
{
    public class ValidateNotValidForProperty
    {
        /// <summary>
        /// /validate for decimal
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
        public class ValidDecimal : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return ValidationResult.Success;
                }
                decimal d;

                return !decimal.TryParse(value.ToString(), out d) ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
            }
        }

        ////validate for interger
        ///
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
        public class ValidInteger : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return ValidationResult.Success;
                }
                int i;

                return !int.TryParse(value.ToString(), out i) ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
            }
        }
        public class ValidDecimalValidator : DataAnnotationsModelValidator<TestDA.Common.ValidateNotValidForProperty.ValidDecimal>
        {
            public ValidDecimalValidator(ModelMetadata metadata, ControllerContext context, TestDA.Common.ValidateNotValidForProperty.ValidDecimal attribute)
                : base(metadata, context, attribute)
            {
                if (!attribute.IsValid(context.HttpContext.Request.Form[metadata.PropertyName]))
                {
                    var propertyName = metadata.PropertyName;
                    context.Controller.ViewData.ModelState[propertyName].Errors.Clear();
                    context.Controller.ViewData.ModelState[propertyName].Errors.Add(attribute.ErrorMessage);
                }
            }
        }

        public class ValidIntegerValidator : DataAnnotationsModelValidator<TestDA.Common.ValidateNotValidForProperty.ValidInteger>
        {
            public ValidIntegerValidator(ModelMetadata metadata, ControllerContext context, TestDA.Common.ValidateNotValidForProperty.ValidInteger attribute)
                : base(metadata, context, attribute)
            {
                if (!attribute.IsValid(context.HttpContext.Request.Form[metadata.PropertyName]))
                {
                    var propertyName = metadata.PropertyName;
                    context.Controller.ViewData.ModelState[propertyName].Errors.Clear();
                    context.Controller.ViewData.ModelState[propertyName].Errors.Add(attribute.ErrorMessage);
                }
            }
        }
    }
}