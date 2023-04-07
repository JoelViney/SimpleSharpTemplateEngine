using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    // It is basically an IfNot with the inverse of the previous IF or IFNOT...
    internal class ElseObject : ITemplateObject
    {
        public string PropertyName { get; }
        public ContainerObject Contents { get; }

        public ElseObject(string propertyName, ContainerObject contents)
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
                throw new Exception($"Unable to locate the property ##{this.PropertyName}##");
            }
            if (!typeof(bool).IsAssignableFrom(property.PropertyType))
            {
                throw new Exception($"The if variable ##{this.PropertyName}## isn't a boolean.");
            }

            bool value = (bool)property.GetValue(model, null);

            if (!value)
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
