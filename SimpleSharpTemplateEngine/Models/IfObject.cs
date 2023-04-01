using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class IfObject : ITemplateObject
    {
        public string PropertyName { get; set; }
        public ContainerObject Contents { get; set; }

        public StringBuilder Process(object model)
        {
            Type modelType = model.GetType();
            PropertyInfo[] properties = modelType.GetProperties();
            var property = properties.FirstOrDefault(x => x.Name.ToLower() == this.PropertyName.ToLower());

            if (property == null)
            {
                throw new Exception($"Unable to locate the property ##{this.PropertyName}##");
            }
            if (!typeof(bool).IsAssignableFrom(property.PropertyType))
            {
                throw new Exception($"The if variable ##{this.PropertyName}## isn't a boolean.");
            }

            bool value = (bool)property.GetValue(model, null);

            if (value)
            {
                return this.Contents.Process(model);
            }
            else
            {
                return new StringBuilder();
            }
        }
    }
}
