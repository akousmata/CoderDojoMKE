using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace CoderDojoMKE.Web.Models.CustomAnnotations
{
    public class NotEqualAttribute : ValidationAttribute, IClientValidatable
    {
        public string OtherProperty { get; private set; }
        public NotEqualAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(OtherProperty);
            if (property == null)
            {
                return new ValidationResult(
                    string.Format(
                        CultureInfo.CurrentCulture, 
                        "{0} is unknown property", 
                        OtherProperty
                    )
                );
            }

            var otherValue = property.GetValue(validationContext.ObjectInstance, null);
            if (object.Equals(value, otherValue))
            {
                string otherDisplayName = "";
                try
                {
                    otherDisplayName = property.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name;
                }
                catch (Exception)
                {
                    otherDisplayName = property.Name;
                }
                
                return new ValidationResult(validationContext.DisplayName + " cannot be the same as " + otherDisplayName);
            }
            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "notequalto",
            };
            rule.ValidationParameters["other"] = OtherProperty;
            yield return rule;
        }
    }
}