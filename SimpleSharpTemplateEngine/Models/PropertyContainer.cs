using SimpleSharpTemplateEngine.Helpers;
using System;
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
            var (property, format) = PropertyHelper.GetReferencedProperty(model, this.PropertyName);

            if (property == null)
            {
                return new StringBuilder();
            }

            if (format != null)
            {
                IFormattable formattable = property as IFormattable;

                if (formattable != null)
                {
                    var str = formattable.ToString(format, null);
                    return new StringBuilder(str);
                }
            }

            return new StringBuilder(property.ToString());
        }
    }
}
