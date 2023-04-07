using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class IfObject : ITemplateObject
    {
        public string PropertyName { get; }
        public ContainerObject Contents { get; }

        public IfObject(string propertyName, ContainerObject contents)
        {
            this.PropertyName = propertyName;
            this.Contents = contents;            
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
            if (!typeof(bool).IsAssignableFrom(property.PropertyType))
            {
                throw new TemplateEngineException($"The if variable ##{this.PropertyName}## isn't a boolean.");
            }

            var value = property.GetValue(model) as bool?;

            if (value == true)
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
