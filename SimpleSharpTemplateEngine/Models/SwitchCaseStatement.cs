using System;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal interface ISwitchCaseObject
    {
        bool MatchExpression(object value, string format);
        StringBuilder Process(object model);
    }

    internal class SwitchCaseStatement : ISwitchCaseObject, ITemplateObject
    {
        public string PropertyName { get; }
        public ContainerObject Contents { get; }

        public SwitchCaseStatement(string propertyName, ContainerObject contents)
        {
            this.PropertyName = propertyName;
            this.Contents = contents;
        }

        public bool MatchExpression(object value, string format)
        {
            if (value == null)
            {
                return this.PropertyName == "null"; // Match on null
            }

            if (format == null)
            {
                return this.PropertyName == value.ToString();
            }
            else
            {
                return String.Format(format, this.PropertyName) == String.Format(format, value);
            }
        }

        public StringBuilder Process(object model)
        {
            return this.Contents.Process(model);
        }
    }
}
