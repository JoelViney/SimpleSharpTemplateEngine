using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal interface ISwitchCaseObject
    {
        bool MatchExpression(object? value);
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

        public bool MatchExpression(object? value)
        {
            if (value == null)
            {
                return this.PropertyName == "null"; // Match on null
            }

            return this.PropertyName == value.ToString();
        }

        public StringBuilder Process(object model)
        {
            return this.Contents.Process(model);
        }
    }
}
