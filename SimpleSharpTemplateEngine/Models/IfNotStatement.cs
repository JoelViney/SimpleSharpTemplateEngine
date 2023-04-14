using SimpleSharpTemplateEngine.Helpers;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class IfNotStatement : IIfStatementObject
    {
        public string PropertyName { get; }
        public ContainerObject Contents { get; }

        public IfNotStatement(string propertyName)
        {
            this.PropertyName = propertyName;
            this.Contents = new ContainerObject();
        }

        public bool MatchesExpression(object model)
        {
            var property = PropertyHelper.GetReferencedProperty(model, this.PropertyName);

            if (property is bool value)
            {
                return !value;
            }

            throw new TemplateEngineException($"The if variable '{this.PropertyName}' isn't a boolean.");

        }

        public StringBuilder Process(object model)
        {
            return this.Contents.Process(model);
        }
    }
}
