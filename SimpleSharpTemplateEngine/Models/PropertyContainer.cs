using SimpleSharpTemplateEngine.Helpers;
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
            var obj = PropertyHelper.GetReferencedProperty(model, this.PropertyName);

            if (obj == null)
            {
                return new StringBuilder();
            }
            if (obj is string value)
            {
                return new StringBuilder(value);
            }
            else if (obj is int objValue)
            {
                return new StringBuilder(objValue.ToString());
            }

            throw new TemplateEngineException($"Reached the default case while processing the Property {this.PropertyName}");
        }
    }
}
