using System;
using System.Linq;
using System.Reflection;

namespace SimpleSharpTemplateEngine.Helpers
{
    internal class PropertyHelper
    {
        // Handles:
        // model.MyProperty
        // model.MyChild.MyProperty
        internal static object GetReferencedProperty(object model, string chainedPropertyName)
        {
            var propertyNames = chainedPropertyName.Split('.');

            var currentObject = model;
            for (int i = 0; i < propertyNames.Length; i++)
            {
                var propertyName = propertyNames[i];

                Type modelType = currentObject.GetType();
                PropertyInfo[] properties = modelType.GetProperties();
                var propertyInfo = properties.FirstOrDefault(x => x.Name.ToLower() == propertyName.ToLower());

                if (propertyInfo == null)
                    throw new TemplateEngineException($"Unable to locate the property ##{chainedPropertyName}##");

                currentObject = modelType.GetProperty(propertyInfo.Name).GetValue(currentObject);

                if (i == propertyNames.Length - 1)
                {
                    return currentObject;
                }
            }

            throw new TemplateEngineException($"Failed to retrieve the property {chainedPropertyName}");
        }
    }
}
