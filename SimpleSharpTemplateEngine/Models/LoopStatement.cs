using SimpleSharpTemplateEngine.Helpers;
using System.Collections;
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
            var (property, _) = PropertyHelper.GetReferencedProperty(model, this.PropertyName);

            if (property is IEnumerable enumerable)
            {
                var builder = new StringBuilder();
                foreach (var child in enumerable)
                {
                    builder.Append(this.Contents.Process(child));
                }
                return builder;
            }

            throw new TemplateEngineException($"The loop variable '{this.PropertyName}' isn't Enumerable.");
        }
    }
}
