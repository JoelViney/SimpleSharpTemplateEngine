using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class PropertyContainer : ITemplateObject
    {
        public string PropertyName { get; }

        public PropertyContainer(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        // Handles:
        // model.MyProperty
        // model.MyChild.MyProperty
        public StringBuilder Process(object model)
        {
            var propertyNames = this.PropertyName.Split('.');

            var currentObject = model;
            for(int i = 0; i < propertyNames.Length; i++) 
            {
                var propertyName = propertyNames[i];

                Type modelType = currentObject.GetType();
                PropertyInfo[] properties = modelType.GetProperties();
                var property = properties.FirstOrDefault(x => x.Name.ToLower() == propertyName.ToLower());

                if (property == null)
                    throw new TemplateEngineException($"Unable to locate the property ##{this.PropertyName}##");

                currentObject = modelType.GetProperty(property.Name).GetValue(currentObject);

                if (i == propertyNames.Length - 1)
                {
                    if (currentObject is string value)
                    {
                        return new StringBuilder(value);
                    }
                    else
                    {
                        return new StringBuilder();
                    }
                }
            }

            return new StringBuilder();
        }
    }
}
