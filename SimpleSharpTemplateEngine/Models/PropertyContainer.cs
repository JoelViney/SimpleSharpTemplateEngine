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

        public StringBuilder Process(object model)
        {
            Type modelType = model.GetType();
            PropertyInfo[] properties = modelType.GetProperties();
            var property = properties.FirstOrDefault(x => x.Name.ToLower() == this.PropertyName.ToLower());

            if (property == null)
            {
                throw new TemplateEngineException($"Unable to locate the property ##{this.PropertyName}##");
            }

            var objValue = modelType.GetProperty(property.Name)!.GetValue(model);

            var value = objValue as string;
            if (value != null)
            {
                return new StringBuilder(value);
            }
            else
            {
                return new StringBuilder();
            }
        }
    }
}
