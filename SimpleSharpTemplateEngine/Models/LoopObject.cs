using System.Collections;
using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class LoopObject : ITemplateObject
    {
        public string PropertyName { get; set; }
        public ContainerObject Contents { get; set; }

        public StringBuilder Process(object model)
        {
            Type modelType = model.GetType();
            PropertyInfo[] properties = modelType.GetProperties();
            var property = properties.FirstOrDefault(x => x.Name.ToLower() == this.PropertyName.ToLower());

            if (property == null)
                throw new Exception($"Unable to locate the property ##{this.PropertyName}##");

            if (!property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                throw new Exception($"The loop variable ##{this.PropertyName}## isn't Enumerable.");

            var enumerable = (IEnumerable)property.GetValue(model, null);

            var builder = new StringBuilder();
            foreach (var child in enumerable)
            {
                builder.Append(this.Contents.Process(child));
            }
            return builder;
        }
    }
}
