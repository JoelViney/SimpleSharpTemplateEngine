using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class PropertyObject : ITemplateObject
    {
        public string PropertyName { get; set; }

        public StringBuilder Process(object model)
        {
            Type modelType = model.GetType();
            PropertyInfo[] properties = modelType.GetProperties();
            var property = properties.FirstOrDefault(x => x.Name.ToLower() == this.PropertyName.ToLower());

            if (property == null)
            {
                throw new Exception($"Unable to locate the property ##{this.PropertyName}##");
            }

            var value = modelType.GetProperty(property.Name).GetValue(model);

            return new StringBuilder(value.ToString());
        }
    }
}
