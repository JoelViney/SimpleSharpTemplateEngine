using System.Collections;
using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class LoopStatement : ITemplateObject
    {
        public string PropertyName { get; }
        public ContainerObject Contents { get; }

        public LoopStatement(string propertyName, ContainerObject contents)
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
                throw new TemplateEngineException($"Unable to locate the property ##{this.PropertyName}##");

            if (!property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                throw new TemplateEngineException($"The loop variable ##{this.PropertyName}## isn't Enumerable.");

            var enumerable = property.GetValue(model, null) as IEnumerable;

            var builder = new StringBuilder();

            if (enumerable != null)
            {
                foreach (var child in enumerable)
                {
                    builder.Append(this.Contents.Process(child));
                }
            }

            return builder;
        }
    }
}
