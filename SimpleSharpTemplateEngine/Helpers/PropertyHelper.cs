using System;
using System.Linq;
using System.Reflection;

namespace SimpleSharpTemplateEngine.Helpers
{
    internal class PropertyHelper
    {
        internal static string CleanPropertyName(string propertyName)
        {
            if (propertyName.Contains(':'))
            {
                var propertySegment = propertyName.Substring(0, propertyName.IndexOf(":"));
                var format = propertyName.Substring(propertyName.IndexOf(":") + 1);
                propertySegment = propertySegment.Replace("-", "");

                return $"{propertySegment}:{format}";
            }
            else
            {
                return propertyName.Replace("-", "");
            }
        }

        // Handles:
        // model.MyProperty
        // model.MyChild.MyProperty
        internal static (object property, string format ) GetReferencedProperty(object model, string chainedPropertyName)
        {
            var propertyNames = chainedPropertyName.Split('.');
            string format = null;

            var currentObject = model;
            for (int i = 0; i < propertyNames.Length; i++)
            {
                var propertyName = propertyNames[i];

                if (i == propertyNames.Length - 1)
                {
                    // Check for formatting
                    if (propertyName.Contains(':'))
                    {
                        format = propertyName.Substring(propertyName.IndexOf(":") + 1).Trim();
                        propertyName = propertyName.Split(':').First();
                    }
                }

                Type modelType = currentObject.GetType();
                PropertyInfo[] properties = modelType.GetProperties();
                var propertyInfo = properties.FirstOrDefault(x => x.Name.ToLower() == propertyName.ToLower());

                if (propertyInfo == null)
                    throw new TemplateEngineException($"Unable to locate the property '{chainedPropertyName}'");

                currentObject = modelType.GetProperty(propertyInfo.Name).GetValue(currentObject);

                if (i == propertyNames.Length - 1)
                {
                    return (currentObject, format);
                }
            }

            throw new TemplateEngineException($"Failed to retrieve the property {chainedPropertyName}");
        }
    }
}
